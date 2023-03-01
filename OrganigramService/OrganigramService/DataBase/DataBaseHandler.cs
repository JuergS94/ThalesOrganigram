using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganigramService
{
    public class DataBaseHandler : IDataBaseHandler
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Organigram\OrganigramService\OrganigramService\OrgDatabase.mdf;Integrated Security=True";

        public DataBaseHandler()
        {

        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public Collection<T> Select<T>(string query, Func<IDataRecord, T> map)
        {
            Collection<T> result = new Collection<T>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T item = map(reader);
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int Select(string query, params SqlParameter[] parameters)
        {
            var result = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddRange(parameters);
                connection.Open();
                result = (int)command.ExecuteScalar();
            }
            return result;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        public void Insert(string query, params SqlParameter[] parameters)
        {
            using(var connection  = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddRange(parameters);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        public void Update(string query, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddRange(parameters);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        public void Delete(string query, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddRange(parameters);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
