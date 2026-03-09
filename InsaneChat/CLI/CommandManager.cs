namespace InsaneChat.CLI;

public class CommandManager
{
    private IEnumerable<ICommand> _commands = new List<ICommand>();

    public void SetCommands(IEnumerable<ICommand> commands)
    {
        _commands = commands;
    }

    public IEnumerable<ICommand> GetCommands() => _commands;

    public async Task ExecuteCommandAsync(string commandName)
    {
        var command = _commands.FirstOrDefault(c => c.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));
        if (command == null)
        {
            Console.WriteLine($"Unknown command: {commandName}");
            Console.WriteLine("Type '/help' for a list of available commands.");
            return;
        }

        await command.ExecuteAsync();
    }
}