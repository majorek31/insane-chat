namespace InsaneChat.AI.Tools;

public class ToolAttribute : Attribute
{
    public string Name { get; }
    public string Description { get; }
    public Type? ParameterType { get; set; }

    public ToolAttribute(string name, string description, Type? parameterType = null)
    {
        Name = name;
        Description = description;
        ParameterType = parameterType;
    }
}