﻿using System;
using System.IO;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.infogather.system_info
{
    internal class RecentFiles : ICommand, ICommandMarker
    {
        public string Name => "RecentFiles";
        public string Description => "最近的文件";
        public void Execute(string[] args) {
            //WINDOWS RECENT FILES   Windows最近使用的文件
            string recents = @"Microsoft\Windows\Recent";
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string recentsPath = Path.Combine(userPath, recents);
            DirectoryInfo di = new DirectoryInfo(recentsPath);
            Console.WriteLine("\n[+] Recent Items in " + recentsPath);
            foreach (var file in di.GetFiles())
            {
                Console.WriteLine("\t" + file.Name);
            }
        }
    }
}
