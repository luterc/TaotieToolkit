using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TaotieToolkit.config;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace TaotieToolkit.modules.systemcontrol
{
    class BlockMouseAndKeyboard : ICommand, ICommandMarker
    {
        public string Name => "BlockMouseAndKeyboard";
        public string Description => "禁用键盘和鼠标，second为时间，单位为秒";
        public void Execute(string[] args)
        {
            if (!Utils.isAdmin())
            {
                Console.WriteLine("\t[-] Access denied, administrator rights needed to block system!");
                return;
            }

            if (args.Length != 1) {
                Console.WriteLine("\t[-] parameter error!");
                return;
            }
            int time;
            try
            {
                time = Int32.Parse(args[0]);
            }
            catch (Exception e) {
                Console.WriteLine("\t[-] parameter error!");
                return;
            }
            
            BlockInput(true);
            System.Threading.Thread.Sleep(time * 1000);
            BlockInput(false);
            Console.WriteLine("[*] Blocked system at " + time + " seconds!");
        }
        [DllImport("user32.dll", EntryPoint = "BlockInput")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BlockInput([MarshalAs(UnmanagedType.Bool)] bool fBlockIt);


       
    }
}
