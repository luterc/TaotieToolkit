using System;
using System.Collections.Generic;
using Microsoft.Win32;
using TaotieToolkit.config;
using Utils;

namespace TaotieToolkit.modules.infogather.system_info
{
    internal class AutoRunsCommand : ICommand, ICommandMarker
    {
        public string Name => "AutoRuns";
        public string Description => "自启动查询";

        public void Execute(string[] args)
        {
        
            Console.WriteLine("\n[+] 自启动的可执行文件：");
            string[] autorunLocations = new string[]
            {
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce",
                "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run",
                "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnce",
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunService",
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnceService",
                "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunService",
                "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnceService"
            };

            foreach (string autorunLocation in autorunLocations)
            {
                var settings = RegistryUtil.GetValues(RegistryHive.LocalMachine, autorunLocation);

                if ((settings != null) && (settings.Count != 0))
                {
                    string key = $"HKLM:\\{autorunLocation}";
                    List<string> listEntries = new List<string>();

                    foreach (KeyValuePair<string, object> kvp in settings)
                    {
                        listEntries.Add(kvp.Value.ToString());
                    }
                    if (listEntries.Count>0)
                    {
                        Console.WriteLine("[*] "+key+":\n"+string.Join(Environment.NewLine, listEntries));
                    }
                   
                }
            }
        }

    }
}
