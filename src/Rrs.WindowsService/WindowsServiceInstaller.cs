using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Rrs.WinowsService;

public class WindowsServiceInstaller
{
    public static void Install(WindowsServiceConfiguration config)
    {
        var path = Environment.ProcessPath;
        var command = $"""
            @echo OFF
            echo Checking if service is installed...
            sc.exe query "{config.Name}" > NUL
            IF ERRORLEVEL 1060 (
                echo Service is not installed
            ) else (
                echo Service is installed
                echo Stopping service...
                sc.exe stop "{config.Name}" > NUL
                sc.exe delete "{config.Name}"
            )
            
            echo Installing service...
            sc.exe create "{config.Name}" binpath= "{path}" start= auto displayname= "{config.DisplayName}"
            sc.exe description "{config.Name}" "{config.Description}"
            echo Starting service...
            sc.exe start "{config.Name}" > NUL
            """;

        RunCommand(command);
    }

    public static void Uninstall(WindowsServiceConfiguration config)
    {
        var command = $"""
            @echo OFF
            echo Checking if service is installed...
            sc.exe query "{config.Name}" > NUL
            IF ERRORLEVEL 1060 (
                echo Service is not installed
            ) else (
                echo Service is installed
                echo Stopping service...
                sc.exe stop "{config.Name}" > NUL
                sc.exe delete "{config.Name}"
            )
            """;

        RunCommand(command);
    }

    public static void RunCommand(string command)
    {
        const string batFile = "command.bat";
        File.WriteAllText(batFile, command);

        var process = new Process
        {
            StartInfo = new ProcessStartInfo("cmd.exe")
            {
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            }
        };
        process.OutputDataReceived += (s, e) =>
        {
            Console.WriteLine(e.Data);
        };
        process.Start();
        process.BeginOutputReadLine();

        using var sw = process.StandardInput;

        if (sw.BaseStream.CanWrite)
        {
            using var reader = new StringReader(command);
            sw.WriteLine(batFile);
        }

        sw.Close();

        process.WaitForExit();

        try
        {
            File.Delete(batFile);
        }
        catch(Exception e)
        {
            Console.Error.WriteLine($"Could not delete bat file '{batFile}'. Exception: {e.Message}");
        }
    }
}
