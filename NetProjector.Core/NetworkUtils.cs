using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetProjector.Core
{
    static class NetworkUtils
    {
        public static Task<string> GetPublicIPAsync()
        {
            using (WebClient wc = new WebClient())
            {
                return wc.DownloadStringTaskAsync(new Uri("http://icanhazip.com/"));
            }
        }

        public static IEnumerable<IPAddress> GetIPAddressesFromInterfaces()
        {
            var ipList = new List<IPAddress>();
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (var ua in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ua.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        Console.WriteLine("found ip " + ua.Address.ToString());
                        ipList.Add(ua.Address);
                    }
                }
            }

            return ipList;
        }

        public static IEnumerable<IPAddress> GetIPAddressFromDns()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }
    }
}
