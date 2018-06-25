using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSamples.ViewModels
{
    public class Contact : ReactiveObject, INotifyDataErrorInfo
    {
        public Contact(int id, string name, int age)
        {
            IsEditing = false;
            IsNotEditing = true;

            //set properties
            Id = id;
            Name = name;
            NameEdit = name;
            Age = age;
            AgeEdit = age;

            PropertyErrors = new ConcurrentDictionary<string, List<string>>();
        }

        private int _id;
        public int Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string _nameEdit;
        public string NameEdit
        {
            get => _nameEdit;
            set => this.RaiseAndSetIfChanged(ref _nameEdit, value);
        }

        private int _age;
        public int Age
        {
            get => _age;
            set => this.RaiseAndSetIfChanged(ref _age, value);
        }

        private int _ageEdit;
        public int AgeEdit
        {
            get => _ageEdit;
            set => this.RaiseAndSetIfChanged(ref _ageEdit, value);
        }

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set => this.RaiseAndSetIfChanged(ref _isEditing, value);
        }

        private bool _isNotEditing;
        public bool IsNotEditing
        {
            get => _isNotEditing;
            set => this.RaiseAndSetIfChanged(ref _isNotEditing, value);
        }


        public void Validate()
        {
            ValidateNameEdit(this);
        }


        //Validation Methods

        private void ValidateNameEdit(Contact contact)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(contact.NameEdit))
            {
                errors.Add("Name is required.");
            }
            else
            {
                if (contact.NameEdit.Length < 3)
                {
                    errors.Add("Name must be at least 3 characters long.");
                }
                if (contact.Name.Length > 20)
                {
                    errors.Add("Name cannot be more than 20 characters long.");
                }
            }
            PropertyErrors[nameof(contact.NameEdit)] = errors;
            ErrorsChanged(this, new DataErrorsChangedEventArgs(nameof(contact.NameEdit)));
        }


        //INotifyDataErrorInfo

        public bool HasErrors => PropertyErrorsExist();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) { return null;  }

            if (PropertyErrors != null && PropertyErrors.ContainsKey(propertyName))
            {
                return PropertyErrors[propertyName];
            }
            return null;
        }

        private ConcurrentDictionary<string, List<string>> PropertyErrors { get; set; }


        private bool PropertyErrorsExist()
        {
            if (PropertyErrors != null && PropertyErrors.Any(p => p.Value != null && p.Value.Count > 0))
            {
                return true;
            }

            return false;
        }
    }
}
