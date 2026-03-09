using InsaneChat.CLI;
using Microsoft.Extensions.DependencyInjection;

namespace InsaneChat.Extensions;

public static class CommandExtensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddSingleton<CommandManager>();

        var commandTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToList();
        foreach (var type in commandTypes)
        {
            services.AddTransient(type);
        }

        return services;
    }
}