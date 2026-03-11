using Microsoft.Extensions.DependencyInjection;
using InsaneChat.Extensions;
using InsaneChat.CLI;
using Microsoft.Extensions.Configuration;
using InsaneChat.AI;

using InsaneChat.AI.Tools;
using ModelContextProtocol.Client;


Console.WriteLine(Directory.GetCurrentDirectory());
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("/workspaces/dotnet-postgres/InsaneChat/appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .Build();


var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(configuration)
    .AddCommands()
    .AddSingleton<McpClient>(sp =>
    {
        return McpClient.CreateAsync(new HttpClientTransport(new HttpClientTransportOptions
        {
            Endpoint = new Uri("https://api.microchip.com/mcp/resources"),
        })).GetAwaiter().GetResult();
    })
    .AddCoreAI()
    .BuildServiceProvider();

var commandManager = serviceProvider.GetRequiredService<CommandManager>();

var toolManager = serviceProvider.GetRequiredService<ToolManager>();
await toolManager.LoadTools();

Console.WriteLine("Welcome to InsaneChat CLI! Type '/help' for a list of commands.");

var chatSession = serviceProvider.GetRequiredService<ChatSession>();

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
    var response = await chatSession.SendMessageAsync(input);
    Console.WriteLine(response);
}