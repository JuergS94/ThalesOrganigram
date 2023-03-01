using Newtonsoft.Json;
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

namespace OrganigramService
{
    public class TcpIpServer : ITcpIpCommunication
    {
        #region Fields

        #endregion
        #region Properties
        private TcpListener Listener { get; set; }
        private ILogger Logger { get; set; }
        public TcpClient Client { get; set; }
        public OrganizationDatabase DataBase { get; set; }
        public StreamReader StreamReader { get; set; }
        public StreamWriter StreamWriter { get; set; }
        #endregion

        #region Constructor
        public TcpIpServer(ILogger log, OrganizationDatabase database)
        {
            Logger = log ?? throw new ArgumentNullException(nameof(log));
            DataBase = database ?? throw new ArgumentNullException(nameof(database));
        }
        #endregion

        #region Methods
        public void StartConnection(string ipAddress, int port)
        {
            var ipAddr = IPAddress.Parse(ipAddress);

            Listener = new TcpListener(ipAddr, port);
            Listener.Start();
            Thread serverThread = new Thread(new ThreadStart(ListenForClients));
            serverThread.Start();
            Logger.Log($"Server is listening on {ipAddress}:{port}");

        }

        private void ListenForClients()
        {
            Listener.Start();
            Logger.Log("Server started");

            while (true)
            {
                TcpClient client = this.Listener.AcceptTcpClient();
                Logger.Log("Client connected");

                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                clientThread.Start(client);
            }
        }

        public void HandleClient(object client)
        {
            // retrieve client from parameter passed to thread
            TcpClient tcpClient = (TcpClient)client;

            // sets two streams
            StreamWriter = new StreamWriter(tcpClient.GetStream(), Encoding.ASCII) { AutoFlush = true };
            StreamReader = new StreamReader(tcpClient.GetStream(), Encoding.ASCII);

            while (true)
            {
                // reads from stream
                var dataString = StreamReader.ReadLine();
                Logger.Log(dataString);
                string[] splitted = dataString.Split(';');
                string command = splitted[0];

                if(command == "GetOrganization")
                {
                    var person = DataBase.GetOrganization();

                    var jsonString = JsonConvert.SerializeObject(person);
                    var data = $"GetOrganization;{jsonString}";
                    StreamWriter.WriteLine(data);
                    StreamWriter.Flush();
                }
                else if(command == "GetPersons")
                {
                    var persons = DataBase.GetPersons();

                    var jsonString = JsonConvert.SerializeObject(persons);
                    var data = $"GetPersons;{jsonString}";
                    StreamWriter.WriteLine(data);
                    StreamWriter.Flush();
                }
                else if (command == "GetRoles")
                {
                    var roles = DataBase.GetRoles();

                    var jsonString = JsonConvert.SerializeObject(roles);
                    var data = $"GetRoles;{jsonString}";
                    StreamWriter.WriteLine(data);
                    StreamWriter.Flush();
                }
                else if (command == "GetPersonsToReportTo")
                {
                    var persons = DataBase.GetPersonsToReportTo(splitted[1]);

                    var jsonString = JsonConvert.SerializeObject(persons);
                    var data = $"GetPersons;{jsonString}";
                    StreamWriter.WriteLine(data);
                    StreamWriter.Flush();
                }
            }
        }

        public void CloseConnection()
        {
            Listener.Stop();
            Client.Close();
            StreamReader.Close();
            StreamWriter.Close();
            Logger.Log("Server is closed");
        }


        public void SendData()
        {
            
            StreamWriter.WriteLine("this is the Server");
            StreamWriter.Flush();

        }

        public string ReadData()
        {
            throw new NotImplementedException();
        }

        public void SendData(string message)
        {
            StreamWriter.WriteLine(message);
            StreamWriter.Flush();
        }


        #endregion

    }

    
}
