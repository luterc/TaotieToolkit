using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TaotieToolkit.config
{
    internal class CommandManager
    {
        public static Dictionary<string, ICommand> Commands { get; private set; } = new Dictionary<string, ICommand>();

        static CommandManager()
        {
            // 自动注册所有实现了ICommandMarker接口的命令
            var commandTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(ICommandMarker).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
  
            foreach (var type in commandTypes)
            {
                var command = (ICommand)Activator.CreateInstance(type);
                Commands.Add(command.Name, command);
            }
        }

        public static void ExecuteCommand(string command, string[] args)
        {
            if (Commands.TryGetValue(command, out ICommand cmd))
            {
                cmd.Execute(args);
            }
            else
            {
                Console.WriteLine($"\t[-] 未知命令: {command}");
                PrintHelp();
            }
        }

        public static void PrintHelp()
        {
            // 获取最长的命令名称长度，以便于对齐
            int maxLength = Commands.Max(c => c.Key.Length);

            Console.WriteLine("Usage:");
            foreach (var command in Commands.Values.OrderBy(c => c.Name))
            {
                // 打印格式化的命令名称和描述
                Console.WriteLine($"       TaotieToolkit {command.Name.PadRight(maxLength)}    {command.Description}");
            }
        }
    }
}