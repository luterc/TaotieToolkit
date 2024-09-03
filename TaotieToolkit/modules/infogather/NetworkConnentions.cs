using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.infogather
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
                Console.WriteLine("\tLocal : " + info.LocalEndPoint.Address.ToString() + ":" + info.LocalEndPoint.Port.ToString() + " - Remote : " + info.RemoteEndPoint.Address.ToString() + ":" + info.RemoteEndPoint.Port.ToString());
            }
        }
    }
}
