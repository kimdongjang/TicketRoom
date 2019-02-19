using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.Gift.SaleList
{
    public class G_SaleInfo
    {
        [JsonProperty("SL_NUM")]
        public string SL_NUM { get; set; } // 판매 번호
        [JsonProperty("SL_USERID")]
        public string SL_USERID { get; set; } // 판매자 ID
        [JsonProperty("SL_SALE_DATE")]
        public string SL_SALE_DATE { get; set; } // 판매날짜
        [JsonProperty("SL_ISSUCCES")]
        public string SL_ISSUCCES { get; set; } // 성공여부(1:판매완료, 2: 판매실패: 판매대기중
        [JsonProperty("SL_FAILSTRING")]
        public string SL_FAILSTRING { get; set; } // 실패사유
        [JsonProperty("SL_TOTAL_PRICE")]
        public string SL_TOTAL_PRICE { get; set; } // 총 판매 가격
        [JsonProperty("SL_ACC_NAME")]
        public string SL_ACC_NAME { get; set; } // 예금자명
        [JsonProperty("SL_ACC_NUM")]
        public string SL_ACC_NUM { get; set; } // 계좌번호
        [JsonProperty("SL_SALEPRO_TYPE")]
        public string SL_SALEPRO_TYPE { get; set; } // 판매상품타입(1: 지류 , 2핀번호
        [JsonProperty("SL_SEND_DATE")]
        public string SL_SEND_DATE { get; set; } // 발송날짜
        [JsonProperty("SL_SENDSTRING")]
        public string SL_SENDSTRING { get; set; } // 전달사항
        [JsonProperty("SL_SALE_PW")]
        public string SL_SALE_PW { get; set; } // 접수비밀번호
        [JsonProperty("SL_PIN_LIST")]
        public List<G_PinInfo> SL_PIN_LIST { get; set; } // 핀번호 리스트
        [JsonProperty("SL_PRONUM")]
        public string SL_PRONUM { get; set; } // 판매상품 번호
        [JsonProperty("SL_PROCOUNT")]
        public string SL_PROCOUNT { get; set; } // 판매수량
        [JsonProperty("SL_BANK_NAME")]
        public string SL_BANK_NAME { get; set; } // 은행명
    }
}
