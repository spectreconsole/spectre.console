using System.Globalization;
using Spectre.Console.Rendering;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Represents errors related to parameter templates.
    /// </summary>
    public sealed class CommandTemplateException : CommandConfigurationException
    {
        /// <summary>
        /// Gets the template that contains the error.
        /// </summary>
        public string Template { get; }

        internal override bool AlwaysPropagateWhenDebugging => true;

        internal CommandTemplateException(string message, string template, IRenderable pretty)
            : base(message, pretty)
        {
            Template = template;
        }

        internal static CommandTemplateException UnexpectedCharacter(string template, int position, char character)
        {
            return CommandLineTemplateExceptionFactory.Create(
                template,
                new TemplateToken(TemplateToken.Kind.Unknown, position, $"{character}", $"{character}"),
                $"Encountered unexpected character '{character}'.",
                "Unexpected character.");
        }

        internal static CommandTemplateException UnterminatedValueName(string template, TemplateToken token)
        {
            return CommandLineTemplateExceptionFactory.Create(
                template, token,
                $"Encountered unterminated value name '{token.Value}'.",
                "Unterminated value name.");
        }

        internal static CommandTemplateException ArgumentCannotContainOptions(string template, TemplateToken token)
        {
            return CommandLineTemplateExceptionFactory.Create(
                template, token,
                "Arguments can not contain options.",
                "Not permitted.");
        }

        internal static CommandTemplateException MultipleValuesAreNotSupported(string template, TemplateToken token)
        {
            return CommandLineTemplateExceptionFactory.Create(template, token,
                "Multiple values are not supported.",
                "Too many values.");
        }

        internal static CommandTemplateException ValuesMustHaveName(string template, TemplateToken token)
        {
            return CommandLineTemplateExceptionFactory.Create(template, token,
                "Values without name are not allowed.",
                "Missing value name.");
        }

        internal static CommandTemplateException OptionsMustHaveName(string template, TemplateToken token)
        {
            return CommandLineTemplateExceptionFactory.Create(template, token,
                "Options without name are not allowed.",
                "Missing option name.");
        }

        internal static CommandTemplateException OptionNamesCannotStartWithDigit(string template, TemplateToken token)
        {
            // Rewrite the token to point to the option name instead of the whole string.
            token = new TemplateToken(
                token.TokenKind,
                token.TokenKind == TemplateToken.Kind.ShortName ? token.Position + 1 : token.Position + 2,
                token.Value, token.Value);

            return CommandLineTemplateExceptionFactory.Create(template, token,
                "Option names cannot start with a digit.",
                "Invalid option name.");
        }

        internal static CommandTemplateException InvalidCharacterInOptionName(string template, TemplateToken token, char character)
        {
            // Rewrite the token to point to the invalid character instead of the whole value.
            var position = (token.TokenKind == TemplateToken.Kind.ShortName
                ? token.Position + 1
                : token.Position + 2) + token.Value.OrdinalIndexOf(character);

            token = new TemplateToken(
                token.TokenKind, position,
                token.Value, character.ToString(CultureInfo.InvariantCulture));

            return CommandLineTemplateExceptionFactory.Create(template, token,
                $"Encountered invalid character '{character}' in option name.",
                "Invalid character.");
        }

        internal static CommandTemplateException LongOptionMustHaveMoreThanOneCharacter(string template, TemplateToken token)
        {
            // Rewrite the token to point to the option name instead of the whole option.
            token = new TemplateToken(token.TokenKind, token.Position + 2, token.Value, token.Value);

            return CommandLineTemplateExceptionFactory.Create(template, token,
                "Long option names must consist of more than one character.",
                "Invalid option name.");
        }

        internal static CommandTemplateException MultipleShortOptionNamesNotAllowed(string template, TemplateToken token)
        {
            return CommandLineTemplateExceptionFactory.Create(template, token,
                "Multiple short option names are not supported.",
                "Too many short options.");
        }

        internal static CommandTemplateException ShortOptionMustOnlyBeOneCharacter(string template, TemplateToken token)
        {
            // Rewrite the token to point to the option name instead of the whole option.
            token = new TemplateToken(token.TokenKind, token.Position + 1, token.Value, token.Value);

            return CommandLineTemplateExceptionFactory.Create(template, token,
                "Short option names can not be longer than one character.",
                "Invalid option name.");
        }

        internal static CommandTemplateException MultipleOptionValuesAreNotSupported(string template, TemplateToken token)
        {
            return CommandLineTemplateExceptionFactory.Create(template, token,
                "Multiple option values are not supported.",
                "Too many option values.");
        }

        internal static CommandTemplateException InvalidCharacterInValueName(string template, TemplateToken token, char character)
        {
            // Rewrite the token to point to the invalid character instead of the whole value.
            token = new TemplateToken(
                token.TokenKind,
                token.Position + 1 + token.Value.OrdinalIndexOf(character),
                token.Value, character.ToString(CultureInfo.InvariantCulture));

            return CommandLineTemplateExceptionFactory.Create(template, token,
                $"Encountered invalid character '{character}' in value name.",
                "Invalid character.");
        }

        internal static CommandTemplateException MissingLongAndShortName(string template, TemplateToken? token)
        {
            return CommandLineTemplateExceptionFactory.Create(template, token,
                "No long or short name for option has been specified.",
                "Missing option. Was this meant to be an argument?");
        }

        internal static CommandTemplateException ArgumentsMustHaveValueName(string template)
        {
            return CommandLineTemplateExceptionFactory.Create(template, null,
                "Arguments must have a value name.",
                "Missing value name.");
        }
    }
}
