namespace InsaneChat.AI.Tools.Features.Utilities;

[Tool("get_user_name", "Returns the user name of currently logged-in system user")]
public class GetUserNameTool : ITool
{
    public Task<string> ExecuteAsync(BinaryData parameters)
    {
        return Task.FromResult("User name is: " + Environment.UserName);
    }
}