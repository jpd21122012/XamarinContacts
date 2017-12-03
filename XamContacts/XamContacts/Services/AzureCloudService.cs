﻿using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamContacts.Abstractions;
using XamContacts.Model;

namespace XamContacts.Services
{
    public class AzureCloudService : ICloudService
    {
        MobileServiceClient client;
        public AzureCloudService()
        {
            client = new MobileServiceClient("https://xamarinjpdbackend.azurewebsites.net");
            App.CurrentClient = client;
        }
        public async Task<ICloudTable<T>> GetTableAsync<T>() where T : TableData
        {
            await InitializeAsync();
            return new AzureCloudTable<T>(client);
        }

        public bool IsUserLogged()
        {
            bool isUserLogged = client.CurrentUser != null;
            return isUserLogged;
        }

        public Task LoginAsync()
        {
            var loginProvider = DependencyService.Get<ILoginProvider>();
            return loginProvider.LoginAsync(client);
        }

        async Task InitializeAsync()
        {
            if (client.SyncContext.IsInitialized)
            {
                return;
            }
                var store = new MobileServiceSQLiteStore("offlinecache.db");
                store.DefineTable<Contact>();
                await client.SyncContext.InitializeAsync(store);
        }
    }
}