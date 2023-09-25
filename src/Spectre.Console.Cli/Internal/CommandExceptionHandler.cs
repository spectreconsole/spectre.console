using Spectre.Console.Cli.Internal.Extensions;

namespace Spectre.Console.Cli.Internal;

internal static class CommandExceptionHandler
{
    public static bool HandleException(
        CommandTree leaf,
        CommandContext context,
        ITypeResolver resolver,
        Exception ex)
    {
        var args = new CommandExceptionArgs(context, ex, leaf?.Command?.CommandType);

        if (leaf?.Command?.CommandType == null)
        {
            return TryInvokeHandler(
                resolver,
                args,
                typeof(IEnumerable<ICommandExceptionHandler>),
                typeof(ICommandExceptionHandler));
        }

        var handlerType = typeof(ICommandExceptionHandler<>)
            .MakeGenericType(leaf.Command.CommandType);

        var enumerableHandlerType = typeof(IEnumerable<>)
            .MakeGenericType(handlerType);

        var handled = TryInvokeHandler(resolver, args, enumerableHandlerType, handlerType);
        if (handled)
        {
            return true;
        }

        return TryInvokeHandler(resolver, args, typeof(IEnumerable<ICommandExceptionHandler>),
            typeof(ICommandExceptionHandler));
    }

    private static bool TryInvokeHandler(
        ITypeResolver resolver,
        CommandExceptionArgs args,
        Type enumerableHandlerType,
        Type handlerType)
    {
        var handlers = resolver.TryResolve(enumerableHandlerType);
        if (handlers is IEnumerable<ICommandExceptionHandler> exHandlerEnumerable)
        {
            foreach (var exHandlerItem in exHandlerEnumerable)
            {
                var isHandled = exHandlerItem.Handle(args);
                if (isHandled)
                {
                    return true;
                }
            }
        }
        else
        {
            var handler = resolver.TryResolve(handlerType);
            if (handler is not ICommandExceptionHandler exHandler)
            {
                return false;
            }

            var isHandled = exHandler.Handle(args);
            if (isHandled)
            {
                return true;
            }
        }

        return false;
    }
}