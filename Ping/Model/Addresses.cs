using System.Collections.Generic;

namespace Ping.Model
{
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
                    {"AWS China (Beijing)", "dynamodb.cn-north-1.amazonaws.com.cn"},
                    {"AWS Ireland Region", "dynamodb.eu-west-1.amazonaws.com" },
                    {"AWS London Region", "dynamodb.eu-west-2.amazonaws.com" },
                    {"Europe", "185.82.209.9" },
                    {"Japan", "dynamodb.ap-northeast-1.amazonaws.com" },
                    {"Korea", "dynamodb.ap-northeast-2.amazonaws.com" },
                    {"North America (Ohio)", "dynamodb.us-east-2.amazonaws.com" },
                    {"Oceania (Sydney)", "dynamodb.ap-southeast-2.amazonaws.com" },
                    {"South Africa (Sao Paulo)", "dynamodb.sa-east-1.amazonaws.com" },
                    {"South-East Asia (Singapore)", "dynamodb.ap-southeast-1.amazonaws.com" }
                }
            },
            new AddressesDictionary()
            {
                Title = "Counter-Strike: Global Offensive",
                Addresses = new Dictionary<string, string>()
                {
                    {"Amsterdam", "155.133.248.34" },
                    {"Atlanta", "162.254.199.170" },
                    {"Tokyo", "34.85.47.228" },
                    {"Sao Paulo", "205.185.194.34" },
                    {"Los Angeles", "162.254.195.70" },
                    {"London", "162.254.196.82" },
                    {"Madrid", "155.133.246.50" },
                    {"Paris", "185.25.182.66" },
                    {"Stockholm", "162.254.198.40" },
                    {"Sydney", "103.10.125.146" },
                    {"Warsaw", "155.133.230.35" }
                }
            }
        };
    }
}
