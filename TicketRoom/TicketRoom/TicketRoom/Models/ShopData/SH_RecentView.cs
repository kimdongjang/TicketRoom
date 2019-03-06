using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.ShopData
{
    public class SH_RecentView
    {
        [JsonProperty("SH_RECENTVIEW_INDEX")]
        public int SH_RECENTVIEW_INDEX { get; set; } // 
        [JsonProperty("USER_ID")]
        public string USER_ID { get; set; } // 
        [JsonProperty("SH_HOME_INDEX1")]
        public int SH_HOME_INDEX1 { get; set; } // 1번
        [JsonProperty("SH_HOME_INDEX2")]
        public int SH_HOME_INDEX2 { get; set; } // 2번
        [JsonProperty("SH_HOME_INDEX3")]
        public int SH_HOME_INDEX3 { get; set; } // 3번

    }
}
