using InsaneChat.AI.Tools;
using InsaneChat.Services;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;

namespace InsaneChat.AI;

public class ChatSession
{
    private readonly OpenAIService _openAIService;
    private readonly ToolManager _toolManager;
    private readonly List<ChatMessage> _messages;
    private readonly IConfiguration _configuration;
    private readonly ChatMessage _systemPrompt;

    public ChatSession(
        OpenAIService openAIService,
        IConfiguration configuration,
        ToolManager toolManager
    )
    {
        _toolManager = toolManager;
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

        for (int i = 0; i < 10; i++)
        {
            var response = await _openAIService.CompleteChatAsync(
                model,
                _messages,
                chatTools: _toolManager.ChatTools
            );
            var finishReason = response.FinishReason;
            switch (finishReason)
            {
                case ChatFinishReason.Stop:
                    {
                        return response.Content.First().Text;
                    }
                case ChatFinishReason.ToolCalls:
                    {
                        var toolCalls = response.ToolCalls;
                        foreach (var toolCall in toolCalls)
                        {
                            var callId = toolCall.Id;
                            var name = toolCall.FunctionName;
                            var parameters = toolCall.FunctionArguments;

                            var toolResponse = await _toolManager.ExecuteToolAsync(name, parameters);
                            _messages.Add(ChatMessage.CreateToolMessage(callId, toolResponse));
                        }
                        break;
                    }
            }
        }

        return "Model has failed after 10 iterations.";
    }
}