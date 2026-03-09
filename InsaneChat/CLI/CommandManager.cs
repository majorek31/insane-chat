using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace InsaneChat.CLI;

public class CommandManager
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, CommandInfo> _commands = new();

    public CommandManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        var commandTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(ICommand).IsAssignableFrom(t));

        foreach (var type in commandTypes)
        {
            var attr = type.GetCustomAttribute<CommandAttribute>();
            if (attr == null)
                continue;
            var commandInfo = new CommandInfo(attr.Name, attr.Description, type);
            _commands[attr.Name] = commandInfo;
        }
    }

    public IEnumerable<CommandInfo> GetCommandInfos() => _commands.Values;

    public async Task ExecuteCommandAsync(string commandName)
    {
        if (!_commands.TryGetValue(commandName, out var commandInfo))
        {
            Console.WriteLine($"Unknown command: {commandName}");
            Console.WriteLine("Type '/help' to see available commands.");
            return;
        }

        var command = (ICommand)_serviceProvider.GetRequiredService(commandInfo.CommandType);
        await command.ExecuteAsync();
    }
}