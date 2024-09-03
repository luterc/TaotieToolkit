using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using TaotieToolkit.config;

namespace TaotieToolkit
{
    class ProcessList : ICommand, ICommandMarker
    {
        public string Name => "GetProcessList";
        public string Description => "枚举所有进程信息";

        public void Execute(string[] args)
        {
            Console.WriteLine("\n[+] Gets a list of running processes on the system.");

            Process[] processes = Process.GetProcesses();

            Console.WriteLine("PID\tProcess Name\tParent PID\tParent Process Name\tPath\tOwner");

            // 使用并行循环遍历进程列表
            Parallel.ForEach(processes, p =>
            {
                try
                {
                    string processName = p.ProcessName;
                    int pid = p.Id;
                    int parentPid = GetParentProcessId(p);
                    string parentProcessName = GetParentProcessName(parentPid);
                    string path = p.MainModule.FileName;
                    string owner = GetProcessOwner(p);

                    Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", pid, processName, parentPid, parentProcessName, path, owner);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\t[-] Can not get process info,exception:" + ex.Message);
                }
            });
        }

        static ConcurrentDictionary<int, string> parentProcessNames = new ConcurrentDictionary<int, string>();
        static int GetParentProcessId(Process p)
        {
            int result = 0;

            try
            {
                ManagementObject mo = new ManagementObject("win32_process.handle='" + p.Id + "'");
                mo.Get();
                result = Convert.ToInt32(mo["ParentProcessId"]);
            }
            catch { }

            return result;
        }

        static string GetParentProcessName(int parentPid)
        {
            if (parentPid == 0) return "";

            // 使用ConcurrentDictionary缓存父进程名称，避免重复查询
            if (parentProcessNames.TryGetValue(parentPid, out string name))
            {
                return name;
            }

            try
            {
                Process parentProcess = Process.GetProcessById(parentPid);
                name = parentProcess.ProcessName;
            }
            catch { }

            parentProcessNames.TryAdd(parentPid, name);

            return name;
        }

        static ConcurrentDictionary<int, string> processOwners = new ConcurrentDictionary<int, string>();
        static string GetProcessOwner(Process p)
        {
            string result = "";
            int pid = p.Id;

            // 使用ConcurrentDictionary缓存进程所有者，避免重复查询
            if (processOwners.TryGetValue(pid, out string owner))
            {
                return owner;
            }

            try
            {
                string query = "SELECT * FROM Win32_Process WHERE ProcessID = " + pid;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                ManagementObjectCollection processList = searcher.Get();

                foreach (ManagementObject obj in processList)
                {
                    string[] argList = { string.Empty, string.Empty };
                    int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                    if (returnVal == 0)
                    {
                        owner = argList[0];
                    }
                }
            }
            catch { }

            processOwners.TryAdd(pid, owner);

            return owner;
        }

    }
}
