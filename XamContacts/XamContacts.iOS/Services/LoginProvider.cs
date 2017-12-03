using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using XamContacts.iOS.Services;
using XamContacts.Services;
using UIKit;

[assembly:Dependency(typeof(LoginProvider))]
namespace XamContacts.iOS.Services
{
    public class LoginProvider : ILoginProvider
    {
        public UIViewController RootViewController =>
            UIApplication.SharedApplication.KeyWindow.RootViewController;
        public async Task LoginAsync(MobileServiceClient client)
        {
            var user = await client.LoginAsync(RootViewController,
                MobileServiceAuthenticationProvider.Facebook,
                "xamarinjpdbackend");
        }
    }
}