using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.infogather
{
    internal class EnvironmentalVariables : ICommand, ICommandMarker
    {
        public string Name => "EnvironmentalVariables";
        public string Description => "环境变量";
        public void Execute(string[] args)
        {
            //ENVIRONMENTAL VARIABLES  系统信息
            WindowsPrincipal myId = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            var operating_system = Environment.OSVersion;
            Console.WriteLine("[+] Environmental Variables");
            Console.WriteLine("\tComputer Name: " + Environment.MachineName);
            Console.WriteLine("\tPlatform: " + operating_system.Platform + " - " + operating_system.VersionString);
            Console.WriteLine("\tRunning as User: " + Environment.UserName);
            Console.WriteLine("\tLocal Admin Privs: " + myId.IsInRole("BUILTIN\\" + "Administrators"));
            Console.WriteLine("\tOSVersion: {0}", Environment.OSVersion.ToString());
            Console.WriteLine("\tDomain: " + Environment.UserDomainName);
            //获取系统环境变量 用以判断是否安装Java,Python等编程环境
            Console.WriteLine("\n[+] System environment variable Path");
            string path = "Environment";
            RegistryKey masterKey = Registry.CurrentUser.OpenSubKey(path);
            string sPath = masterKey.GetValue("Path").ToString();
            masterKey.Close();
            //string sPath = Environment.GetEnvironmentVariable("Path");
            string[] sArray = Regex.Split(sPath, ";", RegexOptions.IgnoreCase);
            foreach (string i in sArray)
            {
                Console.WriteLine("\t" + i);
            }

        }
    }
}
