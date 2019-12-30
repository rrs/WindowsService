using System;
using System.ServiceProcess;

namespace Rrs.WinowsService
{
    public static class ProgramHelper
    {
        public static int Run(ServiceBase service, string[] args, string serviceDisplayName = null, string serviceDescription = null)
        {
            try
            {
                if (args.Length > 0)
                {
                    if (args[0] == "--service")
                    {
                        ServiceBase.Run(new ServiceBase[]
                        {
                            service
                        });
                    }
                    else
                    {
                        ConsoleHelper.Run(service.ServiceName, args, serviceDisplayName, serviceDescription);
                    }
                }
                else
                {
                    Console.WriteLine("install with\t-i");
                    Console.WriteLine("uninstall with\t-u");
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return -1;
            }
        }
    }
}
