using System.ClientModel;
using OpenAI;
using OpenAI.Chat;

namespace InsaneChat.Services;

public class OpenAIService
{
    private readonly OpenAIClient _client;

    public OpenAIService(string apiKey, string? baseUrl = null)
    {
        _client = new OpenAIClient(new ApiKeyCredential(apiKey), new OpenAIClientOptions
        {
            Endpoint = new Uri(baseUrl ?? "https://api.openai.com")
        });
    }

    public async Task<ChatCompletion> CompleteChatAsync(string model, IEnumerable<ChatMessage> messages, int maxTokens = 1024, float temperature = 0.7f)
    {
        var chat = _client.GetChatClient(model);
        var response = await chat.CompleteChatAsync(
            messages: messages,
            options: new ChatCompletionOptions
            {
                MaxOutputTokenCount = maxTokens,
                Temperature = temperature
            }
        );
        return response.Value;
    }

    public async Task<float[]> GetEmbeddingAsync(string model, string input)
    {
        var embeddingClient = _client.GetEmbeddingClient(model);
        var response = await embeddingClient.GenerateEmbeddingAsync(input);
        return response.Value.ToFloats().ToArray();
    }
}