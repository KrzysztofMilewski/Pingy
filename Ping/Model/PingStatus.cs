using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Ping.Model
{

    public class PingStatus
    {
        public static readonly Dictionary<string, string> PredefinedAddresses = new Dictionary<string, string>()
        {
            {"North America", "104.160.131.3" },
            {"Europe West","104.160.141.3" },
            {"Europe North-East", "104.160.142.3" },
            {"Oceania", "104.160.156.1" },
            {"Latin America", "104.160.136.3" }
        };

        private readonly System.Net.NetworkInformation.Ping _pingSender = new System.Net.NetworkInformation.Ping();

        public PingStatus()
        {
            IPAddress = IPAddress.Parse(PredefinedAddresses["Oceania"]);
        }

        public IPAddress IPAddress { get; set; }

        public async Task<PingReply> PingServer()
        {
            var result = await _pingSender.SendPingAsync(IPAddress);
            return result;
        }
    }
}
