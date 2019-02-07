using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.PointData
{
    public class PT_WithDraw
    {
        [JsonProperty("PT_WITHDRAW_INDEX")]
        public int PT_WITHDRAW_INDEX { get; set; } // 자신 인덱스
        [JsonProperty("PT_WITHDRAW_CONTENT")]
        public string PT_WITHDRAW_CONTENT { get; set; } // 상세내용(생일 적립, 회원가입 적립 etc)
        [JsonProperty("PT_WITHDRAW_BANK")]
        public string PT_WITHDRAW_BANK { get; set; } // 출금은행
        [JsonProperty("PT_WITHDRAW_ACCOUNT")]
        public string PT_WITHDRAW_ACCOUNT { get; set; } // 출금계좌
        [JsonProperty("PT_WITHDRAW_NAME")]
        public string PT_WITHDRAW_NAME { get; set; } // 예금주
        [JsonProperty("USER_ID")]
        public string USER_ID { get; set; } // 유저 아이디
        [JsonProperty("PT_WITHDRAW_POINT")]
        public int PT_WITHDRAW_POINT { get; set; } // 적립 포인트
        [JsonProperty("PT_WITHDRAW_DATE")]
        public string PT_WITHDRAW_DATE { get; set; } // 적립 날짜
        [JsonProperty("PT_POINT_INDEX")]
        public int PT_POINT_INDEX { get; set; } // 포인트 리스트 인덱스
    }
}
