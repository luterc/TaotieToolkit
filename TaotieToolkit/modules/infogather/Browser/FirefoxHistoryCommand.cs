using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TaotieToolkit.config;


namespace TaotieToolkit.modules.infogather.Browser
{
    internal class FirefoxHistoryCommand : ICommand, ICommandMarker
    {
        public string Name => "FirefoxHistory";
        public string Description => "抓取任何找到的FireFox历史";

        public void Execute(string[] args)
        {
            var dirs = Utils.GetDirectories("\\Users\\");
            Console.WriteLine("[*] 开始抓取Firefox浏览器历史：");
            var history = new List<string>();
            foreach (var dir in dirs)
            {
                var parts = dir.Split('\\');
                var userName = parts[parts.Length - 1];

                if (dir.EndsWith("Public") || dir.EndsWith("Default") || dir.EndsWith("Default User") ||
                    dir.EndsWith("All Users"))
                {
                    continue;
                }

                var userFirefoxBasePath = $"{dir}\\AppData\\Roaming\\Mozilla\\Firefox\\Profiles\\";

                // parses a Firefox history file via regex
                if (Directory.Exists(userFirefoxBasePath))
                {
                    
                    var directories = Directory.GetDirectories(userFirefoxBasePath);

                    foreach (var directory in directories)
                    {
                        var firefoxHistoryFile = $"{directory}\\places.sqlite";
                        var historyRegex = new Regex(@"(http|ftp|https|file)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?");

                        try
                        {
                            using (var r = new StreamReader(firefoxHistoryFile))
                            {
                                string line;
                                while ((line = r.ReadLine()) != null)
                                {
                                    var m = historyRegex.Match(line);
                                    if (m.Success)
                                    {
                                        // WriteHost("      " + m.Groups[0].ToString().Trim());
                                        history.Add(m.Groups[0].ToString().Trim());
                                    }
                                }
                            }
                        }
                        catch (IOException exception)
                        {
                            Console.WriteLine(
                                "\t[-] IO exception, places.sqlite file in use (i.e. Firefox is likely running):" +
                                exception.Message);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("\t[-] Unknown exception" + e);
                        }
                    }
                }
            }
            
            Console.WriteLine(history.Count > 0 ?string.Join(Environment.NewLine, history) : "\t[-] Cannot find history!");
        }
    }
}
