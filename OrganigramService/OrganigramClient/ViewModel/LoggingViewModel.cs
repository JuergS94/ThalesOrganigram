using OrganigramClient.Resources;
using OrganigramEssentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganigramClient.ViewModel
{
    public class LoggingViewModel : ValidateObservableObject, ILogger
    {

        private string _tcpIpText;

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
        /// <inheritdoc/>
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            DateTime timestamp = DateTime.Now;
            TcpIpText = timestamp.ToString() + ": " + message;
        }
    }
}
