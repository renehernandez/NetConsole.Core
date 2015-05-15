using System.Reflection;

namespace NetConsole.Core.Interfaces
{
    public interface IAction
    {

        int Status { get; }

        string Message { get; }

        object[] Perform();
    }
}