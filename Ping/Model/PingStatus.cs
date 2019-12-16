using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ping.Model
{
    public class PingStatus
    {
        public static List<AddressesDictionary> PredefinedAddresses { get { return Addresses.PredefinedAddresses; } }

        private readonly System.Net.NetworkInformation.Ping _pingSender = new System.Net.NetworkInformation.Ping();

        private bool _pingingEndpoint = false;

        private string _serverToPing;
        public string ServerToPing
        {
            get { return _serverToPing; }
            set
            {
                _serverToPing = value;
                var ipRegex = new Regex(@"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$");
                if (ipRegex.IsMatch(value))
                    _pingingEndpoint = false;
                else
                    _pingingEndpoint = true;
            }
        }

        public async Task<PingReply> PingServer()
        {
            if (_pingingEndpoint)
                return await _pingSender.SendPingAsync(_serverToPing);
            else
                return await _pingSender.SendPingAsync(IPAddress.Parse(_serverToPing));
        }
    }
}
