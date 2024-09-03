using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.infogather
{
    internal class ApplockerEnumerating : ICommand, ICommandMarker
    {
        public string Name => "ApplockerEnumerating";
        public string Description => "枚举AppLocker";
        public void Execute(string[] args) {
            //CHECK FOR REGISTRY x64/x32   检查注册表
            var registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey key = registryKey.OpenSubKey("Software");
            if (key == null)
            {
                registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            }
            //APPLOCKER ENUMERATION  applocker枚举
            Console.WriteLine("\n[+] Enumerating Applocker Config...");
            RegistryKey appLocker_config = registryKey.OpenSubKey(@"Software\Policies\Microsoft\Windows\SrpV2\Exe");
            if (appLocker_config != null)
            {
                for (int i = 0; i < appLocker_config.SubKeyCount; i++)
                {
                    Console.WriteLine(appLocker_config.OpenSubKey(appLocker_config.GetSubKeyNames()[i]).GetValue("Value"));
                }
            }
        }
    }
}
