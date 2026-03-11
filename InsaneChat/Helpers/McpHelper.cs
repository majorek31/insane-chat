using ModelContextProtocol.Protocol;

namespace InsaneChat.Helpers;

public static class McpHelper
{
    public static string McpResultToString(CallToolResult result)
    {
        if (result?.Content == null || result.Content.Count == 0)
        {
            return string.Empty;
        }
        var texts = new List<string>();

        foreach (var block in result.Content)
        {
            switch (block)
            {
                case TextContentBlock t:
                    {
                        texts.Add(t.Text);
                        break;
                    }
                case ToolResultContentBlock tr:
                    {
                        if (tr.Content != null)
                        {
                            foreach (var inner in tr.Content)
                            {
                                if (inner is TextContentBlock innteText)
                                {
                                    texts.Add(innteText.Text);
                                }
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
        }
        return string.Join("\n", texts);
    }
}