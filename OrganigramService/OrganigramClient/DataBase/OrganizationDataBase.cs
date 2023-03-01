using Newtonsoft.Json;
using OrganigramClient.Model;
using OrganigramEssentials;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrganigramClient
{
    /// <summary>
    /// This Class can be used to communicate with the Database over TCP/IP
    /// </summary>
    public class OrganizationDataBase
    {
        #region Properties

        public ITcpIpCommunication TcpIpClient { get; set; }
        #endregion
        #region Constructor

        /// <summary>
        /// Constructor of the Organization DataBase
        /// </summary>
        /// <param name="communication"></param>
        public OrganizationDataBase(ITcpIpCommunication communication)
        {
            TcpIpClient = communication ?? throw new ArgumentNullException(nameof(communication));
        }

        #endregion

        /// <summary>
        /// Get the whole Organigram of the datbase
        /// </summary>
        /// <returns></returns>
        public Person GetOrganization()
        {

            var key = "GetOrganization";
            var person = new Person();
            TcpIpClient.SendData($"{key};");
            Thread.Sleep(2000);
            var data = TcpIpClient.ReadData();
            var splitted = data.Split(';');
            if (splitted[0] == key)
            {
                person = JsonConvert.DeserializeObject<Person>(splitted[1]);

            }
            return person;
        }

        /// <summary>
        /// Get all persons of the person table
        /// </summary>
        /// <returns></returns>
        public Collection<Person> GetPersons()
        {

            var key = "GetPersons";
            var persons = new Collection<Person>();
            TcpIpClient.SendData($"{key};");
            Thread.Sleep(100);
            var data = TcpIpClient.ReadData();
            var splitted = data.Split(';');
            if (splitted[0] == key)
            {
                persons = JsonConvert.DeserializeObject<Collection<Person>>(TcpIpClient.ReadData());
            }

            return persons;
        }

        /// <summary>
        /// Get all JobTitles of the JobTitles Table
        /// </summary>
        /// <returns></returns>
        public Collection<JobTitle> GetRoles()
        {
            var key = "GetRoles";
            var jobTitles = new Collection<JobTitle>();
            TcpIpClient.SendData($"{key};");
            Thread.Sleep(100);
            var data = TcpIpClient.ReadData();
            var splitted = data.Split(';');
            if (splitted[0] == key)
            {

                jobTitles = JsonConvert.DeserializeObject<Collection<JobTitle>>(TcpIpClient.ReadData());

            }
            return jobTitles;
        }

        /// <summary>
        /// Get all personsToReport to according to the jobTitle
        /// </summary>
        /// <param name="jobTitle"></param>
        /// <returns></returns>
        public Collection<Person> GetPersonsToReportTo(string jobTitle)
        {
            var key = "GetPersonsToReportTo";
            var persons = new Collection<Person>();
            TcpIpClient.SendData($"{key};{jobTitle}");
            Thread.Sleep(100);
            var data = TcpIpClient.ReadData();
            var splitted = data.Split(';');
            if (splitted[0] == key)
            {
                persons = JsonConvert.DeserializeObject<Collection<Person>>(TcpIpClient.ReadData());
            }

            return persons;
        }


    }
}
