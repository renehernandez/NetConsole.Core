namespace NetConsole.Core.Interfaces
{
    public interface ICommand
    {

        IOptionAccessor Accessor { get; }

        int Status { get; }

        string Name { get; }

        string Overview { get; }

    }
}