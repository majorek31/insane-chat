using NJsonSchema.Generation;

namespace InsaneChat.Helpers;

public static class SchemaHelper
{
    public static BinaryData EmptyToolParameters => BinaryData.FromString("""
    {
        "type": "object",
        "properties": {}
    }
    """);

    public static BinaryData CreateSchema<T>()
    {
        var options = new SystemTextJsonSchemaGeneratorSettings
        {
            GenerateEnumMappingDescription = true,
            FlattenInheritanceHierarchy = true,
            AlwaysAllowAdditionalObjectProperties = false,
        };

        var generator = new JsonSchemaGenerator(options);
        var schema = generator.Generate(typeof(T));
        return BinaryData.FromString(schema.ToJson());
    }
}