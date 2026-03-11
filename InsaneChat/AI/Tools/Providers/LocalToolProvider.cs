using System.Reflection;
using InsaneChat.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace InsaneChat.AI.Tools.Providers;

public class LocalToolProvider : IToolProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, Type> _tools;
    private readonly List<ToolInfo> _toolInfos = new();

    public LocalToolProvider(IServiceProvider provider)
    {
        _serviceProvider = provider;

        var toolTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract && typeof(ITool).IsAssignableFrom(x))
            .ToList();

        foreach (var type in toolTypes)
        {
            var attr = type.GetCustomAttribute<ToolAttribute>();
            if (attr is null)
            {
                continue;
            }

            _toolInfos.Add(new ToolInfo(
                attr.Name,
                attr.Description,
                attr.ParameterType is not null ? SchemaHelper.CreateSchema(attr.ParameterType) : SchemaHelper.EmptyToolParameters,
                type
            ));
        }

        _tools = _toolInfos.ToDictionary(x => x.Name, x => x.ToolType);
    }

    public Task<IReadOnlyList<ToolInfo>> GetToolInfosAsync() => Task.FromResult<IReadOnlyList<ToolInfo>>(_toolInfos);

    public async Task<string> ExecuteToolAsync(string name, BinaryData parameters)
    {
        if (!_tools.TryGetValue(name, out var toolType))
        {
            throw new Exception($"Tool {name} was not found!");
        }
        var tool = (ITool)_serviceProvider.GetRequiredService(toolType);
        return await tool.ExecuteAsync(parameters);
    }
}