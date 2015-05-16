using System.Reflection;
using NetConsole.Core.Grammar;

namespace NetConsole.Core.Interfaces
{
    public interface IAction
    {

        int Status { get; }

        string Message { get; }

        object[] Perform();
    }
}