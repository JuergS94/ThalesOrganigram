using Newtonsoft.Json;
using OrganigramEssentials;
using OrganigramService.Model;
using OrganigramService.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganigramService
{
    public class OrganizationDatabase
    {
        #region Properties

        public IDataBaseHandler DataBaseHandler { get; set; }
        public ILogger Logger { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataBaseHandler"></param>
        /// <param name="logger"></param>
        public OrganizationDatabase(IDataBaseHandler dataBaseHandler, ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            DataBaseHandler = dataBaseHandler ?? throw new ArgumentNullException(nameof(dataBaseHandler));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the organization table
        /// </summary>
        /// <returns></returns>
        public Person GetOrganization()
        {
            var query = "SELECT O.JobTitleId, O.ReportsToRoleId, P.Name, P.Id AS 'PId', J.JobTitle " +
                        "FROM Organization O " +
                        "Left Join Persons P On O.PersonId = P.Id " +
                        "Left Join JobTitles J On O.JobTitleId = J.Id";

            var ceo = new Person();
            try
            {
                var persons = DataBaseHandler.Select(query, (record) =>
                {
                    return new Person
                    {
                        Name = (string)record["Name"],
                        PersonId = (int)record["PId"],
                        JobTitle = (string)record["JobTitle"],
                        JobTitleId = (int)record["JobTitleId"],
                        ReportsToRoleId = (int)record["ReportsToRoleId"]
                    };

                });
                ceo = persons.Where(x => x.ReportsToRoleId == x.PersonId).First();
                persons.Remove(ceo);

                persons.OrderBy(x => x.ReportsToRoleId);

                foreach (var person in persons)
                {
                    var children = persons.Where(x => x.ReportsToRoleId == person.PersonId).ToList();
                    if (children.Count > 0)
                    {
                        person.Children = children;
                    }
                }
                var childrenOfCeo = persons.Where(x => x.ReportsToRoleId == ceo.PersonId).ToList();
                if (childrenOfCeo.Count() > 0)
                {
                    ceo.Children = childrenOfCeo;
                }


            }
            catch (SqlCorruptException)
            {

                throw new SqlCorruptException(query);
            }
            

            return ceo;
        }

        /// <summary>
        /// Get all Persons from Person table
        /// </summary>
        /// <returns></returns>
        public Collection<Person> GetPersons()
        {
            var persons = new Collection<Person>();
            var query = "SELECT Name FROM Persons";
            try
            {
                persons = DataBaseHandler.Select(query, (record) =>
                {
                    return new Person
                    {
                        Name = (string)record["Name"]
                    };
                });
            }
            catch (SqlCorruptException)
            {

                throw new SqlCorruptException(query);
            }
             

            return persons;
        }

        public Collection<Person> GetPersonsWithNoRoles()
        {
            var query = "SELECT Name FROM Persons WHERE Id NOT IN(SELECT PersonId FROM Organization)";

            var persons = new Collection<Person>();
            try
            {
                persons = DataBaseHandler.Select(query, (record) =>
                {
                    return new Person
                    {
                        Name = (string)record["Name"]
                    };
                });
            }
            catch (SqlCorruptException)
            {

                throw new SqlCorruptException(query);
            }


            return persons;
        }
       

        /// <summary>
        /// Get all Job roles/title
        /// </summary>
        /// <returns></returns>
        public Collection<JobTitle> GetRoles()
        {
            var query = "SELECT JobTitle FROM JobTitles";

            var persons = new Collection<JobTitle>();
            try
            {
                 persons = DataBaseHandler.Select(query, (record) =>
                {
                    return new JobTitle
                    {
                        Name = (string)record["JobTitle"]
                    };
                });
            }
            catch (SqlCorruptException)
            {

                throw new SqlCorruptException(query);
            }
            

            return persons;
        }

        /// <summary>
        /// Get all Persons which the jobTitle can report to
        /// </summary>
        /// <param name="jobTitle"></param>
        /// <returns></returns>
        public Collection<Person> GetPersonsToReportTo(string jobTitle)
        {

            if (jobTitle is null) throw new ArgumentNullException();

            var query = "SELECT O.JobTitleId, P.Name, J.JobTitle " +
                        "FROM Organization O " +
                        "Left Join Persons P On O.PersonId = P.Id " +
                        "Left Join JobTitles J On O.JobTitleId = J.Id";

            try
            {
                var persons = DataBaseHandler.Select(query, (record) =>
                {
                    return new Person
                    {
                        Name = (string)record["Name"],
                        JobTitleId = (int)record["JobTitleId"],
                        JobTitle = (string)record["JobTitle"]
                    };
                });

                var jobTitleId = persons.Where(x => x.JobTitle == jobTitle).First().JobTitleId;

                var filteredPersons = persons.Where(x => x.JobTitleId == jobTitleId - 1).ToList();
                var personsCollection = new Collection<Person>(filteredPersons);

                return personsCollection;
            }
            catch (SqlCorruptException)
            {

                throw new SqlCorruptException(query);
            }
            
        }

        /// <summary>
        /// Update a Person in the Persons Table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="jobTitle"></param>
        /// <param name="personToReportTo"></param>
        public void UpdatePerson(string name, string jobTitle, string personToReportTo)
        {


            string query = "UPDATE Organization SET JobTitleID = @newJobTitleID WHERE PersonID = @personID";

            try
            {

                var jobTitleId = GetJobTitleId(jobTitle);

                var personId = GetPersonId(name);

                var count = CheckIfPersonExistInOrganization(personId);
                if (count > 0)
                {
                    var nameParam = new SqlParameter("@newJobTitleID", jobTitleId);
                    var personParam = new SqlParameter("@personID", personId);
                    DataBaseHandler.Update(query, nameParam, personParam);

                    query = "UPDATE Organization SET ReportsToRoleId = @newReportsToRoleId WHERE PersonID = @personID";


                    var reportsToRoleId = GetPersonId(personToReportTo);

                    personParam = new SqlParameter("@personID", personId);
                    var reportsToRoleParam = new SqlParameter("@newReportsToRoleId", reportsToRoleId);
                    DataBaseHandler.Update(query, reportsToRoleParam, personParam);
                    
                }
                else
                {
                    AddPerson(name, jobTitle, personToReportTo);
                }
                Logger.Log($"Person: {name} was updated");

            }
            catch (SqlCorruptException)
            {

                throw new SqlCorruptException(query);
            }

        }

        /// <summary>
        /// Add a Person to the Persons Table and to the Organization Table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="jobTitle"></param>
        /// <param name="personToReportTo"></param>
        public void AddPerson(string name, string jobTitle, string personToReportTo)
        {
            if (name is null) throw new ArgumentNullException();
            if (jobTitle is null) throw new ArgumentNullException();
            if (personToReportTo is null) throw new ArgumentNullException();

            var query = "INSERT INTO ORGANIZATION(PersonId, JobTitleId, ReportsToRoleId) " +
                        "VALUES(@PersonId, @JobTitleId, @ReportsToRoleId)";

            try
            {

                var personId = GetPersonId(name);
                var count = CheckIfPersonExistInOrganization(personId);


                if (count == 0)
                {
                    var count2 = CheckIfPersonExist(name);
                    if (count2 == 0)
                    {
                        AddPerson(name);
                    }

                    var reportsToRoleId = GetPersonId(personToReportTo);

                    var jobTitleId = GetJobTitleId(jobTitle);

                    var personIdParam = new SqlParameter("@PersonId", personId);
                    var jobTitleIdParam = new SqlParameter("@JobTitleId", jobTitleId);
                    var reportsToRoleIdParam = new SqlParameter("@ReportsToRoleId", reportsToRoleId);

                    DataBaseHandler.Insert(query, personIdParam, jobTitleIdParam, reportsToRoleIdParam);
                    Logger.Log($"Person was added: {name}");
                }
                else
                {
                    Logger.Log($"Person does already exist: {name}");
                }
            }
            catch (Exception)
            {

                throw new SqlCorruptException(query);
            }
        }

        /// <summary>
        /// Add a Person to the Persons table
        /// </summary>
        /// <param name="name"></param>
        public void AddPerson(string name)
        {
            if (name is null) throw new ArgumentNullException();

            var query = "INSERT INTO PERSONS(Name) " +
                           "VALUES(@Name)";

            try
            {
                var count = CheckIfPersonExist(name);

                if(count == 0)
                {
                    var nameParam = new SqlParameter("@Name", name);

                    DataBaseHandler.Insert(query, nameParam);
                    Logger.Log($"Person was added: {name}");
                }
                else
                {
                    Logger.Log($"Person already exist: {name}");
                }

            }
            catch (SqlCorruptException)
            {

                throw new SqlCorruptException(query);
            }
        }

        /// <summary>
        /// Returns the Id of a Person in the Person table
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetPersonId(string name)
        {
            if (name is null) throw new ArgumentNullException();

            var query = $"SELECT P.Id FROM Persons P WHERE P.Name = '{name}'";

            try
            {
                
                var personId = DataBaseHandler.Select(query, (record) =>
                {
                    return (int)record["Id"];
                }).First();

                return personId;
            }
            catch (SqlCorruptException)
            {
                throw new SqlCorruptException(query);
            }
            
        }

        /// <summary>
        /// Check in the Database if person exist
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int CheckIfPersonExistInOrganization(int id)
        {

            var query = "SELECT COUNT(*) FROM Organization WHERE PersonId= @PersonId";

            var nameParam = new SqlParameter("@PersonId", id);

            try
            {
                var count = DataBaseHandler.Select(query, nameParam);

                return count;
            }
            catch (SqlCorruptException)
            {
                throw new SqlCorruptException(query);
            }
        }

        /// <summary>
        /// Check in the Database if person exist
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int CheckIfPersonExist(string name)
        {
            if (name is null) throw new ArgumentNullException();

            var query = "SELECT COUNT(*) FROM Persons WHERE Name= @Name";

            var nameParam = new SqlParameter("@Name", name);

            try
            {
                var count = DataBaseHandler.Select(query, nameParam);

                return count;
            }
            catch (SqlCorruptException)
            {
                throw new SqlCorruptException(query);
            }
        }

        /// <summary>
        /// Returns the Id of a JobTitle in the JobTitles table
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetJobTitleId(string jobTitle)
        {
            if (jobTitle is null) throw new ArgumentNullException();

            var query = $"SELECT J.Id FROM JobTitles J WHERE J.JobTitle = '{jobTitle}'";
 
            try
            {

                var jobTitleId = DataBaseHandler.Select(query, (record) =>
                {
                    return (int)record["Id"];
                }).First();

                return jobTitleId;
            }
            catch (SqlCorruptException)
            {
                throw new SqlCorruptException(query);
            }

        }

        /// <summary>
        /// Remove Person from Person table and Organization table
        /// </summary>
        /// <param name="name"></param>
        public void RemovePerson(string name)
        {
            if (name is null) throw new ArgumentNullException();


            var query = "DELETE FROM Organization WHERE PersonId = @PersonId";

            try
            {
                var count = CheckIfPersonExist(name);

                if (count != 0)
                {
                    var personId = GetPersonId(name);

                    var personIdParam = new SqlParameter("@PersonId", personId);
                    DataBaseHandler.Delete(query, personIdParam);

                    query = "DELETE FROM Persons WHERE Name = @Name";
                    personIdParam = new SqlParameter("@Name", name);
                    DataBaseHandler.Delete(query, personIdParam);
                    Logger.Log($"Person was removed: {name}");
                }
                else
                {
                    Logger.Log($"Person was already removed: {name}");
                }
            }
            catch (SqlCorruptException)
            {
                throw new SqlCorruptException(query);
            }
        }


        


        #endregion

    }
}
