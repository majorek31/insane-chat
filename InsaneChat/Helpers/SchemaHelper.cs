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

    public static BinaryData CreateSchema(Type type)
    {
        var options = new SystemTextJsonSchemaGeneratorSettings
        {
            GenerateEnumMappingDescription = true,
            FlattenInheritanceHierarchy = true,
            AlwaysAllowAdditionalObjectProperties = false,
        };

        var generator = new JsonSchemaGenerator(options);
        var schema = generator.Generate(type);
        return BinaryData.FromString(schema.ToJson());
    }
    public static BinaryData CreateSchema<T>()
    {
        return CreateSchema(typeof(T));
    }
}