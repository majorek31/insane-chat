using System.Reflection;
using InsaneChat.Helpers;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Chat;

namespace InsaneChat.AI.Tools;

public class ToolManager
{
    private readonly IEnumerable<IToolProvider> _toolProviders;
    private readonly List<ToolInfo> _tools = new();

    public IEnumerable<ToolInfo> Tools => _tools;

    public IEnumerable<ChatTool> ChatTools => Tools.Select(t => ChatTool.CreateFunctionTool(t.Name, t.Description, t.ParameterSchema, true));

    public ToolManager(IEnumerable<IToolProvider> toolProviders)
    {
        _toolProviders = toolProviders;
    }

    public async Task LoadTools()
    {
        foreach (var provider in _toolProviders)
        {
            _tools.AddRange(await provider.GetToolInfosAsync());
        }
    }


    public async Task<string> ExecuteToolAsync(string name, BinaryData parameters)
    {
        foreach (var provider in _toolProviders)
        {
            var providerTools = await provider.GetToolInfosAsync();

            if (Tools.Any(t => t.Name == name))
            {
                return await provider.ExecuteToolAsync(name, parameters);
            }
        }
        throw new Exception($"Tool: {name} not found");
    }
}