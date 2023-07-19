using System.Linq.Expressions;

namespace Spectre.Console.Cli.Completion;

/*
 Usage:
        return new CommandParameterMatcher<LionSettings>()
            .Add(x => x.Legs, (prefix) =>
            {
                if (prefix.Length != 0)
                {
                    return CompletionResult.Result(FindNextEvenNumber(prefix)).WithPreventDefault();
                }

                return CompletionResult.Result("16").WithPreventDefault();
            })
            .Add(x => x.Teeth, (prefix) =>
            {
                if (prefix.Length != 0)
                {
                    return CompletionResult.Result(FindNextEvenNumber(prefix)).WithPreventDefault();
                }

                return CompletionResult.Result("32").WithPreventDefault();
            })
            .Match(parameter, prefix);
 */

/// <summary>
/// Represents a command parameter matcher.
/// </summary>
/// <typeparam name="T">The settings type.</typeparam>
public class CommandParameterMatcher<T>
    where T : CommandSettings
{
    private readonly List<(PropertyInfo Property, Func<string, ICompletionResult> Func)> _completers;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandParameterMatcher{T}"/> class.
    /// </summary>
    public CommandParameterMatcher()
    {
        _completers = new();
    }

    private CommandParameterMatcher(IEnumerable<(PropertyInfo, Func<string, ICompletionResult>)>? completers)
    {
        _completers = completers?.ToList() ?? new();
    }

    /// <summary>
    /// Gets the suggestions for the specified parameter.
    /// </summary>
    /// <param name="parameter">Information on which parameter to get suggestions for.</param>
    /// <param name="prefix">The prefix.</param>
    /// <returns>The suggestions for the specified parameter.</returns>
    public ICompletionResult Match(ICommandParameterInfo parameter, string prefix)
    {
        var property = _completers.Find(x => x.Property.Name == parameter.PropertyName);
        if (property.Property == null)
        {
            return CompletionResult.None();
        }

        return property.Func(prefix);
    }

    /// <summary>
    /// Adds a completer for the specified property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <param name="completer">The completer.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public CommandParameterMatcher<T> Add(Expression<Func<T, object>> property, Func<string, ICompletionResult> completer)
    {
        var parameter = PropertyOf(property);
        _completers.Add((parameter, completer));
        return this;
    }

    /// <summary>
    /// Adds a completer for the specified property.
    /// </summary>
    /// <param name="completers">The completers.</param>
    /// <returns>A new instance of the <see cref="CommandParameterMatcher{T}"/> class.</returns>
    public static CommandParameterMatcher<T> Create(Dictionary<Expression<Func<T, object>>, Func<string, ICompletionResult>> completers)
    {
        var result = new List<(PropertyInfo, Func<string, ICompletionResult>)>();

        foreach (var completer in completers)
        {
            var parameter = PropertyOf(completer.Key);
            result.Add((parameter, completer.Value));
        }

        return new CommandParameterMatcher<T>(result);
    }

    /// <summary>
    /// Adds a completer for the specified property.
    /// </summary>
    /// <param name="completers">The completers.</param>
    /// <returns>A new instance of the <see cref="CommandParameterMatcher{T}"/> class.</returns>
    public static CommandParameterMatcher<T> Create(params (Expression<Func<T, object>>, Func<string, ICompletionResult>)[] completers)
    {
        var result = new List<(PropertyInfo, Func<string, ICompletionResult>)>();
        foreach (var (key, value) in completers)
        {
            var parameter = PropertyOf(key);
            result.Add((parameter, value));
        }

        return new CommandParameterMatcher<T>(result);
    }

    private static PropertyInfo PropertyOf(LambdaExpression methodExpression)
    {
        var body = RemoveConvert(methodExpression.Body) ?? methodExpression.Body;
        var prop = (MemberExpression)body;
        return (PropertyInfo)prop.Member;
    }

    private static Expression? RemoveConvert(Expression? expression)
    {
        while (
            expression != null
            && (
                expression.NodeType == ExpressionType.Convert
                || expression.NodeType == ExpressionType.ConvertChecked))
        {
            expression = RemoveConvert(((UnaryExpression)expression).Operand);
        }

        return expression;
    }
}