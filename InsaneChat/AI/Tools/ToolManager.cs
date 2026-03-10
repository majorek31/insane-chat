using System.Reflection;
using InsaneChat.Helpers;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Chat;

namespace InsaneChat.AI.Tools;

public class ToolManager
{
    private readonly IServiceProvider _serviceProvier;
    private readonly Dictionary<string, ToolInfo> _tools = new();

    public IEnumerable<ToolInfo> Tools => _tools.Values;

    public IEnumerable<ChatTool> ChatTools => Tools.Select(t => ChatTool.CreateFunctionTool(t.Name, t.Description, t.ParameterSchema, true));

    public ToolManager(IServiceProvider provider)
    {
        _serviceProvier = provider;

        var toolTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(ITool).IsAssignableFrom(t));

        foreach (var toolType in toolTypes)
        {
            var attr = toolType.GetCustomAttribute<ToolAttribute>();
            if (attr == null)
            {
                continue;
            }
            var schema = attr.ParameterType is null
                ? SchemaHelper.EmptyToolParameters
                :
                SchemaHelper.CreateSchema(attr.ParameterType);

            var toolInfo = new ToolInfo(attr.Name, attr.Description, schema, toolType);

            _tools[toolInfo.Name] = toolInfo;
        }
    }


    public async Task<string> ExecuteToolAsync(string name, BinaryData parameters)
    {
        if (!_tools.TryGetValue(name, out var toolInfo))
        {
            return "Couldn't not find tool";
        }
        var tool = (ITool)_serviceProvier.GetRequiredService(toolInfo.ToolType);
        return await tool.ExecuteAsync(parameters);
    }
}