using System;
using System.Management;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.infogather.system_info
{
    class GetSystemInfo : ICommand, ICommandMarker
    {
        public string Name => "GetSystemInfo";
        public string Description => "获取系统信息";
        public void Execute(string[] args)
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");

            foreach (ManagementObject os in searcher.Get())
            {
                // 打印操作系统名称、版本、制造商等信息
                Console.WriteLine("Operating System Information:");
                Console.WriteLine("Name: {0}", os["Caption"]);
                Console.WriteLine("Version: {0}", os["Version"]);
                Console.WriteLine("BuildNumber: {0}", os["BuildNumber"]);
                Console.WriteLine("Manufacturer: {0}", os["Manufacturer"]);
                Console.WriteLine("Architecture: {0}", os["OSArchitecture"]);
                Console.WriteLine("Language: {0}", os["MUILanguages"]);
                Console.WriteLine();
            }
        }

    }
}
