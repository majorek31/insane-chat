using System.Reflection;
using InsaneChat.AI;
using InsaneChat.AI.Tools;
using InsaneChat.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InsaneChat.Extensions;

public static class CoreAIExtensions
{
    public static IServiceCollection AddCoreAI(this IServiceCollection services)
    {

        // registe OpenAI
        services.AddSingleton<OpenAIService>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var apiKey = configuration["OpenAI:ApiKey"];
            var baseUrl = configuration["OpenAI:BaseUrl"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("OpenAI API key is not configured. Please set 'OpenAI:ApiKey' in your configuration.");
            }
            return new OpenAIService(
                apiKey,
                baseUrl
            );
        });

        // Add session related functionalities
        services.AddSingleton<ChatSession>();

        // register tools

        services.AddSingleton<ToolManager>();

        var toolTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(ITool).IsAssignableFrom(t))
            .ToList();

        foreach (var toolType in toolTypes)
        {
            services.AddTransient(toolType);
        }

        return services;
    }
}