using System;
using System.IO;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.infogather.system_info
{
    internal class Drives : ICommand, ICommandMarker
    {
        public string Name => "Drives";
        public string Description => "磁盘情况";
        public void Execute(string[] args)
        {
            //ATTACHED DRIVES  磁盘情况
            Console.WriteLine("\n[+] Enumerating Drives...");
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo d in drives)
            {
                if (d.IsReady == true)
                {
                    Console.WriteLine("\tDrive " + d.Name + " " + d.DriveType + " - Size:" + d.TotalSize / 1024 / 1024 / 1024 + " G  -FreeSpace:" + d.AvailableFreeSpace / 1024 / 1024 / 1024 + "G");
                }
            }
        }

    }
}
