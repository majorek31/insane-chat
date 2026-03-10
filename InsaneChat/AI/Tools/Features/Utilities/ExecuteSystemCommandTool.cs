using System.Diagnostics;

namespace InsaneChat.AI.Tools.Features.Utilities;

public record CommandDto(string cmd);

[Tool("execute_system_command", "Executes the command in the system terminal", typeof(CommandDto))]
public class ExecuteSystemCommandTool : ITool
{
    public async Task<string> ExecuteAsync(BinaryData parameters)
    {
        var dto = parameters.ToObjectFromJson<CommandDto>();
        Console.WriteLine($"Executing: {dto.cmd}");
        var isWindows = Environment.OSVersion.Platform == PlatformID.Win32NT;
        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = isWindows ? "cmd" : "bash",
            Arguments = isWindows ? $"/c {dto.cmd}" : $"-c {dto.cmd}",
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
            UseShellExecute = false,
        };
        using var process = Process.Start(processStartInfo);
        string output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();
        return output;
    }
}