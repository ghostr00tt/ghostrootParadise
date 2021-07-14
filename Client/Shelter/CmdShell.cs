using System;
using System.Text;
using System.Diagnostics;
namespace Client.Shelter
{


public static class CmdShell
{
    public static string LaunchCmd(string ShellCommand)
    {
        try
        {
            return Shell.ShellCmdExecute(ShellCommand);
        }
        catch (Exception e) { return e.GetType().FullName + ": " + e.Message + Environment.NewLine + e.StackTrace; }
    }
}


public class Shell
{
 

   
    public static string ShellExecuteWithPath(string ShellCommand, string Path, string Username = "", string Domain = "", string Password = "")
    {
        if (ShellCommand == null || ShellCommand == "") return "";

        string ShellCommandName = ShellCommand.Split(' ')[0];
        string ShellCommandArguments = "";
        if (ShellCommand.Contains(" "))
        {
            ShellCommandArguments = ShellCommand.Replace(ShellCommandName + " ", "");
        }

        Process shellProcess = new Process();
        if (Username != "")
        {
            shellProcess.StartInfo.UserName = Username;
            shellProcess.StartInfo.Domain = Domain;
            System.Security.SecureString SecurePassword = new System.Security.SecureString();
            foreach (char c in Password)
            {
                SecurePassword.AppendChar(c);
            }
            shellProcess.StartInfo.Password = SecurePassword;
        }
        shellProcess.StartInfo.FileName = ShellCommandName;
        shellProcess.StartInfo.Arguments = ShellCommandArguments;
        shellProcess.StartInfo.WorkingDirectory = Path;
        shellProcess.StartInfo.UseShellExecute = false;
        shellProcess.StartInfo.CreateNoWindow = true;
        shellProcess.StartInfo.RedirectStandardOutput = true;
        shellProcess.StartInfo.RedirectStandardError = true;

        var output = new StringBuilder();
        shellProcess.OutputDataReceived += (sender, args) => { output.AppendLine(args.Data); };
        shellProcess.ErrorDataReceived += (sender, args) => { output.AppendLine(args.Data); };

        shellProcess.Start();

        shellProcess.BeginOutputReadLine();
        shellProcess.BeginErrorReadLine();
        shellProcess.WaitForExit();

        return output.ToString().TrimEnd();
    }


        public static string ShellCmdExecute(string ShellCommand, string Username = "", string Domain = "", string Password = "")
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                return ShellExecute("bash -c " + ShellCommand, Username, Domain, Password);
            }
            else if (Environment.OSVersion.Platform.ToString().Contains("Win32"))
            {
                return ShellExecute("cmd.exe /c " + ShellCommand, Username, Domain, Password);
            }
            return ShellExecute("cmd.exe /c " + ShellCommand, Username, Domain, Password);
        }


        public static string ShellExecute(string ShellCommand, string Username = "", string Domain = "", string Password = "")
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                return ShellExecuteWithPath(ShellCommand, "/bin/", Username, Domain, Password);
            }
            else if (Environment.OSVersion.Platform.ToString().Contains("Win32"))
            {
                return ShellExecuteWithPath(ShellCommand, "C:\\Windows\\System32\\", Username, Domain, Password);
            }
            return ShellExecuteWithPath(ShellCommand, "~", Username, Domain, Password);
        }
    }
}