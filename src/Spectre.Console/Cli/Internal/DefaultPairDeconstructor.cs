namespace Spectre.Console.Cli;

[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class DefaultPairDeconstructor : IPairDeconstructor
{
    /// <inheritdoc/>
    (object? Key, object? Value) IPairDeconstructor.Deconstruct(
        ITypeResolver resolver,
        Type keyType,
        Type valueType,
        string? value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var parts = value.Split(new[] { '=' }, StringSplitOptions.None);
        if (parts.Length < 1 || parts.Length > 2)
        {
            throw CommandParseException.ValueIsNotInValidFormat(value);
        }

        var stringkey = parts[0];
        var stringValue = parts.Length == 2 ? parts[1] : null;
        if (stringValue == null)
        {
            // Got a default constructor?
            if (valueType.IsValueType)
            {
                // Get the string variant of a default instance.
                // Should not get null here, but compiler doesn't know that.
                stringValue = Activator.CreateInstance(valueType)?.ToString() ?? string.Empty;
            }
            else
            {
                // Try with an empty string.
                // Hopefully, the type converter knows how to convert it.
                stringValue = string.Empty;
            }
        }

        return (Parse(stringkey, keyType),
            Parse(stringValue, valueType));
    }

    private static object? Parse(string value, Type targetType)
    {
        try
        {
            var converter = GetConverter(targetType);
            return converter.ConvertFrom(value);
        }
        catch
        {
            // Can't convert something. Just give up and tell the user.
            throw CommandParseException.ValueIsNotInValidFormat(value);
        }
    }

    private static TypeConverter GetConverter(Type type)
    {
        var converter = TypeDescriptor.GetConverter(type);
        if (converter != null)
        {
            return converter;
        }

        throw new CommandConfigurationException($"Could find a type converter for '{type.FullName}'.");
    }
}