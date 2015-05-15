using System.Reflection;

namespace NetConsole.Core.Interfaces
{
    public interface IAction
    {

        ICommand Command { get; }

        int Status { get; }

        MethodInfo[] Body { get; } 

        string Message { get; }
    }
}