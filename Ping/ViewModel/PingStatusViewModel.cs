using LiveCharts;
using Ping.Model;
using Ping.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ping.ViewModel
{
    public class PingStatusViewModel : INotifyPropertyChanged
    {
        #region INPC Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(params string[] parameterNames)
        {
            if (PropertyChanged != null)
            {
                foreach (var parameter in parameterNames)
                    PropertyChanged(this, new PropertyChangedEventArgs(parameter));
            }
        }
        #endregion

        private readonly PingStatus _pingStatus = new PingStatus();
        private int _totalNumberOfPingsCollected;

        public PingStatusViewModel()
        {
            StartPinging = new RelayCommandAsync<object>(GetPingData);

            StopPinging = new RelayCommand(
                o =>
                {
                    IsPinging = false;
                    OnPropertyChanged(nameof(IsPinging));
                },
                o =>
                {
                    return IsPinging;
                });

            Formatter = value => value.ToString(CultureInfo.InvariantCulture) + " ms";
        }

        public double AveragePing { get; private set; }
        public double MaximumPing { get; private set; }
        public int PacketsLost { get; private set; }
        public double PercentageOfLostPackets
        {
            get
            {
                if (_totalNumberOfPingsCollected == 0)
                    return 0;
                else
                    return (100.0 * PacketsLost) / (double)_totalNumberOfPingsCollected;
            }
        }

        public ICommand StartPinging { get; private set; }
        public ICommand StopPinging { get; private set; }

        public bool IsPinging { get; set; }

        public List<string> LolServers
        {
            get { return PingStatus.PredefinedAddresses.Keys.ToList(); }
        }

        public string SelectedAddress
        {
            get
            {
                var currentAddress = _pingStatus.IPAddress.ToString();
                var addressName = PingStatus.PredefinedAddresses.SingleOrDefault(x => x.Value == currentAddress).Key;
                return addressName;
            }
            set
            {
                var address = PingStatus.PredefinedAddresses[value];
                _pingStatus.IPAddress = IPAddress.Parse(address);
                PingPoints.Clear();
                _totalNumberOfPingsCollected = 0;
                PacketsLost = 0;
                OnPropertyChanged(nameof(SelectedAddress));
            }
        }

        public ChartValues<long> PingPoints { get; private set; } = new ChartValues<long>();

        public Func<double, string> Formatter { get; private set; }

        private async Task GetPingData(object parameter)
        {
            IsPinging = true;
            OnPropertyChanged(nameof(IsPinging));
            while (IsPinging)
            {
                var result = await _pingStatus.PingServer();
                _totalNumberOfPingsCollected++;

                if (result.Status != IPStatus.Success)
                {
                    PacketsLost++;
                    OnPropertyChanged(nameof(PacketsLost), nameof(PercentageOfLostPackets));
                }
                else
                {
                    PingPoints.Add(result.RoundtripTime);

                    if (PingPoints.Count > 20)
                        PingPoints.RemoveAt(0);

                    AveragePing = PingPoints.Average();
                    OnPropertyChanged(nameof(AveragePing));
                }
            }
        }
    }
}
