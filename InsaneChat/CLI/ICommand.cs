namespace InsaneChat.CLI;

public interface ICommand
{
    public Task ExecuteAsync();
}