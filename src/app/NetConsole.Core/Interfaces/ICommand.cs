namespace NetConsole.Core.Interfaces
{
    public interface ICommand
    {
        int Status { get; }

        string Name { get; }

        string Overview { get; }

    }
}