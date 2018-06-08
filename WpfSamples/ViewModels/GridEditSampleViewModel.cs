using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                contact.Validate();
                
                if (!contact.HasErrors)
                {
                    contact.Name = contact.NameEdit;
                    contact.Age = contact.AgeEdit;
                    MessageBox.Show($"Saved changes for {contact.Name}");

                    contact.IsEditing = false;
                    contact.IsNotEditing = true;
                }
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

}
