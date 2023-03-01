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
    public interface IDataBaseHandler
    {
        /// <summary>
        /// Select Data in Database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        Collection<T> Select<T>(string query, Func<IDataRecord, T> map);

        /// <summary>
        /// Get Scalar of selected Param
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        int Select(string query, params SqlParameter[] parameters);

        /// <summary>
        /// Insert Data to DataBase
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        void Insert(string query, params SqlParameter[] parameters);

        /// <summary>
        /// Update Data in DataBase
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        void Update(string query, params SqlParameter[] parameters);

        /// <summary>
        /// Delete Data in DataBase
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        void Delete(string query, params SqlParameter[] parameters);
    }
}
