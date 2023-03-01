using Microsoft.Toolkit.Mvvm.Input;
using OrganigramEssentials;
using OrganigramService.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OrganigramService.ViewModel
{
    public class TcpIpViewModel : ValidateObservableObject
    {
        #region Fields
        private string _ipAddress = "127.0.0.1";
        private int _port = 8080;
        #endregion
        #region Properties
        public ITcpIpCommunication TcpIpCommunication { get; set; }
        public ILogger Logger { get; set; }
        public IAsyncRelayCommand OnLoadedCommand { get; }
        public IAsyncRelayCommand StartCommand { get; }
        public IAsyncRelayCommand StopCommand { get; }
        public IAsyncRelayCommand SendCommand { get; }
        #endregion

        #region Constructor
   
        /// <summary>
        /// Constructor of the TcpIpViewModel
        /// </summary>
        /// <param name="tcpIpCommunication"></param>
        /// <param name="log"></param>
        public TcpIpViewModel(ITcpIpCommunication tcpIpCommunication, ILogger log)
        {
            Logger = log ?? throw new ArgumentNullException(nameof(log));
            TcpIpCommunication = tcpIpCommunication ?? throw new ArgumentNullException(nameof(tcpIpCommunication));
            OnLoadedCommand = new AsyncRelayCommand(OnLoaded);
            StartCommand = new AsyncRelayCommand(StartConnection);
            StopCommand = new AsyncRelayCommand(StopConnection);
            SendCommand = new AsyncRelayCommand(Send);


            var ipAddress = IPAddress.Parse(IpAddress);

            TcpIpCommunication.StartConnection(IpAddress, Port);
        }
        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task OnLoaded()
        {
        }

        /// <summary>
        /// Starts the Connection of the Server
        /// </summary>
        /// <returns></returns>
        private async Task StartConnection()
        {
            var ipAddress = IPAddress.Parse(IpAddress);

            TcpIpCommunication.StartConnection(IpAddress,Port);


        }

        /// <summary>
        /// Sends data to the clients
        /// </summary>
        /// <returns></returns>
        private async Task Send()
        {
            TcpIpCommunication.SendData();
        }

        /// <summary>
        /// Stops the Connection
        /// </summary>
        /// <returns></returns>
        private async Task StopConnection()
        {
            TcpIpCommunication.CloseConnection();

        }

        /// <summary>
        /// IPAddress of the server
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
        /// Port of the server
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
