namespace Spectre.Console;

internal static class ExceptionConverter
{
    public static ExceptionInfo Convert(Exception exception)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        var exceptionType = exception.GetType();
        var stackTrace = new StackTrace(exception, true);
        var frames = stackTrace.GetFrames().Where(f => f != null).Cast<StackFrame>().Select(Convert).ToList();
        var inner = exception.InnerException is null ? null : Convert(exception.InnerException);
        return new ExceptionInfo(exceptionType.FullName ?? exceptionType.Name, exception.Message, frames, inner);
    }

    private static StackFrameInfo Convert(StackFrame frame)
    {
        var method = frame.GetMethod();
        if (method is null)
        {
            return new StackFrameInfo("<unknown method>", new List<(string Type, string Name)>(), null, null);
        }

        var methodName = GetMethodName(method);
        var parameters = method.GetParameters().Select(e => (e.ParameterType.Name, e.Name ?? string.Empty)).ToList();
        var path = frame.GetFileName();
        var lineNumber = frame.GetFileLineNumber();
        return new StackFrameInfo(methodName, parameters, path, lineNumber == 0 ? null : lineNumber);
    }

    private static string GetMethodName(MethodBase method)
    {
        var builder = new StringBuilder(256);

        var fullName = method.DeclaringType?.FullName;
        if (fullName != null)
        {
            // See https://github.com/dotnet/runtime/blob/v6.0.0/src/libraries/System.Private.CoreLib/src/System/Diagnostics/StackTrace.cs#L247-L253
            builder.Append(fullName.Replace('+', '.'));
            builder.Append('.');
        }

        builder.Append(method.Name);

        if (method.IsGenericMethod)
        {
            builder.Append('[');
            builder.Append(string.Join(",", method.GetGenericArguments().Select(t => t.Name)));
            builder.Append(']');
        }

        return builder.ToString();
    }
}