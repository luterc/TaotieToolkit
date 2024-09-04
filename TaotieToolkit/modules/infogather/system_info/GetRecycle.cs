using System;
using System.Runtime.InteropServices;
using Shell32;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.infogather.system_info
{
    internal class GetRecycle : ICommand, ICommandMarker
    {
        public string Name => "GetRecycle";
        public string Description => "回收站";
        public void Execute(string[] args)
        {
            Console.WriteLine("\n[+] Get RecycleBin Filenames");
            Type shellAppType = Type.GetTypeFromProgID("Shell.Application");
            Object shell = Activator.CreateInstance(shellAppType);
            Folder recycleBin = GetShell32Folder(10, shell, shellAppType);

            foreach (FolderItem2 recfile in recycleBin.Items())
            {
                Console.WriteLine("\t" + recfile.Name);
            }

            Marshal.FinalReleaseComObject(shell);
        }
        public static Folder GetShell32Folder(object folder, Object shell, Type shellAppType)
        {
            return (Folder)shellAppType.InvokeMember("NameSpace",
            System.Reflection.BindingFlags.InvokeMethod, null, shell, new object[] { folder });
        }

    }
}
