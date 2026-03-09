namespace InsaneChat.CLI.Commands;

public class HelpCommand(CommandManager commandManager) : ICommand
{
    public string Name => "help";

    public string Description => "Displays a list of available commands.";

    public Task ExecuteAsync()
    {
        Console.WriteLine("Available commands:");
        foreach (var command in commandManager.GetCommands())
        {
            Console.WriteLine($"  {command.Name} - {command.Description}");
        }

        return Task.CompletedTask;
    }
}