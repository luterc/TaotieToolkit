using System;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.systemcontrol
{

        // Command
       

        class Shutdown : ICommand, ICommandMarker
        {
            public string Name => "Shutdown";
            public string Description => "关机";
            public void Execute(string[] args)
            {
                Utils.PowerCommand("/s /t 0");
                Console.WriteLine("[*] Shutting down..");
            }


        }

        class Logoff : ICommand, ICommandMarker
        {
            public string Name => "Logoff";
            public string Description => "注销登录";
            public void Execute(string[] args)
            {
                Utils.PowerCommand("/l");
                Console.WriteLine("[*] In Sandboxie");
            }


        }
        class Hibernate : ICommand, ICommandMarker
        {
            public string Name => "Hibernate";
            public string Description => "休眠";
            public void Execute(string[] args)
            {

                Utils.PowerCommand("/h");
                Console.WriteLine("[*] Hibernate..");
            }


        }
        class Reboot : ICommand, ICommandMarker
        {
            public string Name => "Reboot";
            public string Description => "重启";
            public void Execute(string[] args)
            {
                Utils.PowerCommand("/r /t 0");
                Console.WriteLine("[*] Rebooting computer..");
            }


        }

      
}
