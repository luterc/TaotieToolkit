using System;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using TaotieToolkit.config;

namespace TaotieToolkit
{
   class IsInVirtualMachine : ICommand, ICommandMarker
    {
        public string Name => "IsInVirtualMachine";
        public string Description => "是否在虚拟机中";
        public void Execute(string[] args)
        {
            using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
            {
                try
                {
                    using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
                    {
                        foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
                        {
                            if ((managementBaseObject["Manufacturer"].ToString().ToLower() == "microsoft corporation" && managementBaseObject["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL")) || managementBaseObject["Manufacturer"].ToString().ToLower().Contains("vmware") || managementBaseObject["Model"].ToString() == "VirtualBox")
                            {
                                Console.WriteLine("[*] In VirtualMachine");
                                return;
                            }
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("[*] In VirtualMachine");
                    return;
                }
            }
            foreach (ManagementBaseObject managementBaseObject2 in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController").Get())
            {
                if (managementBaseObject2.GetPropertyValue("Name").ToString().Contains("VMware") && managementBaseObject2.GetPropertyValue("Name").ToString().Contains("VBox"))
                {
                    Console.WriteLine("[*] In VirtualMachine");
                    return;
                }
            }
            Console.WriteLine("\t[-] Not in VirtualMachine");
        }


        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

    }
    class IsInSandboxie : ICommand, ICommandMarker
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        public string Name => "IsInSandboxie";
        public string Description => "是否在Sandboxie沙箱中";
        public void Execute(string[] args)
        {
            string[] array = new string[5]
            {
                "SbieDll.dll",
                "SxIn.dll",
                "Sf2.dll",
                "snxhk.dll",
                "cmdvrt32.dll"
            };
            for (int i = 0; i < array.Length; i++)
            {
                if (GetModuleHandle(array[i]).ToInt32() != 0)
                {
                    Console.WriteLine("[*] In Sandboxie");
                    return;
                }
            }
            Console.WriteLine("\t[-] Not in Sandboxie");
        }
    }


    class IsDebugger : ICommand, ICommandMarker
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        public string Name => "IsDebugger";
        public string Description => "是否为被调试状态";
        public void Execute(string[] args)
        {
            try
            {
                long ticks = DateTime.Now.Ticks;
                Thread.Sleep(10);
                if (DateTime.Now.Ticks - ticks < 10L)
                {
                    Console.WriteLine("[*] In Debugger");
                    return;

                }
            }
            catch { }
            Console.WriteLine("\t[-] Not in Debugger");
        }

    }

}
