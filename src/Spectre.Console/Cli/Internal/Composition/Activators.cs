using System;
using System.Collections.Generic;
using System.Reflection;

namespace Spectre.Console.Cli
{
    internal abstract class ComponentActivator
    {
        public abstract object Activate(DefaultTypeResolver container);

        public abstract ComponentActivator CreateCopy();
    }

    internal class CachingActivator : ComponentActivator
    {
        private readonly ComponentActivator _activator;
        private object? _result;

        public CachingActivator(ComponentActivator activator)
        {
            _activator = activator ?? throw new ArgumentNullException(nameof(activator));
            _result = null;
        }

        public override object Activate(DefaultTypeResolver container)
        {
            return _result ??= _activator.Activate(container);
        }

        public override ComponentActivator CreateCopy()
        {
            return new CachingActivator(_activator.CreateCopy());
        }
    }

    internal sealed class InstanceActivator : ComponentActivator
    {
        private readonly object _instance;

        public InstanceActivator(object instance)
        {
            _instance = instance;
        }

        public override object Activate(DefaultTypeResolver container)
        {
            return _instance;
        }

        public override ComponentActivator CreateCopy()
        {
            return new InstanceActivator(_instance);
        }
    }

    internal sealed class ReflectionActivator : ComponentActivator
    {
        private readonly Type _type;
        private readonly ConstructorInfo _constructor;
        private readonly List<ParameterInfo> _parameters;

        public ReflectionActivator(Type type)
        {
            _type = type;
            _constructor = GetGreediestConstructor(type);
            _parameters = new List<ParameterInfo>();

            foreach (var parameter in _constructor.GetParameters())
            {
                _parameters.Add(parameter);
            }
        }

        public override object Activate(DefaultTypeResolver container)
        {
            var parameters = new object?[_parameters.Count];
            for (var i = 0; i < _parameters.Count; i++)
            {
                var parameter = _parameters[i];
                if (parameter.ParameterType == typeof(DefaultTypeResolver))
                {
                    parameters[i] = container;
                }
                else
                {
                    var resolved = container.Resolve(parameter.ParameterType);
                    if (resolved == null)
                    {
                        if (!parameter.IsOptional)
                        {
                            throw new InvalidOperationException($"Could not find registration for '{parameter.ParameterType.FullName}'.");
                        }

                        parameters[i] = null;
                    }
                    else
                    {
                        parameters[i] = resolved;
                    }
                }
            }

            return _constructor.Invoke(parameters);
        }

        public override ComponentActivator CreateCopy()
        {
            return new ReflectionActivator(_type);
        }

        private static ConstructorInfo GetGreediestConstructor(Type type)
        {
            ConstructorInfo? current = null;
            var count = -1;
            foreach (var constructor in type.GetTypeInfo().GetConstructors())
            {
                var parameters = constructor.GetParameters();
                if (parameters.Length > count)
                {
                    count = parameters.Length;
                    current = constructor;
                }
            }

            if (current == null)
            {
                throw new InvalidOperationException($"Could not find a constructor for '{type.FullName}'.");
            }

            return current;
        }
    }
}