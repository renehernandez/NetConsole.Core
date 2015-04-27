namespace NetConsole.Core.Interfaces
{
    public interface ICommandInfo
    {

        bool HasDefaultCommand { get; }

        bool Match(string cmdAction = null, params string[] parametersTypes);

    }
}