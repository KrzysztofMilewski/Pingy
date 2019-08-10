using LiveCharts;
using Ping.Model;
using Ping.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
            _selectedAddressGroup = AddressesGroups[0];
            ServerListForGroup = new ObservableCollection<string>();
            ChangeServerListForGroup();
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

        public List<string> AddressesGroups
        {
            get
            {
                return PingStatus.PredefinedAddresses.Select(ad => ad.Title).ToList();
            }
        }

        private string _selectedAddressGroup;
        public string SelectedAddressGroup
        {
            get
            {
                return _selectedAddressGroup;
            }
            set
            {
                if (value != _selectedAddressGroup)
                {
                    _selectedAddressGroup = value;
                    ChangeServerListForGroup();
                }
            }
        }

        private void ChangeServerListForGroup()
        {
            ServerListForGroup.Clear();

            var newAddresses = CurrentServerList;

            foreach (var newAddress in newAddresses)
                ServerListForGroup.Add(newAddress);

            SelectedAddress = ServerListForGroup[0];
        }

        private List<string> CurrentServerList
        {
            get
            {
                return PingStatus.PredefinedAddresses.SingleOrDefault(ad => ad.Title == SelectedAddressGroup).Addresses.Keys.ToList();
            }
        }

        public ObservableCollection<string> ServerListForGroup { get; }

        public string SelectedAddress
        {
            get
            {
                var ipAddress = _pingStatus.IPAddress.ToString();
                return PingStatus.PredefinedAddresses.SingleOrDefault(ad => ad.Title == SelectedAddressGroup).Addresses.SingleOrDefault(x => x.Value == ipAddress).Key;
            }
            set
            {
                Debug.WriteLine(value);

                if (value == null)
                    return;

                var ipAddress = PingStatus.PredefinedAddresses.SingleOrDefault(ad => ad.Title == SelectedAddressGroup).Addresses[value];
                _pingStatus.IPAddress = IPAddress.Parse(ipAddress);
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
