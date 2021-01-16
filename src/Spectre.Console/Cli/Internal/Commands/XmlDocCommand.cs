using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace Spectre.Console.Cli
{
    [Description("Generates an XML representation of the CLI configuration.")]
    [SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
    internal sealed class XmlDocCommand : Command<XmlDocCommand.Settings>
    {
        private readonly CommandModel _model;
        private readonly IAnsiConsole _writer;

        public XmlDocCommand(IConfiguration configuration, CommandModel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _writer = configuration.Settings.Console.GetConsole();
        }

        public sealed class Settings : CommandSettings
        {
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            _writer.Write(Serialize(_model), Style.Plain);
            return 0;
        }

        public static string Serialize(CommandModel model)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\n",
                OmitXmlDeclaration = false,
                Encoding = Encoding.UTF8,
            };

            using (var buffer = new StringWriterWithEncoding(Encoding.UTF8))
            using (var xmlWriter = XmlWriter.Create(buffer, settings))
            {
                SerializeModel(model).WriteTo(xmlWriter);
                xmlWriter.Flush();
                return buffer.GetStringBuilder().ToString();
            }
        }

        private static XmlDocument SerializeModel(CommandModel model)
        {
            var document = new XmlDocument();
            var root = document.CreateElement("Model");

            if (model.DefaultCommand != null)
            {
                root.AppendChild(document.CreateComment("DEFAULT COMMAND"));
                root.AppendChild(CreateCommandNode(document, model.DefaultCommand, isDefaultCommand: true));
            }

            foreach (var command in model.Commands.Where(x => !x.IsHidden))
            {
                root.AppendChild(document.CreateComment(command.Name.ToUpperInvariant()));
                root.AppendChild(CreateCommandNode(document, command));
            }

            document.AppendChild(root);
            return document;
        }

        private static XmlNode CreateCommandNode(XmlDocument doc, CommandInfo command, bool isDefaultCommand = false)
        {
            var node = doc.CreateElement("Command");

            // Attributes
            node.SetNullableAttribute("Name", command.Name);
            node.SetBooleanAttribute("IsBranch", command.IsBranch);

            if (isDefaultCommand)
            {
                node.SetBooleanAttribute("IsDefault", true);
            }

            if (command.CommandType != null)
            {
                node.SetNullableAttribute("ClrType", command.CommandType?.FullName);
            }

            node.SetNullableAttribute("Settings", command.SettingsType?.FullName);

            // Parameters
            if (command.Parameters.Count > 0)
            {
                var parameterRootNode = doc.CreateElement("Parameters");
                foreach (var parameter in CreateParameterNodes(doc, command))
                {
                    parameterRootNode.AppendChild(parameter);
                }

                node.AppendChild(parameterRootNode);
            }

            // Commands
            foreach (var childCommand in command.Children)
            {
                node.AppendChild(doc.CreateComment(childCommand.Name.ToUpperInvariant()));
                node.AppendChild(CreateCommandNode(doc, childCommand));
            }

            return node;
        }

        private static IEnumerable<XmlNode> CreateParameterNodes(XmlDocument document, CommandInfo command)
        {
            // Arguments
            foreach (var argument in command.Parameters.OfType<CommandArgument>().OrderBy(x => x.Position))
            {
                var node = document.CreateElement("Argument");
                node.SetNullableAttribute("Name", argument.Value);
                node.SetAttribute("Position", argument.Position.ToString(CultureInfo.InvariantCulture));
                node.SetBooleanAttribute("Required", argument.Required);
                node.SetEnumAttribute("Kind", argument.ParameterKind);
                node.SetNullableAttribute("ClrType", argument.ParameterType?.FullName);

                if (!string.IsNullOrWhiteSpace(argument.Description))
                {
                    var descriptionNode = document.CreateElement("Description");
                    descriptionNode.InnerText = argument.Description;
                    node.AppendChild(descriptionNode);
                }

                if (argument.Validators.Count > 0)
                {
                    var validatorRootNode = document.CreateElement("Validators");
                    foreach (var validator in argument.Validators.OrderBy(x => x.GetType().FullName))
                    {
                        var validatorNode = document.CreateElement("Validator");
                        validatorNode.SetNullableAttribute("ClrType", validator.GetType().FullName);
                        validatorNode.SetNullableAttribute("Message", validator.ErrorMessage);
                        validatorRootNode.AppendChild(validatorNode);
                    }

                    node.AppendChild(validatorRootNode);
                }

                yield return node;
            }

            // Options
            foreach (var option in command.Parameters.OfType<CommandOption>()
                .OrderBy(x => string.Join(",", x.LongNames))
                .ThenBy(x => string.Join(",", x.ShortNames)))
            {
                var node = document.CreateElement("Option");

                if (option.IsShadowed)
                {
                    node.SetBooleanAttribute("Shadowed", true);
                }

                node.SetNullableAttribute("Short", option.ShortNames);
                node.SetNullableAttribute("Long", option.LongNames);
                node.SetNullableAttribute("Value", option.ValueName);
                node.SetBooleanAttribute("Required", option.Required);
                node.SetEnumAttribute("Kind", option.ParameterKind);
                node.SetNullableAttribute("ClrType", option.ParameterType?.FullName);

                if (!string.IsNullOrWhiteSpace(option.Description))
                {
                    var descriptionNode = document.CreateElement("Description");
                    descriptionNode.InnerText = option.Description;
                    node.AppendChild(descriptionNode);
                }

                yield return node;
            }
        }
    }
}
