namespace InsaneChat.CLI.Commands;

public class ExitCommand : ICommand
{
    public string Name => "exit";
    public string Description => "Exits the application.";

    public Task ExecuteAsync()
    {
        Console.WriteLine("Exiting...");
        Environment.Exit(0);
        return Task.CompletedTask;
    }
}