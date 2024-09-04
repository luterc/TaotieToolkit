using System;
using System.IO;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.infogather.system_info
{
    internal class LAPS : ICommand, ICommandMarker
    {
        public string Name => "LAPS";
        public string Description => "枚举LAPS";
        public void Execute(string[] args)
        {
            //LAPS
            Console.WriteLine("\n[+] Checking if LAPS is used...");
            string laps_path = @"C:\Program Files\LAPS\CSE\Admpwd.dll";
            Console.WriteLine(File.Exists(laps_path) ? "\t[!] LAPS is enabled" : "\t[-] LAPS is not enabled");

        }

    }
}
