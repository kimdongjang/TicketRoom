using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.PointData
{
    public class PT_Point
    {
        [JsonProperty("PT_POINT_INDEX")]
        public int PT_POINT_INDEX { get; set; } // 자신 인덱스
        [JsonProperty("USER_ID")]
        public string USER_ID { get; set; } // 유저 아이디
        [JsonProperty("PT_POINT_HAVEPOINT")]
        public int PT_POINT_HAVEPOINT { get; set; } // 보유 포인트(잔여 포인트)
    }
}
