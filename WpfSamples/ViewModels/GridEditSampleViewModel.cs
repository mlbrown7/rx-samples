using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfSamples.ViewModels
{
    public class GridEditSampleViewModel : ReactiveObject
    {
        public GridEditSampleViewModel()
        {
            Contacts = new ReactiveList<Contact> { ChangeTrackingEnabled = true };
            Contacts.AddRange(GetContactList());
        }

        ReactiveList<Contact> _contacts;
        public ReactiveList<Contact> Contacts
        {
            get => _contacts;
            set => this.RaiseAndSetIfChanged(ref _contacts, value);
        }

        private List<Contact> GetContactList()
        {
            var list = new List<Contact>();
            list.Add(new Contact { Id = 1, Name = "Bob Parr", Age = 37 });
            list.Add(new Contact { Id = 1, Name = "Han Solo", Age = 32 });
            list.Add(new Contact { Id = 1, Name = "Like Skywalker", Age = 24 });
            list.Add(new Contact { Id = 1, Name = "Steve Rogers", Age = 22 });
            return list;
        }

        public Contact SelectedContact { get; set; }

    }


    public class Contact : ReactiveObject
    {
        public Contact()
        {
            IsEditing = false;
            IsNotEditing = true;

            EditContactCommand = ReactiveCommand.Create(() =>
            {
                this.IsEditing = true;
                this.IsNotEditing = false;
            });

            CancelEditCommand = ReactiveCommand.Create(() =>
            {
                this.IsEditing = false;
                this.IsNotEditing = true;
            });
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

        public ReactiveCommand EditContactCommand { get; set; }
        public ReactiveCommand CancelEditCommand { get; set; }

    }
}
