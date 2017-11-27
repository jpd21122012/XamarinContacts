using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamContacts.Helpers;
using XamContacts.Model;
using XamContacts.View;
using XamContacts.Data;

namespace XamContacts.ViewModel
{
    public class ContactsPageViewModel
    {
        public ObservableCollection<Grouping<string, Contact>> 
            ContactsList { get; set; }

        public Contact CurrentContact { get; set; }
        public Command AddContactCommand { get; set; }
        public Command ItemTappedCommand { get; }
        public INavigation Navigation { get; set; }

        public ContactsPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Task.Run(async () =>
                //ContactsList = await App.Database.GetItemsGroupedAsync()).Wait();
                ContactsList = await ContactsManager.DefaultManager.GetItemsGroupedAsync());
            AddContactCommand = new Command(async () =>await
            GoToContactDetailPage());
            ItemTappedCommand = new Command(async() => GoToContactDetailPage(CurrentContact));
        }

        public async Task GoToContactDetailPage(Contact contact = null)
        {
            if (contact == null)
            {
                await Navigation.PushAsync(new ContactDetailPage());
            }
            else
            {
                await Navigation.PushAsync(new ContactDetailPage(CurrentContact));
            }
        }
    }
}
