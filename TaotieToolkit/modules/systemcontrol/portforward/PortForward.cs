using TaotieToolkit.config;
using System.Threading;
using System.Collections.Generic;
using System;
using ArgParseCS;

namespace TaotieToolkit.modules.systemcontrol.portforward
{
    public class PortForward : ICommand, ICommandMarker
    {
        public string Name => "PortForward";
        public string Description => "端口转发，example： -lp 8081 -rh 192.168.1.1.1 -rp 8081";

        //[ValueArgument(typeof(string), "lp", "localport", Description = "localport")]
        //private static string localport { get; set; }

        //[ValueArgument(typeof(string), "rh", "remotehost", Description = "remote tran host")]
        //private static string remotehost { get; set; }

        //[ValueArgument(typeof(string), "rp", "remoteport", Description = "remote port")]
        private static string remoteport { get; set; }

        private ArgParse argParse;
        private void DefineParams()
        {
            argParse = new ArgParse {
                new OptionSet("Analysis command") {
                    new Option("-lp", "--localport", "localport", true, true),
                    new Option("-rh", "--remotehost", "remotehost", true, true),
                    new Option("-rp", "--remoteport", "remoteport", true, true),
                },
                new OptionSet("Help command") {
                    new Option("-h", "--help", "Show help options", true, false),
                }
            };
        }
        public void Execute(string[] args)
        {
            DefineParams();
            if (args.Length == 0) {
                argParse.Usage();
                return;
            }
            try
            {
                argParse.Parse(args);
                OptionSet activeOptionSet = argParse.GetActiveOptionSet();
                Console.Out.WriteLine("Active optionset: " + activeOptionSet.Name);

                if (activeOptionSet.Name.Equals("Analysis command"))
                {
                    Option option = activeOptionSet.GetOption("-lp");
                    int localport = int.Parse(option.ParamValue);
                    option = activeOptionSet.GetOption("-rh");
                    string remotehost = option.ParamValue;
                    option = activeOptionSet.GetOption("-rp");
                    int remoteport = int.Parse(option.ParamValue);
                    Tran.start(localport, remotehost, remoteport);
                    while (true)
                    {
                        Thread.Sleep(999999999);
                    }
                }
                else {
                    argParse.Usage();
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