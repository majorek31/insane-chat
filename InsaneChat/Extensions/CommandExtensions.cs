using System.Reflection;
using InsaneChat.CLI;
using Microsoft.Extensions.DependencyInjection;

namespace InsaneChat.Extensions;

public static class CommandExtensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddSingleton<CommandManager>();

        var commandTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(ICommand).IsAssignableFrom(t));

        foreach (var commandType in commandTypes)
        {
            services.AddTransient(typeof(ICommand), commandType);
        }

        return services;
    }
}