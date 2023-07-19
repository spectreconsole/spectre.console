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

public class CommandParameterMatcher<T>
    where T : CommandSettings
{
    private readonly List<(PropertyInfo Property, Func<string, ICompletionResult> Func)> _completers;

    public CommandParameterMatcher()
    {
        _completers = new();
    }

    private CommandParameterMatcher(IEnumerable<(PropertyInfo, Func<string, ICompletionResult>)>? completers)
    {
        _completers = completers?.ToList() ?? new();
    }

    public ICompletionResult Match(ICommandParameterInfo parameter, string prefix)
    {
        var property = _completers.FirstOrDefault(x => x.Property.Name == parameter.PropertyName);
        if (property.Property == null)
        {
            return CompletionResult.None();
        }

        return property.Func(prefix);
    }

    public CommandParameterMatcher<T> Add(Expression<Func<T, object>> property, Func<string, ICompletionResult> completer)
    {
        var parameter = PropertyOf(property);
        _completers.Add((parameter, completer));
        return this;
    }

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

    // params create
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
        var body = RemoveConvert(methodExpression.Body);
        var prop = (MemberExpression)body;
        return (PropertyInfo)prop.Member;
    }

    private static Expression RemoveConvert(Expression expression)
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