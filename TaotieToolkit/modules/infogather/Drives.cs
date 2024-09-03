using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.infogather
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
