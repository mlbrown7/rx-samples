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

            EditContactCommand = ReactiveCommand.Create<Contact>((contact) =>
            {
                contact.IsEditing = true;
                contact.IsNotEditing = false;
            });

            CancelEditContactCommand = ReactiveCommand.Create<Contact>((contact) =>
            {
                contact.IsEditing = false;
                contact.IsNotEditing = true;
            });

            SaveContactCommand = ReactiveCommand.Create<Contact>((contact) =>
            {
                contact.Name = contact.NameEdit;
                contact.Age = contact.AgeEdit;
                MessageBox.Show($"Saved changes for {contact.Name}");

                contact.IsEditing = false;
                contact.IsNotEditing = true;
            });
        }

        ReactiveList<Contact> _contacts;
        public ReactiveList<Contact> Contacts
        {
            get => _contacts;
            set => this.RaiseAndSetIfChanged(ref _contacts, value);
        }

        public ReactiveCommand EditContactCommand { get; set; }
        public ReactiveCommand CancelEditContactCommand { get; set; }
        public ReactiveCommand SaveContactCommand { get; set; }

        private List<Contact> GetContactList()
        {
            var list = new List<Contact>();
            list.Add(new Contact(1, "Bob Parr", 37));
            list.Add(new Contact(1, "Han Solo", 32));
            list.Add(new Contact(1, "Luke Skywalker", 24));
            list.Add(new Contact(1, "Steve Rogers", 22));
            return list;
        }
    }


    public class Contact : ReactiveObject
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
    }
}
