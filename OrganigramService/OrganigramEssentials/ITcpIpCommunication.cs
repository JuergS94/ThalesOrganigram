using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OrganigramEssentials
{
    public interface ITcpIpCommunication
    {
        /// <summary>
        /// StartConnection
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        void StartConnection(string ipAddress, int port);

        /// <summary>
        /// CloseConnection
        /// </summary>
        void CloseConnection();

        /// <summary>
        /// Read Data
        /// </summary>
        string ReadData();

        /// <summary>
        /// Send Data
        /// </summary>
        void SendData();

        /// <summary>
        /// Send Data message
        /// </summary>
        /// <param name="message"></param>
        void SendData(string message);


    }
}
