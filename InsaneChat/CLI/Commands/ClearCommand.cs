using InsaneChat.AI;

namespace InsaneChat.CLI.Commands;

[Command("clear", "Clears the chat history")]
public class ClearCommand : ICommand
{
    private readonly ChatSession _chatSession;

    public ClearCommand(ChatSession chatSession)
    {
        _chatSession = chatSession;
    }

    public Task ExecuteAsync()
    {
        _chatSession.ClearHistory();
        Console.WriteLine("Chat history cleared.");
        return Task.CompletedTask;
    }
}