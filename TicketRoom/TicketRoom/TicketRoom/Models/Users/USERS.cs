using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.Users
{
    public class USERS
    {
        [JsonProperty("ID")]
        public string ID { get; set; }
        [JsonProperty("EMAIL")]
        public string EMAIL { get; set; }
        [JsonProperty("NAME")]
        public string NAME { get; set; }
        [JsonProperty("PHONENUM")]
        public string PHONENUM { get; set; }
        [JsonProperty("TEMPPW")]
        public string TEMPPW { get; set; }
        [JsonProperty("RECOMMENDER")]
        public string RECOMMENDER { get; set; }
        [JsonProperty("REGISTDATE")]
        public string REGISTDATE { get; set; }
    }
}
