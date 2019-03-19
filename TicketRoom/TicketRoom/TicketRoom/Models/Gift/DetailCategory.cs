using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.Gift
{
    public class DetailCategory
    {
        [JsonProperty("DETAILCATEGORYNUM")]
        public string DETAILCATEGORYNUM { get; set; } // 
        [JsonProperty("CATEGORYNUM")]
        public string CATEGORYNUM { get; set; } // 
        [JsonProperty("PRODUCTTYPE")]
        public string PRODUCTTYPE { get; set; }// 
        [JsonProperty("PRODUCTIMAGE")]
        public string PRODUCTIMAGE { get; set; } // 
    }
}