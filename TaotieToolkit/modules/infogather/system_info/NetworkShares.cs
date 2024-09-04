using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using ConsoleTables;
using TaotieToolkit.config;
using TaotieToolkit.utils;

namespace TaotieToolkit.modules.infogather.system_info
{
    public class NetworkShares: ICommand, ICommandMarker
    {
        public string Name => "NetworkShares";
        public string Description => "网络连接";
        public void Execute(string[] args) {
            var typeDict = new Dictionary<uint, string>()
            {
                { 0, "Disk Drive" },
                { 1, "Print Queue" },
                { 2, "Device " },
                { 3, "IPC" },
                { 2147483648, "Disk Drive Admin" },
                { 2147483649, "Print Queue Admin" },
                { 2147483650, "Device Admin" },
                { 2147483651, "IPC Admin" },
            };
            //NETWORK CONNECTIONS  网络连接
            Console.WriteLine("\n[+] Enumerating Network Connections...");
            string computerName = Environment.MachineName; // 获取计算机名称
            string query = "SELECT * FROM Win32_Share"; // 查询所有共享资源
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                "\\\\" + computerName + "\\root\\CIMV2", query);
            var table = new ConsoleTable("Name", "Path", "Description","type");
            foreach (ManagementObject share in searcher.Get())
            {

                table.AddRow((string)share["Name"] , (string)share["Path"], (string)share["Description"],typeDict[(uint)share["type"]]);
            }
            table.Write(Format.Minimal);
            Console.WriteLine();
            
        }
    }
}
