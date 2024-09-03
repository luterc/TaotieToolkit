using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TaotieToolkit
{
    class Utils
    {
        public static void PowerCommand(string args)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "shutdown.exe";
            startInfo.Arguments = args;
            process.StartInfo = startInfo;
            process.Start();
        }
        public static bool isAdmin()
        {
            bool result;
            using (WindowsIdentity current = WindowsIdentity.GetCurrent())
            {
                result = new WindowsPrincipal(current).IsInRole(WindowsBuiltInRole.Administrator);
            }
            return result;
        }


        // Powershell
        public static void RunPS(string args)
        {
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = args,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                }
            };
            proc.Start();
        }

        // Edit registry
        public static void RegistryEdit(string regPath, string name, string value)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regPath, RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    if (key == null)
                    {
                        Registry.LocalMachine.CreateSubKey(regPath).SetValue(name, value, RegistryValueKind.DWord);
                        return;
                    }
                    if (key.GetValue(name) != (object)value)
                        key.SetValue(name, value, RegistryValueKind.DWord);
                }
            }
            catch { }
        }

        public static bool Is64Bit
        {
            get { return IntPtr.Size == 8; }
        }


        public static string[] GetDirectories(string relPath)
        {
            relPath = relPath.Trim('\\');
            return System.IO.Directory.GetDirectories($"{Environment.GetEnvironmentVariable("SystemDrive")}\\{relPath}\\");
        }
    }
}
