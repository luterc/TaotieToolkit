using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
using TaotieToolkit.config;

namespace TaotieToolkit.modules
{
    internal class All : ICommand, ICommandMarker
    {
        public string Name => "all";
        public string Description => "执行所有操作，second为时间，单位为秒";
        public void Execute(string[] args)
        {
            // 
            int second = int.Parse(args[0]) * 1000;

            foreach (var command in CommandManager.Commands.Values.OrderBy(c => c.Name))
            {
                if (command.Name == "Shutdown" || command.Name == "Logoff" || command.Name == "Hibernate" || command.Name == "Reboot" || command.Name == "PortForward" || command.Name == "all")
                {
                    continue;
                }
                List<string> arg=new List<string>();
                if (command.Name == "Mimikatz")
                {
                    arg.Add("-a");
                }
               

                DateTime currentTimeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
                DateTime currentTimeBeiJing = TimeZoneInfo.ConvertTimeFromUtc(currentTimeUtc, cstZone);
                Console.WriteLine("--------------------------------------------------------------------------------------------");
                Console.WriteLine("正在执行----------" + command.Name + "当前时间：" + currentTimeBeiJing);
                Console.WriteLine("--------------------------------------------------------------------------------------------");

                try {
                    CommandManager.ExecuteCommand(command.Name, arg.ToArray());

                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
               

                DateTime currentTimeUtc01 = DateTime.UtcNow;
                DateTime currentTimeBeiJing02 = TimeZoneInfo.ConvertTimeFromUtc(currentTimeUtc01, cstZone);
                Console.WriteLine("--------------------------------------------------------------------------------------------");
                Console.WriteLine("执行完成----------" + command.Name + "结束时间：" + currentTimeBeiJing02);
                Console.WriteLine("--------------------------------------------------------------------------------------------");
                
                Thread.Sleep(second);
                Console.WriteLine("");
                Console.WriteLine("");

            }
        }
    }
}
