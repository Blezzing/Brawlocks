using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace CommonLibrary
{
    public static class HelperFunctions
    {
        public static string GetIP4Address()
        {
            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());

            foreach(IPAddress ip in ips)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return "127.0.0.1";
        }
    }
}
