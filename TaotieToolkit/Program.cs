using TaotieToolkit.config;

namespace TaotieToolkit
{

    class Program
    {

        static void Main(string[] args)
        {
            System.Console.WriteLine("by: RabbitQ");
            System.Console.WriteLine("");
            if (args.Length < 1 || args[0] == "-h")
            {

                CommandManager.PrintHelp() ;
                return;
            }
            var commandKey = args[0]; // 假设命令键由模块名和子命令名组成
            var commandArgs = args.Length > 1 ? args : new string[0];
            CommandManager.ExecuteCommand(commandKey, commandArgs);
        }

     
     


    
   
    }
}

