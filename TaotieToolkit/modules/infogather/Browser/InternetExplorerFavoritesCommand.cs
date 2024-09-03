using System;
using System.Collections.Generic;
using System.IO;
using TaotieToolkit.config;


namespace TaotieToolkit.modules.infogather.Browser
{
    internal class InternetExplorerFavoritesCommand : ICommand, ICommandMarker
    {
        public string Name => "IEFavorites";
        public string Description => "Internet Explorer 收藏夹";

        public void Execute(string[] args)
        {
            var dirs = Utils.GetDirectories("\\Users\\");
            Console.WriteLine("[*] 开始抓取Internet Explorer 收藏夹：");
            var favorites = new List<string>();
            foreach (var dir in dirs)
            {
                if (dir.EndsWith("Public") || dir.EndsWith("Default") || dir.EndsWith("Default User") ||
                    dir.EndsWith("All Users"))
                {
                    continue;
                }

                var userFavoritesPath = $"{dir}\\Favorites\\";
                if (!Directory.Exists(userFavoritesPath))
                {
                    continue;
                }

                var bookmarkPaths = Directory.GetFiles(userFavoritesPath, "*.url", SearchOption.AllDirectories);
                if (bookmarkPaths.Length == 0)
                {
                    continue;
                }

             

                foreach (var bookmarkPath in bookmarkPaths)
                {
                    var rdr = new StreamReader(bookmarkPath);
                    string line;
                    var url = "";

                    while ((line = rdr.ReadLine()) != null)
                    {
                        if (!line.StartsWith("URL=", StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }

                        if (line.Length > 4)
                        {
                            url = line.Substring(4);
                        }

                        break;
                    }

                    favorites.Add(url.Trim());
                }
            }
            Console.WriteLine(favorites.Count > 0
                ? string.Join(Environment.NewLine, favorites)
                : "\t[-] Cannot find Internet Explorer favorites!");
        }
    }
}