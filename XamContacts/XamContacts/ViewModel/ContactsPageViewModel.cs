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
using Plugin.Connectivity;
using System.Diagnostics;

namespace XamContacts.ViewModel
{
    public class ContactsPageViewModel
    {
        public ObservableCollection<Grouping<string, Contact>>
            ContactsList
        { get; set; }

        public Contact CurrentContact { get; set; }
        public Command AddContactCommand { get; set; }
        public Command ItemTappedCommand { get; }
        public Command LoginCommand { get; set; }
        public INavigation Navigation { get; set; }

        public ContactsPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            var isConnected = CrossConnectivity.Current.IsConnected;
            //Task.Run(async () =>
            //    //ContactsList = await App.Database.GetItemsGroupedAsync()).Wait();
            //    ContactsList = await App.CloudService.GetItemsGroupedAsync(isConnected)).Wait();

            Task.Run(async () =>
    ContactsList = await App.CloudService.GetTableAsync<Contact>().Result
    .GetItemsGroupedAsync(isConnected)).Wait();

            AddContactCommand = new Command(async () => await
            GoToContactDetailPage());
            ItemTappedCommand = new Command(async () => GoToContactDetailPage(CurrentContact));
            LoginCommand = new Command(async() => await TryLogin());
        }
        private async Task TryLogin()
        {
            try
            {
                var cloudService = App.CloudService;
                await cloudService.LoginAsync();
                Debug.WriteLine("Login Successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
           
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
