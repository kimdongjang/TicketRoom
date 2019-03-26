using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.Gift.PurchaseList
{
    public class G_PLInfo
    {
        [JsonProperty("PL_NUM")]
        public int PL_NUM { get; set; } // 구매번호
        [JsonProperty("PL_PURCHASE_DATE")]
        public string PL_PURCHASE_DATE { get; set; } // 구매날짜
        [JsonProperty("PL_ISSUCCESS")]
        public string PL_ISSUCCESS { get; set; } // 구매 성공여부(1:구매완료 ,2:수량부족 , 3:구매진행중-입금대기)
        [JsonProperty("PL_DELIVERYPAY_TYPE")]
        public string PL_DELIVERYPAY_TYPE { get; set; } // 배송타입(1: 선불 2: 착불)
        [JsonProperty("PL_DELIVERY_ADDRESS")]
        public string PL_DELIVERY_ADDRESS { get; set; } // 배송지
        [JsonProperty("AC_NUM")]
        public string AC_NUM { get; set; } // 계좌번호
        [JsonProperty("PL_DV_PHONE")]
        public string PL_DV_PHONE { get; set; } // 수취인 핸드폰 
        [JsonProperty("PL_PAYMENT_PRICE")]
        public string PL_PAYMENT_PRICE { get; set; } // 총 결제금액
        [JsonProperty("PL_USED_POINT")]
        public string PL_USED_POINT { get; set; } // 사용 포인트
    }
}
