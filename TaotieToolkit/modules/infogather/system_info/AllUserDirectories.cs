﻿using System;
using System.IO;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.infogather.system_info
{
    internal class AllUserDirectories : ICommand, ICommandMarker
    {
        public string Name => "AllUserDirectories";
        public string Description => "用户目录";
        public void Execute(string[] args)
        {
            //ALL USER FOLDERS ACCESS
            Console.WriteLine("\n[+] All user directories");
            string[] dirs = Directory.GetDirectories(@"c:\users");
            foreach (string dir in dirs)
            {
                try
                {
                    System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(dir);
                    Console.WriteLine("\t[*] " + dir + " Folder is accessible by current user");
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("\t[-] " + dir + " Folder is NOT accessible by current user");
                }
            }
        }
    }
}
