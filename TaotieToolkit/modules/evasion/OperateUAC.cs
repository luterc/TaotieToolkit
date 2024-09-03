using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.evasion
{
    class DisableUAC : ICommand, ICommandMarker
    {
        public string Name => "DisableUAC";
        public string Description => "禁用UAC";

        // Disable UAC
        public void Execute(string[] args)
        {
            if (!Utils.isAdmin())
            {
                Console.WriteLine("\t[-]Access denied, administrator rights needed to change UAC mode!");
                return;
            }

            Utils.RegistryEdit(@"Software\Microsoft\Windows\CurrentVersion\Policies\System", "EnableLUA", "0");
            Console.WriteLine("[*] UAC disabled, please reboot!");
        }

    }
    class EnableUAC : ICommand, ICommandMarker
    {
        public string Name => "EnableUAC";
        public string Description => "启用UAC";

        public void Execute(string[] args)
        {
            if (!Utils.isAdmin())
            {
                Console.WriteLine("\t[-] Access denied, administrator rights needed to change UAC mode!");
                return;
            }

            Utils.RegistryEdit(@"Software\Microsoft\Windows\CurrentVersion\Policies\System", "EnableLUA", "1");
            Console.WriteLine("[*] UAC enabled, please reboot!");
        }

    }




}
