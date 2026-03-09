using Microsoft.Extensions.DependencyInjection;
using InsaneChat.Extensions;
using InsaneChat.CLI;

var serviceProvider = new ServiceCollection()
    .AddCommands()
    .BuildServiceProvider();

var commandManager = serviceProvider.GetRequiredService<CommandManager>();
var commands = serviceProvider.GetServices<ICommand>();
commandManager.SetCommands(commands);

while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input)) continue;

    if (input.StartsWith("/"))
    {
        input = input.Substring(1); // Remove the leading '/'
        await commandManager.ExecuteCommandAsync(input);
        continue;
    }

}