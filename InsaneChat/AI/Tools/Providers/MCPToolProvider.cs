using InsaneChat.Helpers;
using ModelContextProtocol;
using ModelContextProtocol.Client;

namespace InsaneChat.AI.Tools.Providers;

public class MCPToolProvider : IToolProvider
{
    private readonly McpClient _client;

    public MCPToolProvider(McpClient client)
    {
        _client = client;
    }

    public async Task<IReadOnlyList<ToolInfo>> GetToolInfosAsync()
    {
        var tools = await _client.ListToolsAsync();

        return tools.Select(t => new ToolInfo(
            t.Name,
            t.Description,
            BinaryData.FromObjectAsJson(t.JsonSchema)
        )).ToList();
    }

    public async Task<string> ExecuteToolAsync(string name, BinaryData parameters)
    {
        var args = parameters.ToObjectFromJson<Dictionary<string, object?>>();
        var result = await _client.CallToolAsync(name, args);
        var textResult = McpHelper.McpResultToString(result);
        Console.WriteLine(textResult);
        return textResult;
    }
}