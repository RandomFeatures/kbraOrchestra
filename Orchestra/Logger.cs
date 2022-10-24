using System.Runtime.CompilerServices;

namespace Orchestra;

public static class Logger
{
    public static void Log(object message, [CallerMemberName] string? callerName = null)
    {
        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd h:mm:ss.fff tt} => Caller: {callerName} / Message: {message}");
    }
}