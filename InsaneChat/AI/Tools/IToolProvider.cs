namespace InsaneChat.AI.Tools;

public interface IToolProvider
{
    Task<IReadOnlyList<ToolInfo>> GetToolInfosAsync();

    Task<string> ExecuteToolAsync(string name, BinaryData parameters);
}