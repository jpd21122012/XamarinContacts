using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Newtonsoft.Json;
using XamContacts.Abstractions;

namespace XamContacts.Model
{
    public class Contact:TableData
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("notes")]
        public string Notes { get; set; }
    }
}
