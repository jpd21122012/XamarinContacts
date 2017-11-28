using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using XamContacts.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using XamContacts.Helpers;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace XamContacts.Data
{
    public class ContactsManager
    {
        static ContactsManager defaultInstance
            = new ContactsManager();
        private IMobileServiceClient client;
        private IMobileServiceSyncTable<Contact> contactsTable;

        private ContactsManager()
        {
            client = new MobileServiceClient("http://xamarinjpd.azurewebsites.net");
            var store = new MobileServiceSQLiteStore("contacts2.db");
            store.DefineTable<Contact>();
            client.SyncContext.InitializeAsync(store);

            contactsTable = client.GetSyncTable<Contact>();
        }
        public static ContactsManager DefaultManager
        {
            get { return defaultInstance; }
            private set { defaultInstance = value; }
        }
        public async Task<ObservableCollection<Contact>> GetItemsAsync(bool syncItems = false)
        {
            try
            {
                if (syncItems)
                {
                    await SyncAsync();
                }
                IEnumerable<Contact> items = await contactsTable.ToEnumerableAsync();
                return new ObservableCollection<Contact>(items);
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception a)
            {
                Debug.WriteLine(a.Message);
            }
            return null;
        }
        public async Task<ObservableCollection<Grouping<string, Contact>>>
           GetItemsGroupedAsync(bool syncItems = false)
        {
            try
            {
                if (syncItems)
                {
                    await SyncAsync();
                }
                    IEnumerable<Contact> contacts =
     await GetItemsAsync();
                    IEnumerable<Grouping<string, Contact>> sorted =
                        new Grouping<string, Contact>[0];
                    if (contacts != null)
                    {
                        sorted =
                            from c in contacts
                            orderby c.Name
                            group c by c.Name[0].ToString()
                            into theGroup
                            select new Grouping<string, Contact>
                                (theGroup.Key, theGroup);
                    }
                return new ObservableCollection<Grouping<string, Contact>>(sorted);
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception a)
            {
                Debug.WriteLine(a.Message);
            }
            return null;
        }

        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError>
                syncErrors = null;
            try
            {
                await client.SyncContext.PushAsync();
                contactsTable.PullAsync("allCustomers",contactsTable.CreateQuery());
            }
            catch (MobileServicePushFailedException ex)
            {
                if (ex.Source!=null)
                {
                    syncErrors = ex.PushResult.Errors;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            if (syncErrors!=null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update)
                    {
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else {
                        await error.CancelAndDiscardItemAsync();
                    }
                }
            }
        }

        public async Task<Contact> GetItemAsync(string id)
        {
            var items = await contactsTable.Where(i => i.Id == id)
                .ToListAsync();
            return items.FirstOrDefault();
        }
        public async Task SaveItemAsync(Contact item)
        {
            try
            {
                if (item.Id != null)
                {
                    await contactsTable.UpdateAsync(item);
                }
                else
                {
                    await contactsTable.InsertAsync(item);
                }
            }
            catch (Exception eex)
            {
                Debug.WriteLine(eex.Message);
            }
        }
    }
}