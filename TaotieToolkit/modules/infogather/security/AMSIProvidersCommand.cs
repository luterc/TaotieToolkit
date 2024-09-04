using System;
using Microsoft.Win32;
using TaotieToolkit.config;
using Utils;

namespace TaotieToolkit.modules.infogather.security
{
    internal class AMSIProviderCommand : ICommand, ICommandMarker
    {
        public string Name => "AMSIProviders";
        public string Description => "查询已注册AMSI的提供商";

        public void Execute(string[] args)
        {
            Console.WriteLine("\n[+] 已注册AMSI的提供商：");
            var providers =
                RegistryUtil.GetSubkeyNames(RegistryHive.LocalMachine, @"SOFTWARE\Microsoft\AMSI\Providers") ??
                new string[] { };
            foreach (var provider in providers)
            {
                var providerPath = RegistryUtil.GetStringValue(RegistryHive.LocalMachine,
                    $"SOFTWARE\\Classes\\CLSID\\{provider}\\InprocServer32", "");
                Console.WriteLine("[*] Providers registered for AMSI:"+ providerPath);
               
            }
        }
    }
}