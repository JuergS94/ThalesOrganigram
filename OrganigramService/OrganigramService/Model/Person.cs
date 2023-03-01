using OrganigramService.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganigramService.Model
{
    public class Person : ValidateObservableObject
    {
       
        private List<Person> _children = new List<Person>();

        /// <summary>
        /// ChildrenElement of the Person
        /// </summary>
        public List<Person> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        /// <summary>
        /// Name of the Person
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// JobTitle of the Person
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// JobTitle Id of the Person
        /// </summary>
        public int JobTitleId { get; set; }

        /// <summary>
        /// PersonId of the Person
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// ReportsToRoleId of the Person
        /// </summary>
        public int ReportsToRoleId { get; set; }
    }
}
