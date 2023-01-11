using System;

namespace Rrs.WinowsService;

public static class ProgramHelper
{
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
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            return true;
        }
    }
}
