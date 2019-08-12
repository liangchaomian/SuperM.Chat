using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperM.Chat.Model
{
    public class TestUser
    {
        public string userId { get; set; }
        public string clientId { get; set; }
        public string token { get; set; }
        public string group { get; set; } = "1";
    }
}
