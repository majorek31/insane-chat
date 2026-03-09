namespace InsaneChat.CLI.Commands;

[Command("help", "Displays this help message.")]
public class HelpCommand(CommandManager commandManager) : ICommand
{
    public Task ExecuteAsync()
    {
        Console.WriteLine("Available commands:");
        foreach (var cmd in commandManager.GetCommandInfos())
        {
            Console.WriteLine($"   /{cmd.Name} - {cmd.Description}");
        }
        return Task.CompletedTask;
    }
}