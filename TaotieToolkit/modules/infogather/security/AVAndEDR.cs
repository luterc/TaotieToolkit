using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.infogather.security
{
    class AvProcessEDRproduct : ICommand, ICommandMarker
    {
        public string Name => "AvProcessEDRproduct";
        public string Description => "杀软或edr查询";
        public void Execute(string[] args)
        {
            //ANTIVURUS PROCESSES
            string[] avproducts = InfoConfig.getAVList();
            Process[] procs = Process.GetProcesses(Environment.MachineName);
            Console.WriteLine("\n[+] Checking for  Antivirus Processes on " + Environment.MachineName + "...");
            Console.WriteLine("[*] Loaded " + avproducts.Length + " AV Process Names");

            for (int i = 0; i < procs.Length; i++)
            {
                for (int a = 0; a < avproducts.Length; a++)
                {
                    string processSearch = avproducts[a].Substring(0, avproducts[a].Length - 4);
                    if (procs[i].ProcessName.Equals(processSearch))
                    {
                        Console.WriteLine("\t[!] Found AV Process: " + procs[i].ProcessName);
                    }
                }
            }

            //EDR PRODUCTS
            string[] edrproducts = InfoConfig.getEDRList();
            Console.WriteLine("\n[+] Enumerating EDR products on " + Environment.MachineName + "...");
            Console.WriteLine("[*] Loaded " + edrproducts.Length + " EDR Product Names");
            string edrPath = @"C:\Windows\System32\drivers\";
            for (int e = 0; e < edrproducts.Length; e++)
            {
                if (File.Exists(edrPath + edrproducts[e]))
                {
                    Console.WriteLine("\t[!] EDR driver found " + edrproducts[e]);
                }
            }
        }


    }

    class Defender : ICommand, ICommandMarker
    {
        public string Name => "Defender";
        public string Description => "Windows Defender排除项";
        public void Execute(string[] args)
        {
            //WINDOWS DEFENDER CONFIGURATION AND EXCEPTIONS  配置
            Console.WriteLine("\n[+] Enumerating Windows Defender Config...");
            RegistryKey folder_exclusions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows Defender\Exclusions\Paths");
            Console.WriteLine("\tEnumerating Windows Defender Path Exclusions...");
            if (folder_exclusions != null)
            {

                for (int i = 0; i < folder_exclusions.GetValueNames().Length; i++)
                {
                    Console.WriteLine("\t[+] " + folder_exclusions.GetValueNames()[i]);
                }
                Console.WriteLine();
            }
            //WINDOWS DEFENDER EXCLUSIONS  WINDOWS DEFENDER 排除项
            Console.WriteLine("\tEnumerating Windows Defender Extensions Exclusions...");
            RegistryKey ext_exclusions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows Defender\Exclusions\Extensions");

            if (ext_exclusions == null)
            {
                Console.WriteLine("\tNo extensions exclusions specified");
            }
            else
            {
                if (ext_exclusions.GetValueNames().Length > 0)
                {
                    for (int i = 0; i < ext_exclusions.GetValueNames().Length; i++)
                    {
                        Console.WriteLine("\t[+]" + ext_exclusions.GetValueNames()[i]);
                    }
                }
                else
                {
                    Console.WriteLine("\t[-] No extensions exclusions specified.");
                }

            }
        }
    }
}
