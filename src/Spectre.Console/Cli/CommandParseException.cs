using System;
using System.Collections.Generic;
using System.Globalization;
using Spectre.Console.Rendering;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Represents errors that occur during parsing.
    /// </summary>
    public sealed class CommandParseException : CommandRuntimeException
    {
        internal CommandParseException(string message, IRenderable? pretty = null)
            : base(message, pretty)
        {
        }

        internal static CommandParseException CouldNotCreateSettings(Type settingsType)
        {
            return new CommandParseException($"Could not create settings of type '{settingsType.FullName}'.");
        }

        internal static CommandParseException CouldNotCreateCommand(Type? commandType)
        {
            if (commandType == null)
            {
                return new CommandParseException($"Could not create command. Command type is unknown.");
            }

            return new CommandParseException($"Could not create command of type '{commandType.FullName}'.");
        }

        internal static CommandParseException ExpectedTokenButFoundNull(CommandTreeToken.Kind expected)
        {
            return new CommandParseException($"Expected to find any token of type '{expected}' but found null instead.");
        }

        internal static CommandParseException ExpectedTokenButFoundOther(CommandTreeToken.Kind expected, CommandTreeToken.Kind found)
        {
            return new CommandParseException($"Expected to find token of type '{expected}' but found '{found}' instead.");
        }

        internal static CommandParseException OptionHasNoName(string input, CommandTreeToken token)
        {
            return CommandLineParseExceptionFactory.Create(input, token, "Option does not have a name.", "Did you forget the option name?");
        }

        internal static CommandParseException OptionValueWasExpected(string input, CommandTreeToken token)
        {
            return CommandLineParseExceptionFactory.Create(input, token, "Expected an option value.", "Did you forget the option value?");
        }

        internal static CommandParseException OptionHasNoValue(IEnumerable<string> args, CommandTreeToken token, CommandOption option)
        {
            return CommandLineParseExceptionFactory.Create(args, token, $"Option '{option.GetOptionName()}' is defined but no value has been provided.", "No value provided.");
        }

        internal static CommandParseException UnexpectedOption(IEnumerable<string> args, CommandTreeToken token)
        {
            return CommandLineParseExceptionFactory.Create(args, token, $"Unexpected option '{token.Value}'.", "Did you forget the command?");
        }

        internal static CommandParseException CannotAssignValueToFlag(IEnumerable<string> args, CommandTreeToken token)
        {
            return CommandLineParseExceptionFactory.Create(args, token, "Flags cannot be assigned a value.", "Can't assign value.");
        }

        internal static CommandParseException InvalidShortOptionName(string input, CommandTreeToken token)
        {
            return CommandLineParseExceptionFactory.Create(input, token, "Short option does not have a valid name.", "Not a valid name for a short option.");
        }

        internal static CommandParseException LongOptionNameIsMissing(TextBuffer reader, int position)
        {
            var token = new CommandTreeToken(CommandTreeToken.Kind.LongOption, position, string.Empty, "--");
            return CommandLineParseExceptionFactory.Create(reader.Original, token, "Invalid long option name.", "Did you forget the option name?");
        }

        internal static CommandParseException LongOptionNameIsOneCharacter(TextBuffer reader, int position, string name)
        {
            var token = new CommandTreeToken(CommandTreeToken.Kind.LongOption, position, name, $"--{name}");
            var reason = $"Did you mean -{name}?";
            return CommandLineParseExceptionFactory.Create(reader.Original, token, "Invalid long option name.", reason);
        }

        internal static CommandParseException LongOptionNameStartWithDigit(TextBuffer reader, int position, string name)
        {
            var token = new CommandTreeToken(CommandTreeToken.Kind.LongOption, position, name, $"--{name}");
            return CommandLineParseExceptionFactory.Create(reader.Original, token, "Invalid long option name.", "Option names cannot start with a digit.");
        }

        internal static CommandParseException LongOptionNameContainSymbol(TextBuffer reader, int position, char character)
        {
            var name = character.ToString(CultureInfo.InvariantCulture);
            var token = new CommandTreeToken(CommandTreeToken.Kind.LongOption, position, name, name);
            return CommandLineParseExceptionFactory.Create(reader.Original, token, "Invalid long option name.", "Invalid character.");
        }

        internal static CommandParseException UnterminatedQuote(string input, CommandTreeToken token)
        {
            return CommandLineParseExceptionFactory.Create(input, token, $"Encountered unterminated quoted string '{token.Value}'.", "Did you forget the closing quotation mark?");
        }

        internal static CommandParseException UnknownCommand(CommandModel model, CommandTree? node, IEnumerable<string> args, CommandTreeToken token)
        {
            var suggestion = CommandSuggestor.Suggest(model, node?.Command, token.Value);
            var text = suggestion != null ? $"Did you mean '{suggestion.Name}'?" : "No such command.";
            return CommandLineParseExceptionFactory.Create(args, token, $"Unknown command '{token.Value}'.", text);
        }

        internal static CommandParseException CouldNotMatchArgument(IEnumerable<string> args, CommandTreeToken token)
        {
            return CommandLineParseExceptionFactory.Create(args, token, $"Could not match '{token.Value}' with an argument.", "Could not match to argument.");
        }

        internal static CommandParseException UnknownOption(IEnumerable<string> args, CommandTreeToken token)
        {
            return CommandLineParseExceptionFactory.Create(args, token, $"Unknown option '{token.Value}'.", "Unknown option.");
        }

        internal static CommandParseException ValueIsNotInValidFormat(string value)
        {
            var text = $"[red]Error:[/] The value '[white]{value}[/]' is not in a correct format";
            return new CommandParseException("Could not parse value", new Markup(text));
        }
    }
}
