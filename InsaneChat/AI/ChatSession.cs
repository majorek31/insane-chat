using InsaneChat.Services;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;

namespace InsaneChat.AI;

public class ChatSession
{
    private readonly OpenAIService _openAIService;
    private readonly List<ChatMessage> _messages;
    private readonly IConfiguration _configuration;
    private readonly ChatMessage _systemPrompt;

    public ChatSession(OpenAIService openAIService, IConfiguration configuration)
    {
        _openAIService = openAIService;
        _configuration = configuration;
        _systemPrompt = ChatMessage.CreateSystemMessage(configuration["Chat:SystemPrompt"] ?? "You are a helpful AI Assistaint");
        _messages = new List<ChatMessage>
        {
            _systemPrompt
        };
    }

    public void ClearHistory()
    {
        _messages.Clear();
        if (_systemPrompt != null)
        {
            _messages.Add(_systemPrompt);
        }
    }
    public async Task<string> SendMessageAsync(string userInput)
    {
        _messages.Add(ChatMessage.CreateUserMessage(userInput));
        var model = _configuration["ChatModel"] ?? throw new Exception("Model not found please provide model in configuration with key 'ChatModel'");
        var response = await _openAIService.CompleteChatAsync(model, _messages);
        _messages.Add(ChatMessage.CreateAssistantMessage(response));
        return response.Content.First().Text;
    }
}