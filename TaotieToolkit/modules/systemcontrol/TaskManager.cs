using System;
using Microsoft.Win32;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.systemcontrol
{
    internal class DisableTaskManager : ICommand, ICommandMarker
    {
        public string Name => "DisableTaskManager";
        public string Description => "禁用任务管理器";
        public void Execute(string[] args)
        {
            if (!Utils.isAdmin())
            {
                Console.WriteLine("\t[-] Access denied, administrator rights needed to disable taskmanager!");
                return;
            }
            RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
            objRegistryKey.SetValue("DisableTaskMgr", 1);
            objRegistryKey.Close();
            Console.WriteLine("[*] TaskManager disabled!");
        }
  
          }
    class EnableTaskManager : ICommand, ICommandMarker
    {
        public string Name => "EnableTaskManager";
        public string Description => "启用任务管理器";
        public void Execute(string[] args)
        {
            if (!Utils.isAdmin())
            {
                Console.WriteLine("\t[-] Access denied, administrator rights needed to enable taskmanager!");
                return;
            }
            RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
            objRegistryKey.SetValue("DisableTaskMgr", 0);
            objRegistryKey.Close();
            Console.WriteLine("[*] TaskManager disabledenabed");

        }
    }
}
