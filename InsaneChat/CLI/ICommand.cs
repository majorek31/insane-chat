namespace InsaneChat.CLI;

public interface ICommand
{
    public string Name { get; }
    public string Description { get; }
    public Task ExecuteAsync();
}