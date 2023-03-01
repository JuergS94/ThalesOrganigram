using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrganigramService;
using OrganigramService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace OrganigramServiceTests
{
    [TestClass]
    public class DataBaseHandlerTests
    {
        private string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Organigram\OrganigramService\OrganigramService\OrgDatabase.mdf;Integrated Security=True";


        [TestMethod]
        public void Select_Should_Return_Correct_Items()
        {
            // Arrange
            var expectedItems = new Collection<JobTitle>
        {
            new JobTitle { Name = "CEO                      " }
        };

            // Set up a mock data reader with the expected values
            var mockDataReader = new Mock<IDataReader>();
            mockDataReader.SetupSequence(x => x.Read())
                .Returns(true)
                .Returns(true)
                .Returns(false);
            mockDataReader.Setup(x => x["Name"]).Returns(expectedItems[0].Name);

            // Set up a mock command that returns the mock data reader
            var mockCommand = new Mock<IDbCommand>();
            mockCommand.Setup(x => x.ExecuteReader()).Returns(mockDataReader.Object);

            // Set up a mock connection that returns the mock command
            var mockConnection = new Mock<IDbConnection>();
            mockConnection.Setup(x => x.CreateCommand()).Returns(mockCommand.Object);

            // Set up the Select function with the mock connection
            var myClass = new DataBaseHandler();
            var actualItems = myClass.Select("SELECT * FROM JobTitles", reader =>
                new JobTitle
                {
                    Name = reader.GetString(reader.GetOrdinal("JobTitle"))
                }
            );

            // Assert
            Assert.AreEqual(expectedItems[0].Name, actualItems[0].Name);
            
           
        }
    }
}
