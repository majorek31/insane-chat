using InsaneChat.AI.Tools;

namespace InsaneChat.CLI.Commands;

[Command("help", "Displays this help message.")]
public class HelpCommand(CommandManager commandManager, ToolManager toolManager) : ICommand
{
    public Task ExecuteAsync()
    {
        Console.WriteLine("Available commands:");
        foreach (var cmd in commandManager.GetCommandInfos())
        {
            Console.WriteLine($"   /{cmd.Name} - {cmd.Description}");
        }
        Console.WriteLine("Available tools:");
        foreach (var tool in toolManager.Tools)
        {
            Console.WriteLine($"   {tool.Name} - {tool.Description}");
        }
        return Task.CompletedTask;
    }
}