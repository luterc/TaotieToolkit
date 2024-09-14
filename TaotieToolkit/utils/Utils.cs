using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TaotieToolkit
{
    class Utils
    {

        public static bool PowerCommand(string command)
        {
            Process process = new Process();
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c " + command;

            return process.Start();
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
        public static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        public static void AlwaysNotify()
        {
            RegistryKey alwaysNotify = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
            string consentPrompt = alwaysNotify.GetValue("ConsentPromptBehaviorAdmin").ToString();
            string secureDesktopPrompt = alwaysNotify.GetValue("PromptOnSecureDesktop").ToString();
            alwaysNotify.Close();

            if (consentPrompt == "2" & secureDesktopPrompt == "1")
            {
                Console.WriteLine("UAC is set to 'Always Notify.' This attack will fail. Exiting...");
                Environment.Exit(1);
            }
        }

    }
}
