using ArgParseCS;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.Persistence
{
    internal class Service : ICommand, ICommandMarker
    {
        public string Name => "Service";
        public string Description => "创建一个服务用以维持权限。";
        private const uint SC_MANAGER_CREATE_SERVICE = 0x00002;
        private const uint SERVICE_WIN32_OWN_PROCESS = 0x00000010;
        private const uint SERVICE_AUTO_START = 0x00000002;
        private const uint SERVICE_ERROR_NORMAL = 0x00000001;

        private ArgParse argParse;
        private void DefineParams()
        {
            argParse = new ArgParse {
                new OptionSet("Service") {
                    new Option("-n", "--serviceName", "Service name", true, true),
                    new Option("-p", "--binpath", "Executable file path", true, true),
                },
            };
        }
        public void Execute(string[] args)
        {
            DefineParams();
            if (args.Length == 0)
            {
                argParse.Usage();
                return;
            }
            try
            {
                argParse.Parse(args);
                OptionSet activeOptionSet = argParse.GetActiveOptionSet();

                Option option = activeOptionSet.GetOption("-n");
                string serviceName = option.ParamValue;
                option = activeOptionSet.GetOption("-p");
                string binpath = option.ParamValue;
                if (string.IsNullOrEmpty(serviceName) || string.IsNullOrEmpty(binpath)) {
                    argParse.Usage();
                    return;
                }
                IntPtr scmHandle = OpenSCManager(null, null, SC_MANAGER_CREATE_SERVICE);
                if (scmHandle == IntPtr.Zero)
                {
                    throw new Exception("[-] Failed to obtain a handle to the service control manager database - MAKE SURE YOU ARE ADMIN");
                }

                //Obtain a handle to the specified windows service
                Console.WriteLine("[+] Creating {0} service", serviceName);
                IntPtr serviceHandle = CreateService(scmHandle, serviceName, serviceName, SERVICE_ACCESS.SERVICE_ALL_ACCESS, SERVICE_WIN32_OWN_PROCESS, SERVICE_AUTO_START, SERVICE_ERROR_NORMAL, binpath, null, IntPtr.Zero, null, null, null);
                if (serviceHandle == IntPtr.Zero)
                {
                    throw new Exception($"[-] Failed to obtain a handle to service '{serviceName}'.");
                }

                Console.WriteLine("[+] Starting {0} service", serviceName);
                Thread.Sleep(1000);
                StartService(serviceHandle, 0, null);
                Console.WriteLine("[+] {0} has been enabled and started", serviceName);

                //Clean up
                if (scmHandle != IntPtr.Zero)
                    CloseServiceHandle(scmHandle);
                if (serviceHandle != IntPtr.Zero)
                    CloseServiceHandle(serviceHandle);
            }
            catch (Exception exception)
            {
                // User control the exception
                System.Console.WriteLine("exception: " + exception.Message);
            }
           
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr OpenSCManager(string machineName, string databaseName, uint dwAccess);

        [DllImport("Advapi32.dll")]
        public static extern IntPtr CreateService(
            IntPtr serviceControlManagerHandle,
            string lpSvcName,
            string lpDisplayName,
            SERVICE_ACCESS dwDesiredAccess,
            uint dwServiceType,
            uint dwStartType,
            uint dwErrorControl,
            string lpPathName,
            string lpLoadOrderGroup,
            IntPtr lpdwTagId,
            string lpDependencies,
            string lpServiceStartName,
            string lpPassword);

        [DllImport("advapi32.dll", EntryPoint = "CloseServiceHandle")]
        private static extern int CloseServiceHandle(IntPtr hSCObject);


        [Flags]
        public enum SERVICE_ACCESS : uint
        {
            STANDARD_RIGHTS_REQUIRED = 0xF0000,
            SERVICE_QUERY_CONFIG = 0x00001,
            SERVICE_CHANGE_CONFIG = 0x00002,
            SERVICE_QUERY_STATUS = 0x00004,
            SERVICE_ENUMERATE_DEPENDENTS = 0x00008,
            SERVICE_START = 0x00010,
            SERVICE_STOP = 0x00020,
            SERVICE_PAUSE_CONTINUE = 0x00040,
            SERVICE_INTERROGATE = 0x00080,
            SERVICE_USER_DEFINED_CONTROL = 0x00100,
            SERVICE_ALL_ACCESS =
                (STANDARD_RIGHTS_REQUIRED | SERVICE_QUERY_CONFIG | SERVICE_CHANGE_CONFIG | SERVICE_QUERY_STATUS | SERVICE_ENUMERATE_DEPENDENTS | SERVICE_START | SERVICE_STOP | SERVICE_PAUSE_CONTINUE
                 | SERVICE_INTERROGATE | SERVICE_USER_DEFINED_CONTROL)
        }

        [DllImport("advapi32.dll")]
        private static extern int StartService(IntPtr serviceHandle, int dwNumServiceArgs, string lpServiceArgVectors);
    }
}
