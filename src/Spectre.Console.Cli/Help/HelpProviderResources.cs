using System.Resources;

namespace Spectre.Console.Cli.Help;

/// <summary>
/// A strongly-typed resource class, for looking up localized strings, etc.
/// </summary>
internal class HelpProviderResources
{
    private readonly ResourceManager resourceManager = new ResourceManager("Spectre.Console.Cli.Resources.HelpProvider", typeof(HelpProvider).Assembly);
    private readonly CultureInfo? resourceCulture = null;

    public HelpProviderResources()
    {
    }

    public HelpProviderResources(CultureInfo? culture)
    {
        resourceCulture = culture;
    }

    /// <summary>
    /// Gets the localised string for ARGUMENTS.
    /// </summary>
    internal string Arguments
    {
        get
        {
            return resourceManager.GetString("Arguments", resourceCulture) ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the localised string for COMMAND.
    /// </summary>
    internal string Command
    {
        get
        {
            return resourceManager.GetString("Command", resourceCulture) ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the localised string for COMMANDS.
    /// </summary>
    internal string Commands
    {
        get
        {
            return resourceManager.GetString("Commands", resourceCulture) ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the localised string for DEFAULT.
    /// </summary>
    internal string Default
    {
        get
        {
            return resourceManager.GetString("Default", resourceCulture) ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the localised string for DESCRIPTION.
    /// </summary>
    internal string Description
    {
        get
        {
            return resourceManager.GetString("Description", resourceCulture) ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the localised string for EXAMPLES.
    /// </summary>
    internal string Examples
    {
        get
        {
            return resourceManager.GetString("Examples", resourceCulture) ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the localised string for OPTIONS.
    /// </summary>
    internal string Options
    {
        get
        {
            return resourceManager.GetString("Options", resourceCulture) ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the localised string for Prints help information.
    /// </summary>
    internal string PrintHelpDescription
    {
        get
        {
            return resourceManager.GetString("PrintHelpDescription", resourceCulture) ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the localised string for Prints version information.
    /// </summary>
    internal string PrintVersionDescription
    {
        get
        {
            return resourceManager.GetString("PrintVersionDescription", resourceCulture) ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the localised string for USAGE.
    /// </summary>
    internal string Usage
    {
        get
        {
            return resourceManager.GetString("Usage", resourceCulture) ?? string.Empty;
        }
    }
}
