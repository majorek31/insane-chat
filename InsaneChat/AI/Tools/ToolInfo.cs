namespace InsaneChat.AI.Tools;

public record ToolInfo(
    string Name,
    string Description,
    BinaryData ParameterSchema,
    float[]? Embedding = null
);