using Newtonsoft.Json;
using OrganigramClient.Model;
using OrganigramEssentials;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrganigramClient
{
    public class TcpIpClient : ITcpIpCommunication
    {
        #region Fields
        #endregion
        #region Properties
        public TcpClient Client { get; set; }
        public ILogger Logger { get; set; }
        public StreamReader StreamReader { get; set; }
        public StreamWriter StreamWriter { get; set; }
        public string DataString { get; set; }
        #endregion

        #region Constructor
        public TcpIpClient(ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region Methods
        /// <summary>
        /// Start the Connection to the Server
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        public void StartConnection(string ipAddress, int port)
        {
            try
            {
                var _ipAddr = IPAddress.Parse(ipAddress);

                Client = new TcpClient();
                Client.Connect(_ipAddr, port);
                var stream = Client.GetStream();
                StreamWriter = new StreamWriter(stream, Encoding.ASCII);
                StreamWriter.AutoFlush = true;
                StreamReader = new StreamReader(stream, Encoding.ASCII);

                Logger.Log($"Client is listening on {_ipAddr}:{port}");
            }
            catch (Exception)
            {

                Logger.Log("Could not connect to Server");
            }
           
        }

        /// <summary>
        /// Close the Connection to the Server
        /// </summary>
        public void CloseConnection()
        {
            Client.Close();
            StreamReader.Close();
            StreamWriter.Close();
        }



        /// <summary>
        /// Send Data to the Server
        /// </summary>
        public void SendData()
        {
            string message = "Hello, server!";
            StreamWriter.WriteLine(message);
            StreamWriter.Flush();

        }

        /// <summary>
        /// Reads Data from the Server
        /// </summary>
        /// <returns></returns>
        public string ReadData()
        {
             var data = StreamReader.ReadLine();

            return data;
        }


        /// <summary>
        /// Send Data to the Server
        /// </summary>
        /// <param name="message"></param>
        public void SendData(string message)
        {
            StreamWriter.WriteLine(message);
            StreamWriter.Flush();
        }

        #endregion

    }
}
