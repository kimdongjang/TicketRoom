using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.PointData
{
    public class PT_Charge
    {
        [JsonProperty("PT_CHARGE_INDEX")]
        public int PT_CHARGE_INDEX { get; set; } // 자신 인덱스
        [JsonProperty("PT_CHARGE_CONTENT")]
        public string PT_CHARGE_CONTENT { get; set; } // 상세내용(생일 적립, 회원가입 적립 etc)
        [JsonProperty("PT_CHARGE_BANK")]
        public string PT_CHARGE_BANK { get; set; } // 결제은행
        [JsonProperty("PT_CHARGE_CARD")]
        public string PT_CHARGE_CARD { get; set; } // 결제카드
        [JsonProperty("USER_ID")]
        public string USER_ID { get; set; } // 유저 아이디
        [JsonProperty("PT_CHARGE_POINT")]
        public int PT_CHARGE_POINT { get; set; } // 적립 포인트
        [JsonProperty("PT_CHARGE_DATE")]
        public string PT_CHARGE_DATE { get; set; } // 적립 날짜
        [JsonProperty("PT_POINT_INDEX")]
        public int PT_POINT_INDEX { get; set; } // 포인트 리스트 인덱스
    }
}
