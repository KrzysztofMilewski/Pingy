using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Ping.Model
{
    public class PingStatus
    {
        private readonly System.Net.NetworkInformation.Ping _pingSender = new System.Net.NetworkInformation.Ping();

        public IPAddress IPAddress { get; set; }

        public PingReply Ping()
        {
            return _pingSender.Send(IPAddress);
        }

        public async Task<PingReply> PingServer()
        {
            var result = await _pingSender.SendPingAsync(IPAddress);
            return result;
        }
    }
}
