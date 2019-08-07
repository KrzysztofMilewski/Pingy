using LiveCharts;
using LiveCharts.Wpf;
using Ping.Model;
using Ping.Utilities;
using System.ComponentModel;
using System.Net;
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

        public PingStatusViewModel()
        {
            Ping = new RelayCommandAsync<object>(ActionOnClick);
        }

        private readonly PingStatus _pingStatus = new PingStatus();

        public string IPAdress
        {
            get
            {
                if (_pingStatus.IPAddress == null)
                    return "";
                else
                    return _pingStatus.IPAddress.ToString();
            }
            set
            {
                if (value != IPAdress)
                {
                    _pingStatus.IPAddress = IPAddress.Parse(value);
                    OnPropertyChanged(nameof(IPAdress));
                }
            }
        }

        public string RoundtripTime { get; private set; } = "";

        public ICommand Ping { get; private set; }

        private async Task ActionOnClick(object parameter)
        {
            for (int i = 0; i < 10; i++)
            {
                var result = await _pingStatus.PingServer();
                RoundtripTime += result.RoundtripTime.ToString();
                OnPropertyChanged(nameof(RoundtripTime));
            }
        }
        
    }
}
