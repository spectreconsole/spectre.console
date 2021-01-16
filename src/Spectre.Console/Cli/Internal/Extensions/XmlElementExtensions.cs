using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Spectre.Console.Cli
{
    internal static class XmlElementExtensions
    {
        public static void SetNullableAttribute(this XmlElement element, string name, string? value)
        {
            element.SetAttribute(name, value ?? "NULL");
        }

        public static void SetNullableAttribute(this XmlElement element, string name, IEnumerable<string>? values)
        {
            if (values?.Any() != true)
            {
                element.SetAttribute(name, "NULL");
            }

            element.SetAttribute(name, string.Join(",", values ?? Enumerable.Empty<string>()));
        }

        public static void SetBooleanAttribute(this XmlElement element, string name, bool value)
        {
            element.SetAttribute(name, value ? "true" : "false");
        }

        public static void SetEnumAttribute(this XmlElement element, string name, Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                var attribute = field.GetCustomAttribute<DescriptionAttribute>(false);
                if (attribute == null)
                {
                    throw new InvalidOperationException("Enum is missing description.");
                }

                element.SetAttribute(name, attribute.Description);
            }
        }
    }
}
