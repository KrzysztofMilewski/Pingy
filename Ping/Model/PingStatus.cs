using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Ping.Model
{
    public class AddressesDictionary
    {
        public string Title { get; set; }
        public Dictionary<string, string> Addresses { get; set; }
    }

    public static class Addresses
    {
        public static List<AddressesDictionary> PredefinedAddresses { get; } = new List<AddressesDictionary>()
        {
            new AddressesDictionary()
            {
                Title = "League of Legends",
                Addresses = new Dictionary<string, string>()
                {
                    {"North America", "104.160.131.3" },
                    {"Europe West","104.160.141.3" },
                    {"Europe North-East", "104.160.142.3" },
                    {"Oceania", "104.160.156.1" },
                    {"Latin America", "104.160.136.3" }
                }
            },
            new AddressesDictionary()
            {
                Title = "Playerunknown's Battlegrounds",
                Addresses = new Dictionary<string, string>()
                {
                    //{"AWS China (Beijing)", "dynamodb.cn-north-1.amazonaws.com.cn"},
                    //{"AWS Ireland Region", "dynamodb.eu-west-1.amazonaws.com" },
                    //{"AWS London Region", "dynamodb.eu-west-2.amazonaws.com" },
                    //{"Europe", "185.82.209.9" },
                    //{"Japan", "dynamodb.ap-northeast-1.amazonaws.com" },
                    //{"Korea", "dynamodb.ap-northeast-2.amazonaws.com" },
                    //{"North America (Ohio)", "dynamodb.us-east-2.amazonaws.com" },
                    //{"Oceania (Sydney)", "dynamodb.ap-southeast-2.amazonaws.com" },
                    //{"South Africa (Sao Paulo)", "dynamodb.sa-east-1.amazonaws.com" },
                    //{"South-East Asia (Singapore)", "dynamodb.ap-southeast-1.amazonaws.com" }

                    {"ASDF", "104.160.131.3" },
                    {"QWRQ","104.160.141.3" },
                    {"ASASWF", "104.160.142.3" },
                    {"AS#WF", "104.160.156.1" },
                    {"Latin America", "104.160.136.3" }
                }
            }
        };
    }

    public class PingStatus
    {
        public static List<AddressesDictionary> PredefinedAddresses { get { return Addresses.PredefinedAddresses; } }

        private readonly System.Net.NetworkInformation.Ping _pingSender = new System.Net.NetworkInformation.Ping();

        public PingStatus()
        {
            IPAddress = IPAddress.Parse(PredefinedAddresses[0].Addresses["Oceania"]);
        }

        public IPAddress IPAddress { get; set; }

        public async Task<PingReply> PingServer()
        {
            var result = await _pingSender.SendPingAsync(IPAddress);
            return result;
        }
    }
}
