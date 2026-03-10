namespace InsaneChat.AI.Tools;

public interface ITool
{
    public Task<string> ExecuteAsync(BinaryData parameters);
}