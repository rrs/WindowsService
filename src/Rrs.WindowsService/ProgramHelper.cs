using System;

namespace Rrs.WindowsService;

public static class ProgramHelper
{
    /// <summary>
    /// Will install or uninstall depending on command line args. 
    /// </summary>
    /// <param name="args">command line args</param>
    /// <param name="config">config for the service</param>
    /// <returns>true if the service has been 'managed' </returns>
    public static bool ManageService(string[] args, WindowsServiceConfiguration config)
    {
        try
        {
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "-i" or "-install":
                        WindowsServiceInstaller.Install(config);
                        return true;
                    case "-u" or "-uninstall":
                        WindowsServiceInstaller.Uninstall(config);
                        return true;
                    default: return false;
                }
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            return true;
        }
    }
}
