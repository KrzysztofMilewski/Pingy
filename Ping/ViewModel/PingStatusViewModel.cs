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
        private string _addressTypedManually;
        private string _selectedAddressGroup;

        private List<string> CurrentServerList
        {
            get
            {
                return PingStatus.PredefinedAddresses.SingleOrDefault(ad => ad.Title == SelectedAddressGroup).Addresses.Keys.ToList();
            }
        }

        public PingStatusViewModel()
        {
            StartPinging = new RelayCommandAsync<object>(GetPingData,
                o =>
                {
                    if (ManualMode && string.IsNullOrWhiteSpace(AddressTypedManually))
                        return false;
                    else
                        return !ErrorOccurred;
                },
                new DisplayErrorMessage(HandleError));

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

            SwitchToManualMode = new RelayCommand(
                o =>
                {
                    ManualMode = true;
                    SelectedAddressGroup = null;
                    OnPropertyChanged(nameof(ManualMode), nameof(SelectedAddressGroup));
                },
                o =>
                {
                    return !ManualMode;
                });

            AcceptError = new RelayCommand(
                o =>
                {
                    ErrorOccurred = false;
                    OnPropertyChanged(nameof(ErrorOccurred));
                    Debug.WriteLine(ErrorOccurred);
                },
                o =>
                {
                    return ErrorOccurred;
                });

            Formatter = value => value.ToString(CultureInfo.InvariantCulture) + " ms";
            _selectedAddressGroup = AddressesGroups[0];
            ServerListForGroup = new ObservableCollection<string>();
            ChangeServerListForGroup();
        }

        public double AveragePing { get; private set; }
        public double MaximumPing { get; private set; }
        public int PacketsLost { get; private set; }
        public bool ManualMode { get; set; }
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

        public string AddressTypedManually
        {
            get { return _addressTypedManually; }
            set
            {
                _pingStatus.ServerToPing = value;
                _addressTypedManually = value;
                ResetStatistics();
            }
        }

        public bool IsPinging { get; set; }

        public ICommand StartPinging { get; private set; }
        public ICommand StopPinging { get; private set; }
        public ICommand SwitchToManualMode { get; private set; }
        public ICommand AcceptError { get; private set; }

        public List<string> AddressesGroups
        {
            get
            {
                return PingStatus.PredefinedAddresses.Select(ad => ad.Title).ToList();
            }
        }

        public string SelectedAddressGroup
        {
            get
            {
                return _selectedAddressGroup;
            }
            set
            {
                if (value == null)
                {
                    _selectedAddressGroup = null;
                    return;
                }

                if (value != _selectedAddressGroup)
                {
                    _selectedAddressGroup = value;
                    ManualMode = false;
                    OnPropertyChanged(nameof(ManualMode));
                    ChangeServerListForGroup();
                }
            }
        }

        public ObservableCollection<string> ServerListForGroup { get; }

        public string SelectedAddress
        {
            get
            {
                if (SelectedAddressGroup == null)
                    return null;

                var ipAddress = _pingStatus.ServerToPing;
                return PingStatus.PredefinedAddresses.SingleOrDefault(ad => ad.Title == SelectedAddressGroup).Addresses.SingleOrDefault(x => x.Value == ipAddress).Key;
            }
            set
            {
                if (value == null)
                    return;

                var ipAddress = PingStatus.PredefinedAddresses.SingleOrDefault(ad => ad.Title == SelectedAddressGroup).Addresses[value];
                _pingStatus.ServerToPing = ipAddress;
                ResetStatistics();
            }
        }

        public bool ErrorOccurred { get; set; }

        public ChartValues<long> PingPoints { get; private set; } = new ChartValues<long>();

        public Func<double, string> Formatter { get; private set; }

        private void ResetStatistics()
        {
            PingPoints.Clear();
            PacketsLost = 0;
            _totalNumberOfPingsCollected = 0;
            AveragePing = 0;
            OnPropertyChanged(nameof(SelectedAddress), nameof(AveragePing), nameof(PacketsLost));
        }

        private void ChangeServerListForGroup()
        {
            ServerListForGroup.Clear();

            var newAddresses = CurrentServerList;

            foreach (var newAddress in newAddresses)
                ServerListForGroup.Add(newAddress);

            SelectedAddress = ServerListForGroup[0];
        }

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

                    if (PacketsLost != 0)
                        OnPropertyChanged(nameof(PercentageOfLostPackets));
                }
            }
        }

        private void HandleError(Exception ex)
        {
            IsPinging = false;
            ErrorOccurred = true;
            OnPropertyChanged(nameof(IsPinging), nameof(ErrorOccurred));

            Debug.WriteLine(ErrorOccurred);
        }
    }
}
