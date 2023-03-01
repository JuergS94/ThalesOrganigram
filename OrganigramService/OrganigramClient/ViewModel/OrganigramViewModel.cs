using Microsoft.Toolkit.Mvvm.Input;
using OrganigramClient.Resources;
using OrganigramEssentials;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganigramClient.ViewModel
{
    public class OrganigramViewModel : ValidateObservableObject
    {
        private ObservableCollection<PersonViewModel> _firstGeneration;
        private string _selectedRole;
        private string _selectedPersonToReportTo;
        private string _nameOfPerson;
        private string _selectedPerson;
        private ObservableCollection<string> _roles;
        private ObservableCollection<string> _personsToReport;

        #region Properties


        public OrganizationDataBase DataBase { get; set; }
        public ILogger Logger { get; set; }
        public ITcpIpCommunication TcpIpClient { get; set; }
        public IAsyncRelayCommand AddPersonCommand { get; }
        public IAsyncRelayCommand UpdateOrgCommand { get; }
        public IAsyncRelayCommand RemovePersonCommand { get; }
        public IAsyncRelayCommand UpdatePersonCommand { get; }
        public IAsyncRelayCommand SelectedPersonCommand { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of OrganigramViewModel
        /// </summary>
        /// <param name="dataBase"></param>
        /// <param name="log"></param>
        public OrganigramViewModel(OrganizationDataBase dataBase, ILogger log, ITcpIpCommunication communication)
        {
            TcpIpClient = communication ?? throw new ArgumentNullException(nameof(communication));
            DataBase = dataBase ?? throw new ArgumentNullException(nameof(dataBase));
            Logger = log ?? throw new ArgumentNullException(nameof(log));
            AddPersonCommand = new AsyncRelayCommand(AddPerson);
            UpdatePersonCommand = new AsyncRelayCommand(UpdatePerson);
            RemovePersonCommand = new AsyncRelayCommand(RemovePerson);
            UpdateOrgCommand = new AsyncRelayCommand(UpdateOrganization);
            SelectedPersonCommand = new AsyncRelayCommand<PersonViewModel>(SelectPerson);
           // var rolelist = DataBase.GetRoles().Select(x => x.Name.ToString());
            //Roles = new ObservableCollection<string>(rolelist);

            //var rootPerson = DataBase.GetOrganization();
            //FirstGeneration = new ObservableCollection<PersonViewModel>(
            //    new PersonViewModel[]
            //    {
            //        new PersonViewModel(rootPerson)
            //    });

        }

        #endregion
        /// <summary>
        /// Returns the SelectedPerson of the ComboxBox
        /// </summary>
        public string SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                if (_selectedPerson != value)
                {
                    _selectedPerson = value;
                    SetProperty(ref _selectedPerson, value);
                }
            }
        }
        /// <summary>
        /// Has the whole Information about the the TreeStructure of the Organigram
        /// </summary>
        public ObservableCollection<PersonViewModel> FirstGeneration
        {
            get { return _firstGeneration; }
            set
            {
                SetProperty(ref _firstGeneration, value);
            }
        }

        /// <summary>
        /// Returns all JobTitles
        /// </summary>
        public ObservableCollection<string> Roles
        {
            get => _roles;
            set
            {
                SetProperty(ref _roles, value);
            }
        }

        /// <summary>
        /// Shows all persons to report to
        /// </summary>
        public ObservableCollection<string> PersonsToReport
        {
            get => _personsToReport;
            set => SetProperty(ref _personsToReport, value);
        }

        /// <summary>
        /// Returns the selected Persons to report to of the ComboBox
        /// </summary>
        public string SelectedPersonToReportTo
        {
            get => _selectedPersonToReportTo;
            set => SetProperty(ref _selectedPersonToReportTo, value);
        }

        /// <summary>
        /// Returns the selected Role of the ComboBox
        /// </summary>
        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                SetProperty(ref _selectedRole, value);
                if (_selectedRole != null)
                {
                    UpdatePersonsToReport();
                }
            }
        }


        /// <summary>
        /// Has the Name of the Textbox
        /// </summary>
        public string NameOfPerson
        {
            get => _nameOfPerson;
            set => SetProperty(ref _nameOfPerson, value);
        }


        #region Methods

        /// <summary>
        /// Update the ComboxBox for PersonsToReport
        /// </summary>
        public void UpdatePersonsToReport()
        {
            var personToReportList = DataBase.GetPersonsToReportTo(SelectedRole).Select(x => x.Name.ToString()).ToList();
            personToReportList.RemoveAll(x => x == NameOfPerson);

            PersonsToReport = new ObservableCollection<string>(personToReportList);
        }

        /// <summary>
        /// Remove the selected Person from the Organigram
        /// </summary>
        /// <returns></returns>
        private async Task RemovePerson()
        {
            if (SelectedPerson != null)
            {
                //DataBase.RemovePerson(SelectedPerson);
                //UpdateOrganization();
            }
            else
            {
                Logger.Log("Select any Person in the Organigram");
            }
        }

        /// <summary>
        /// Returns the selected Person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        private async Task SelectPerson(PersonViewModel person)
        {
            SelectedPerson = person.Name;
            NameOfPerson = SelectedPerson;
        }

        /// <summary>
        /// Update the whole Organization
        /// </summary>
        /// <returns></returns>
        private async Task UpdateOrganization()
        {

            var rootPerson = DataBase.GetOrganization();
            FirstGeneration = new ObservableCollection<PersonViewModel>(
                   new PersonViewModel[]
                   {
                        new PersonViewModel(rootPerson)
                   });


            //var rolelist = DataBase.GetRoles().Select(x => x.Name.ToString());
            //Roles = new ObservableCollection<string>(rolelist);
        }
        /// <summary>
        /// Person to be added
        /// </summary>
        /// <returns></returns>
        private async Task AddPerson()
        {

            if (NameOfPerson is null)
            {
                Logger.Log("Person Textbox is empty. Please fill in a name");
                return;
            }
            if (SelectedRole is null)
            {
                Logger.Log("Please select any Role");
                return;
            }

            if (SelectedPersonToReportTo is null)
            {
                Logger.Log("Please select a person to report to");
                return;
            }

            //DataBase.AddPerson(NameOfPerson, SelectedRole, SelectedPersonToReportTo);
            UpdateOrganization();
        }

        /// <summary>
        /// Update Person to new Role
        /// </summary>
        /// <returns></returns>
        private async Task UpdatePerson()
        {
            if (NameOfPerson is null)
            {
                Logger.Log("Person Textbox is empty. Please fill in a name");
                return;
            }
            if (SelectedRole is null)
            {
                Logger.Log("Please select any Role");
                return;
            }

            if (SelectedPersonToReportTo is null)
            {
                Logger.Log("Please select a person to report to");
                return;
            }

            //DataBase.UpdatePerson(NameOfPerson, SelectedRole, SelectedPersonToReportTo);
            UpdateOrganization();

        }


        #endregion
    }
}
