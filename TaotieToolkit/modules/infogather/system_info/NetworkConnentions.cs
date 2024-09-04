using System;
using System.Net;
using System.Net.NetworkInformation;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.infogather.system_info
{
    internal class NetworkConnentions : ICommand, ICommandMarker
    {
        public string Name => "NetworkConnentions";
        public string Description => "网络连接";
        public void Execute(string[] args) {

            //NETWORK CONNECTIONS  网络连接
            Console.WriteLine("\n[+] Enumerating Network Connections...");
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] endPoints = ipProperties.GetActiveTcpListeners();
            TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections();
            foreach (TcpConnectionInformation info in tcpConnections)
            {
                Console.WriteLine("\tLocal : " + info.LocalEndPoint.Address + ":" + info.LocalEndPoint.Port + " - Remote : " + info.RemoteEndPoint.Address + ":" + info.RemoteEndPoint.Port);
            }
        }
    }
}
