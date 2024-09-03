using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.infogather.Browser
{
    // TODO: Listing bookmarks does not account for bookmark folders. It lists folders, but not nested bookmarks/folders inside another folder )
    internal class Bookmark
    {
        public Bookmark(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public string Name { get; }
        public string Url { get; }

        public override string ToString()
        {
            return $"{Name}--------------{Url}";
        }
    }

    class ChromiumBookmarksCommand : ICommand, ICommandMarker
    {
        public string Name => "ChromiumBookmarks";
        public string Description => "抓取任何找到的Chrome/Edge/BBrave/Opera书签";

        public void Execute(string[] args)
        {
            string[] dirs = Utils.GetDirectories("\\Users\\");
            //var dirs = ThisRunTime.GetDirectories("\\Users\\");

            Console.WriteLine("[*] 开始抓取浏览器书签：");
            var bookmarks = new List<Bookmark>();
            foreach (string dir in dirs)
            {
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
                    // TODO: Account for other profiles
                    var userChromeBookmarkPath = $"{dir}{path}Bookmarks";

                    // parses a Chrome bookmarks file
                    if (!File.Exists(userChromeBookmarkPath))
                        continue;


                    try
                    {
                        var contents = File.ReadAllText(userChromeBookmarkPath);
                        JsonData json_data = JsonMapper.ToObject(contents);

                        var roots = json_data["roots"];
                        var bookmarkBar = roots["bookmark_bar"];
                        JsonData children = bookmarkBar["children"];
                        foreach (JsonData item in children)
                        {
                            var bookmark = new Bookmark(
                                $"{item["name"].ToString().Trim()}",
                                item.ContainsKey("url") ? $"{item["url"]}" : "(Bookmark Folder?)"
                            );

                            bookmarks.Add(bookmark);
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.ToString());
                    }
                }
            }

            Console.WriteLine(bookmarks.Count > 0
                ? string.Join(Environment.NewLine, bookmarks)
                : "\t[-] Cannot find bookmark!");
        }
    }
}