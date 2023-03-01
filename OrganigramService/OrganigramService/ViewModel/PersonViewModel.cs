using OrganigramService.Model;
using OrganigramService.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganigramService.ViewModel
{
    public class PersonViewModel : ValidateObservableObject
    {
        #region Data

        readonly ObservableCollection<PersonViewModel> _children;
        readonly PersonViewModel _parent;
        readonly Person _person;

        bool _isExpanded;
        bool _isSelected;

        #endregion // Data

        #region Constructors

        /// <summary>
        /// Constructor of the PersonViewmodel
        /// </summary>
        /// <param name="person"></param>
        public PersonViewModel(Person person)
            : this(person, null)
        {
        }

        /// <summary>
        /// Constructor of the PersonViewModel
        /// </summary>
        /// <param name="person"></param>
        /// <param name="parent"></param>
        private PersonViewModel(Person person, PersonViewModel parent)
        {
            _person = person;
            _parent = parent;

            _children = new ObservableCollection<PersonViewModel>(
                    (from child in _person.Children
                     select new PersonViewModel(child, this))
                     .ToList<PersonViewModel>());
        }

        #endregion // Constructors

        #region Person Properties

        /// <summary>
        /// All Children Elements of the Person
        /// </summary>
        public ObservableCollection<PersonViewModel> Children
        {
            get { return _children; }
        }

        /// <summary>
        ///  Name of the person
        /// </summary>
        public string Name
        {
            get { return _person.Name; }
        }

        /// <summary>
        /// JobTitle of the Person
        /// </summary>
        public string JobTitle
        {
            get { return _person.JobTitle; }
        }

        #endregion // Person Properties

        #region Presentation Members

        #region IsExpanded

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (_isExpanded && _parent != null)
                    _parent.IsExpanded = true;
            }
        }

        #endregion // IsExpanded

        #region IsSelected

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        #endregion // IsSelected

        #region NameContainsText

        public bool NameContainsText(string text)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(this.Name))
                return false;

            return this.Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        #endregion // NameContainsText

        #region Parent

        /// <summary>
        /// Parent Person of the current Person
        /// </summary>
        public PersonViewModel Parent
        {
            get { return _parent; }
        }

        #endregion // Parent

        #endregion // Presentation Members        

        
    }
}
