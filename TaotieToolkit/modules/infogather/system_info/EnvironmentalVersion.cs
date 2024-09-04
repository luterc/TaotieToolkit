using System;
using System.IO;
using Microsoft.Win32;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.infogather.system_info
{
    class EnvironmentalVersion : ICommand, ICommandMarker
    {
        public string Name => "PowershellInfo";
        public string Description => "powershell版本信息";
        public void Execute(string[] args)
        {
            //CHECK FOR REGISTRY x64/x32   检查注册表
            var registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey key = registryKey.OpenSubKey("Software");
            if (key == null)
            {
                registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            }
            //POWERSHELL VERSIONS       powershell版本
            Console.WriteLine("\n[+] PowerShell Versions Installed");
            string[] directories = Directory.GetDirectories(@"C:\windows\System32\WindowsPowershell");
            for (int i = 0; i < directories.Length; i++)
            {
                Console.WriteLine("\t" + directories[i]);
            }
            //POWERSHELL HISTORY FILE    powershell 历史记录
            Console.WriteLine("\n[+] Checking for PowerShell History File...");
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string psHistoryPath = @"Microsoft\Windows\PowerShell\PSReadline\ConsoleHost_history.txt";
            string psHistory = Path.Combine(userPath, psHistoryPath);
            if (File.Exists(psHistory))
            {
                Console.WriteLine("\tHistory File in: " + psHistory);
            }
            else Console.WriteLine("\t[-] PowerShell History file does not exist");


            //POWERSHELL SCRIPT LOGGING ENUMERATION   powershell脚本日志记录枚举
            Console.WriteLine("\n[+] Enumerating PowerShell Environment Config...");
            RegistryKey scriptLog_config = registryKey.OpenSubKey(@"Software\Policies\Microsoft\Windows\Powershell\ScriptBlockLogging");
            if (scriptLog_config != null)
            {
                var scLog = scriptLog_config.GetValue("EnableScriptBlockLogging");
                if (scLog.ToString().Equals("1"))
                {

                    Console.WriteLine("\t[!] ScriptBlock Logging is enabled");
                }
                else Console.WriteLine("\t[-] ScriptBlock Logging is Not enabled");
            }
            //POWERSHELL TRANSCRIPTION LOGGING  powershell转录日志记录

            RegistryKey transcriptLog_config = registryKey.OpenSubKey(@"Software\Policies\Microsoft\Windows\PowerShell\Transcription");
            if (transcriptLog_config != null)
            {
                var tsLog = transcriptLog_config.GetValue("EnableTranscripting");
                if (tsLog.ToString().Equals("1"))
                {
                    Console.WriteLine("\t[!] Transcript Logging is enabled");
                }
                else Console.WriteLine("\t[-] Transcript Logging is Not enabled");
            }

            //POWERSHELL CONSTRAINED MODES ENUMERATION  powershell约束模式枚举
            //1. Full Language
            //2. Restricted Language
            //3. No Language
            //4. Constrained Language
            Console.WriteLine("\n[+] Enumerating PowerShell Constrained Config...");
            RegistryKey constrainLog_config = registryKey.OpenSubKey(@"System\CurrentControlSet\Control\Session Manager\Environment");
            if (constrainLog_config != null)
            {
                if (constrainLog_config.GetValue("_PSLockdownPolicy") != null)
                {
                    var psPolicy = constrainLog_config.GetValue("_PSLockdownPolicy");
                    if (psPolicy.Equals("1"))
                    {
                        Console.WriteLine("\tFull Language Mode");
                    }
                    else if (psPolicy.Equals("2"))
                    {
                        Console.WriteLine("\tFull Language Mode");
                    }
                    else if (psPolicy.Equals("3"))
                    {
                        Console.WriteLine("\tNo Language Mode");
                    }
                    else if (psPolicy.Equals("4"))
                    {
                        Console.WriteLine("[!] Constrained Language Mode");
                    }

                }
                else Console.WriteLine("\t[-] PSLockdownPolicy in not enabled");
            }
        }

    }

    class CsharpVersion : ICommand, ICommandMarker
    {
        public string Name => "CsharpVersion";
        public string Description => "c#版本";
        public void Execute(string[] args)
        {
            Console.WriteLine("\n[+] Microsoft.NET Versions Installed");
            string[] Netdirectories = Directory.GetDirectories(@"C:\Windows\Microsoft.NET\Framework");
            for (int i = 0; i < Netdirectories.Length; i++)
            {
                Console.WriteLine("\t" + Netdirectories[i]);
            }
        }
    }
}
