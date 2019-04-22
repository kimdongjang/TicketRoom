using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.Gift.PurchaseList
{
    public class G_PinNumberProduct
    {
        [JsonProperty("PIN_GC_NUM")]
        public int PIN_GC_NUM { get; set; } // 핀번호상품권테이블번호
        [JsonProperty("PRONUM")]
        public int PRONUM { get; set; } // 상품번호
        [JsonProperty("PIN_GC_ISSUEDATE")]
        public string PIN_GC_ISSUEDATE { get; set; }// 발행날짜
        [JsonProperty("PIN_GC_DESTRUCTIONDATE")]
        public string PIN_GC_DESTRUCTIONDATE { get; set; } // 파기날짜
        [JsonProperty("PIN_GC_PINNUM1")]
        public string PIN_GC_PINNUM1 { get; set; } // 핀번호1
        [JsonProperty("PIN_GC_PINNUM2")]
        public string PIN_GC_PINNUM2 { get; set; } // 핀번호2
        [JsonProperty("PIN_GC_PINNUM3")]
        public string PIN_GC_PINNUM3 { get; set; } // 핀번호3
        [JsonProperty("PIN_GC_PINNUM4")]
        public string PIN_GC_PINNUM4 { get; set; } // 핀번호4
        [JsonProperty("PIN_GC_CERTIFINUM")]
        public string PIN_GC_CERTIFINUM { get; set; } // 인증번호
        [JsonProperty("PIN_GC_ISUSED")]
        public string PIN_GC_ISUSED { get; set; } // 사용여부(1:사용안함, 2: 사용함 , 3: 사용대기 
        [JsonProperty("PL_NUM")]
        public string PL_NUM { get; set; } // 구매리스트번호
        [JsonProperty("PIN_STATE")]
        public string PIN_STATE { get; set; } // 1: 구매 대기 2:배송/발송 완료 3: 구매실패(시스템에러)
    }
}
