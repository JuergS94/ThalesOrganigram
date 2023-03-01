using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OrganigramService.Resources
{
    public class ValidateObservableObject : ObservableObject, INotifyDataErrorInfo
    {
        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        public bool HasErrors => errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName is null)
            {
                return null;
            }

            return errors.ContainsKey(propertyName) ? errors[propertyName] : null;
        }

        protected new bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            bool ret = base.SetProperty(ref field, newValue, propertyName);
            ValidateProperty(propertyName, newValue);

            return ret;
        }

        private void ValidateProperty<T>(string propertyName, T newValue)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this);
            context.MemberName = propertyName;
            Validator.TryValidateProperty(newValue, context, results);

            if (results.Any())
            {
                errors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                errors.Remove(propertyName);
            }

            ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));

        }
    }
}
