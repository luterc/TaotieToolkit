using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.infogather
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
