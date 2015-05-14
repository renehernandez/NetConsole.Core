using System.Reflection;

namespace NetConsole.Core.Interfaces
{
    public interface ICommandInfo
    {
        int Status { get; }

        string ReturnType { get; }

        MethodInfo Action { get; } 

        string Message { get; }
    }
}