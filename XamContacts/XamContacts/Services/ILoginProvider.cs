using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamContacts.Services
{
    public interface ILoginProvider
    {
        Task LoginAsync(MobileServiceClient client);
    }
}
