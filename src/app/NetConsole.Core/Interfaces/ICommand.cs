namespace NetConsole.Core.Interfaces
{
    public interface ICommand : IRegistrable
    {

        IOptionAccessor Accessor { get; }

        int Status { get; }

        string Overview { get; }

    }
}