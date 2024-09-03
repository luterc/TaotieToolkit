using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TaotieToolkit.config;


namespace TaotieToolkit.modules.infogather.Browser
{
    internal class ChromiumHistoryCommand : ICommand, ICommandMarker
    {
        public string Name => "ChromiumHistory";
        public string Description => "抓取任何找到的Chrome/Edge/BBrave/Opera历史";

        public void Execute(string[] args)
        {
            string[] dirs = Utils.GetDirectories("\\Users\\");

            Console.WriteLine("[*] 开始抓取浏览器历史：");
            var URLs = new List<string>();
            foreach (var dir in dirs)
            {
                var parts = dir.Split('\\');
                var userName = parts[parts.Length - 1];
                if (dir.EndsWith("Public") || dir.EndsWith("Default") || dir.EndsWith("Default User") ||
                    dir.EndsWith("All Users"))
                {
                    continue;
                }

                string[] paths =
                {
                    "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\",
                    "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\",
                    "\\AppData\\Local\\BraveSoftware\\Brave-Browser\\User Data\\Default\\",
                    "\\AppData\\Roaming\\Opera Software\\Opera Stable\\"
                };

                foreach (string path in paths)
                {
                    var userChromiumHistoryPath = $"{dir}{path}History";

                    // parses a Chrome history file via regex
                    if (File.Exists(userChromiumHistoryPath))
                    {
                        var historyRegex =
                            new Regex(
                                @"(http|ftp|https|file)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?");


                        try
                        {
                            var fs = new FileStream(userChromiumHistoryPath, FileMode.Open, FileAccess.Read,
                                FileShare.ReadWrite);
                            var r = new StreamReader(fs);

                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                var m = historyRegex.Match(line);
                                if (m.Success)
                                {
                                    URLs.Add($"{m.Groups[0].ToString().Trim()}");
                                }
                            }
                        }
                        catch (IOException exception)
                        {
                            Console.WriteLine(
                                "\t[-] IO exception, history file likely in use (i.e. browser is likely running):" +
                                exception.Message);
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine("\t[-] Unknown exception" + exception);
                        }
                    }
                }
            }

            Console.WriteLine(URLs.Count > 0 ? string.Join(Environment.NewLine, URLs) : "\t[-] Cannot find history!");
        }
    }
}