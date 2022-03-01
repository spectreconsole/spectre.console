using System;

namespace Showcase;

public static class ExceptionGenerator
{
    public static Exception GenerateException()
    {
        try
        {
            SomeOperation();
            throw new InvalidOperationException();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    private static void SomeOperation()
    {
        SomeOperationGoingWrong();
    }

    private static void SomeOperationGoingWrong()
    {
        throw new InvalidOperationException("Something went very wrong!");
    }
}
