using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Spectre.Console.Cli
{
    internal abstract class CommandParameter : ICommandParameterInfo
    {
        public Guid Id { get; }
        public Type ParameterType { get; }
        public ParameterKind ParameterKind { get; }
        public PropertyInfo Property { get; }
        public string? Description { get; }
        public DefaultValueAttribute? DefaultValue { get; }
        public TypeConverterAttribute? Converter { get; }
        public PairDeconstructorAttribute? PairDeconstructor { get; }
        public List<ParameterValidationAttribute> Validators { get; }
        public bool Required { get; set; }
        public string PropertyName => Property.Name;

        public virtual bool WantRawValue => ParameterType.IsPairDeconstructable()
            && (PairDeconstructor != null || Converter == null);

        protected CommandParameter(
            Type parameterType, ParameterKind parameterKind, PropertyInfo property,
            string? description, TypeConverterAttribute? converter,
            DefaultValueAttribute? defaultValue,
            PairDeconstructorAttribute? deconstuctor,
            IEnumerable<ParameterValidationAttribute> validators, bool required)
        {
            Id = Guid.NewGuid();
            ParameterType = parameterType;
            ParameterKind = parameterKind;
            Property = property;
            Description = description;
            Converter = converter;
            DefaultValue = defaultValue;
            PairDeconstructor = deconstuctor;
            Validators = new List<ParameterValidationAttribute>(validators ?? Array.Empty<ParameterValidationAttribute>());
            Required = required;
        }

        public bool IsFlagValue()
        {
            return ParameterType.GetInterfaces().Any(i => i == typeof(IFlagValue));
        }

        public bool HaveSameBackingPropertyAs(CommandParameter other)
        {
            return CommandParameterComparer.ByBackingProperty.Equals(this, other);
        }

        public void Assign(CommandSettings settings, ITypeResolver resolver, object? value)
        {
            // Is the property pair deconstructable?
            // TODO: This needs to be better defined
            if (Property.PropertyType.IsPairDeconstructable() && WantRawValue)
            {
                var genericTypes = Property.PropertyType.GetGenericArguments();

                var multimap = (IMultiMap?)Property.GetValue(settings);
                if (multimap == null)
                {
                    multimap = Activator.CreateInstance(typeof(MultiMap<,>).MakeGenericType(genericTypes[0], genericTypes[1])) as IMultiMap;
                    if (multimap == null)
                    {
                        throw new InvalidOperationException("Could not create multimap");
                    }
                }

                // Create deconstructor.
                var deconstructorType = PairDeconstructor?.Type ?? typeof(DefaultPairDeconstructor);
                if (!(resolver.Resolve(deconstructorType) is IPairDeconstructor deconstructor))
                {
                    if (!(Activator.CreateInstance(deconstructorType) is IPairDeconstructor activatedDeconstructor))
                    {
                        throw new InvalidOperationException($"Could not create pair deconstructor.");
                    }

                    deconstructor = activatedDeconstructor;
                }

                // Deconstruct and add to multimap.
                var pair = deconstructor.Deconstruct(resolver, genericTypes[0], genericTypes[1], value as string);
                if (pair.Key != null)
                {
                    multimap.Add(pair);
                }

                value = multimap;
            }
            else if (Property.PropertyType.IsArray)
            {
                // Add a new item to the array
                var array = (Array?)Property.GetValue(settings);
                Array newArray;

                var elementType = Property.PropertyType.GetElementType();
                if (elementType == null)
                {
                    throw new InvalidOperationException("Could not get property type.");
                }

                if (array == null)
                {
                    newArray = Array.CreateInstance(elementType, 1);
                }
                else
                {
                    newArray = Array.CreateInstance(elementType, array.Length + 1);
                    array.CopyTo(newArray, 0);
                }

                newArray.SetValue(value, newArray.Length - 1);
                value = newArray;
            }
            else if (IsFlagValue())
            {
                var flagValue = (IFlagValue?)Property.GetValue(settings);
                if (flagValue == null)
                {
                    flagValue = (IFlagValue?)Activator.CreateInstance(ParameterType);
                    if (flagValue == null)
                    {
                        throw new InvalidOperationException("Could not create flag value.");
                    }
                }

                if (value != null)
                {
                    // Null means set, but not with a valid value.
                    flagValue.Value = value;
                }

                // If the parameter was mapped, then it's set.
                flagValue.IsSet = true;

                value = flagValue;
            }

            Property.SetValue(settings, value);
        }

        public object? Get(CommandSettings settings)
        {
            return Property.GetValue(settings);
        }
    }
}