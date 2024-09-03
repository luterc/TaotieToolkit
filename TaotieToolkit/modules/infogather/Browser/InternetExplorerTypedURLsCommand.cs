using System;
using System.Collections.Generic;
using Microsoft.Win32;
using TaotieToolkit.config;
using Utils;

namespace TaotieToolkit.modules.infogather.Browser
{
    class TypedUrl
    {
        public TypedUrl(DateTime time, string url)
        {
            Time = time;
            Url = url;
        }
        public DateTime Time { get; }
        public string Url { get; }
    }

    internal class InternetExplorerTypedUrlsCommand : ICommand, ICommandMarker
    {
        public string Name => "IEUrls";
        public string Description => "Internet Explorer键入的URL（最近7天，参数==最近X天）";

        public void Execute(string[] args)
        {
            // lists Internet explorer history (last 7 days by default)
            var lastDays = 7;
            if (args.Length >= 1)
            {
                if (!int.TryParse(args[0], out lastDays))
                {
                    throw new ArgumentException("[-] Argument is not an integer");
                }
            }

            var startTime = DateTime.Now.AddDays(-lastDays);

            Console.WriteLine($"[*] Internet Explorer typed URLs for the last {lastDays} days\n");
            var URLs = new List<TypedUrl>();

            var SIDs = RegistryUtil.GetUserSIDs();

            foreach (var sid in SIDs)
            {
                if (!sid.StartsWith("S-1-5") || sid.EndsWith("_Classes"))
                {
                    continue;
                }
                var settings = RegistryUtil.GetValues(RegistryHive.Users, $"{sid}\\SOFTWARE\\Microsoft\\Internet Explorer\\TypedURLs");
                if ((settings == null) || (settings.Count <= 1))
                {
                    continue;
                }

              
                foreach (var kvp in settings)
                {
                    var timeBytes = RegistryUtil.GetBinaryValue(RegistryHive.Users, $"{sid}\\SOFTWARE\\Microsoft\\Internet Explorer\\TypedURLsTime", kvp.Key.Trim());

                    if (timeBytes == null)
                        continue;

                    var timeLong = BitConverter.ToInt64(timeBytes, 0);
                    var urlTime = DateTime.FromFileTime(timeLong);
                    if (urlTime > startTime)
                    {
                        URLs.Add(new TypedUrl(
                            urlTime,
                            kvp.Value.ToString().Trim()
                            ));
                    }
                }
            }
            Console.WriteLine(URLs.Count > 0
                ? string.Join(Environment.NewLine, URLs)
                : "\t[-] Cannot find Internet Explorer键入的URL!");
        }

       
    }
}
