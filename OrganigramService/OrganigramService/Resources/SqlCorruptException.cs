using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganigramService.Resources
{
    public class SqlCorruptException : Exception
    {
        /// <summary>
        /// Special Sql Exception
        /// </summary>
        /// <param name="query"></param>
        public SqlCorruptException(string query) : base($"This Sql Query Command was corrupt: {query}")
        {

        }
        /// <summary>
        /// Special Sql Exception 
        /// </summary>
        public SqlCorruptException(): base("A error happened during SqlCommunication")
        {

        }
    }
}
