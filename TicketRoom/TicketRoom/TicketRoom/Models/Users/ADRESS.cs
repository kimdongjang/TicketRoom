using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.Users
{
    public class ADRESS
    {
        [JsonProperty("ADDR_NUM")]
        public int ADDR_NUM { get; set; }
        [JsonProperty("USED_DATE")]
        public string USED_DATE { get; set; }
        [JsonProperty("USER_ID")]
        public string USER_ID { get; set; }
        [JsonProperty("ROADADDR")]
        public string ROADADDR { get; set; }
        [JsonProperty("JIBUNADDR")]
        public string JIBUNADDR { get; set; }
        [JsonProperty("ZIPNO")]
        public int ZIPNO { get; set; }
        [JsonProperty("SENDPHONE")]
        public string SENDPHONE { get; set; }
    }
}
