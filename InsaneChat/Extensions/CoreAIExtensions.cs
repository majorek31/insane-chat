using InsaneChat.AI;
using InsaneChat.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InsaneChat.Extensions;

public static class CoreAIExtensions
{
    public static IServiceCollection AddCoreAI(this IServiceCollection services)
    {
        services.AddSingleton<OpenAIService>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var apiKey = configuration["OpenAI:ApiKey"];
            var baseUrl = configuration["OpenAI:BaseUrl"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("OpenAI API key is not configured. Please set 'OpenAI:ApiKey' in your configuration.");
            }
            return new OpenAIService(apiKey, baseUrl);
        });

        services.AddSingleton<ChatSession>();

        return services;
    }
}