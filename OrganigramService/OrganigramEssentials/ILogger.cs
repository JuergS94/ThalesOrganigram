using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganigramEssentials
{
    public interface ILogger
    {
        /// <summary>
        /// Logs the Information to the GUI
        /// </summary>
        /// <param name="message"></param>
        void Log(string message);
    }
}
