using Microsoft.Toolkit.Mvvm.Input;
using OrganigramClient.Resources;
using OrganigramEssentials;
using OrganigramService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrganigramClient.ViewModel
{
    public class TcpIpViewModel : ValidateObservableObject
    {
        #region Fields
        private string _ipAddress = "127.0.0.1";
        private int _port = 8080;
        private string _tcpIpText;
        #endregion
        #region Properties
        public IAsyncRelayCommand OnLoadedCommand { get; }
        public IAsyncRelayCommand StartCommand { get; }
        public IAsyncRelayCommand StopCommand { get; }
        public IAsyncRelayCommand SendCommand { get; }
        public IAsyncRelayCommand ReadCommand { get; }
        public ITcpIpCommunication TcpIpCommunication { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor of the TcpIpViewModel
        /// </summary>
        /// <param name="tcpIpCommunication"></param>
        public TcpIpViewModel(ITcpIpCommunication tcpIpCommunication)
        {
            TcpIpCommunication = tcpIpCommunication ?? throw new ArgumentNullException(nameof(tcpIpCommunication));
            OnLoadedCommand = new AsyncRelayCommand(OnLoaded);
            StartCommand = new AsyncRelayCommand(StartConnection);
            StopCommand = new AsyncRelayCommand(StopConnection);
            SendCommand = new AsyncRelayCommand(Send);
            ReadCommand = new AsyncRelayCommand(Read);


            var ipAddress = IPAddress.Parse(IpAddress);
            TcpIpCommunication.StartConnection(IpAddress, Port);
        }
        #endregion

        #region Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        private async Task StartConnection()
        {
            var ipAddress = IPAddress.Parse(IpAddress);

            TcpIpCommunication.StartConnection(IpAddress, Port);


        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        private async Task StopConnection()
        {
            TcpIpCommunication.CloseConnection();
            TcpIpText = $"Client is cloesed";

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task OnLoaded()
        {

        }

        /// <summary>
        ///  Binding to the Read Button
        /// </summary>
        /// <returns></returns>
        private async Task Read()
        {
            TcpIpText = TcpIpCommunication.ReadData();
        }

        /// <summary>
        /// Binding to the Send button
        /// </summary>
        /// <returns></returns>
        private async Task Send()
        {
            TcpIpCommunication.SendData();
        }

        public string TcpIpText
        {
            get { return _tcpIpText; }
            set
            {
                _tcpIpText = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// IP Address of the Server
        /// </summary>
        public string IpAddress
        {
            get
            {
                return _ipAddress;
            }
            set
            {
                _ipAddress = value;
                this.OnPropertyChanged();
            }

        }

        /// <summary>
        /// Port of the Server
        /// </summary>
        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
                this.OnPropertyChanged();
            }

        }
        #endregion

    }
}
