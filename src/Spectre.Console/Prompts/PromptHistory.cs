namespace Spectre.Console;

/// <summary>
/// Stores the history of valid prompt input entries.
/// </summary>
public sealed class PromptHistory
{
    private readonly List<string> _entries;

    /// <summary>
    /// Gets the default shared prompt history instance.
    /// </summary>
    public static PromptHistory Default { get; } = new PromptHistory();

    /// <summary>
    /// Gets or sets a value indicating whether history is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether secret input should be ignored.
    /// </summary>
    public bool IgnoreSecret { get; set; } = true;

    /// <summary>
    /// Occurs when a new entry is added to history.
    /// </summary>
    public event EventHandler<string>? EntryAdded;

    /// <summary>
    /// Gets the history entries.
    /// </summary>
    public IReadOnlyList<string> Entries => _entries.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the <see cref="PromptHistory"/> class.
    /// </summary>
    /// <param name="capacity">The initial capacity of the history list.</param>
    public PromptHistory(int capacity = 32)
    {
        _entries = new List<string>(capacity);
    }

    /// <summary>
    /// Adds a new entry to the prompt history.
    /// </summary>
    /// <param name="entry">The user input entry.</param>
    public void Add(string entry)
    {
        ArgumentNullException.ThrowIfNull(entry);

        if (string.IsNullOrWhiteSpace(entry))
        {
            return;
        }

        _entries.Add(entry);
        EntryAdded?.Invoke(this, entry);
    }

    /// <summary>
    /// Clears the prompt history.
    /// </summary>
    public void Clear()
    {
        _entries.Clear();
    }
}
