
namespace NetConsole.Core.Interfaces
{
    public interface IReturnInfo
    {
        int Status { get; }

        string Type { get; }

        object Output { get; }

    }
}