using Newtonsoft.Json;
using System.Collections.Generic;

namespace TicketRoom.Models.Gift.Purchase
{
    class G_PurchaseInfo
    {
        [JsonProperty("ID")]
        public string ID { get; set; } // 구매자 ID
        [JsonProperty("PL_DELIVERY_ADDRESS")]
        public string PL_DELIVERY_ADDRESS { get; set; }// 배송지 주소
        [JsonProperty("PL_USED_POINT")]
        public string PL_USED_POINT { get; set; } // 사용 포인트
        [JsonProperty("PL_ISSUCCESS")]
        public string PL_ISSUCCESS { get; set; } // 성공여부(1:성공 ,2:실패 , 3:구매진행중)
        [JsonProperty("PL_PAYMENT_PRICE")]
        public string PL_PAYMENT_PRICE { get; set; } // 총 결제금액
        [JsonProperty("PL_DELIVERYPAY_TYPE")]
        public string PL_DELIVERYPAY_TYPE { get; set; } // 배송비 타입(1:선불, 2:착불)
        [JsonProperty("PL_ACCUSER_NAME")]
        public string PL_ACCUSER_NAME { get; set; } // 입금예정자 이름
        [JsonProperty("AC_NUM")]
        public string AC_NUM { get; set; } // 입금계좌번호
        [JsonProperty("G_PD_LIST")]
        public List<G_PurchasedetailInfo> G_PD_LIST { get; set; } // 입금계좌번호
        [JsonProperty("PL_DV_NAME")]
        public string PL_DV_NAME { get; set; } // 배송받을 사람 이름
        [JsonProperty("PL_DV_PHONE")]
        public string PL_DV_PHONE { get; set; } // 배송받을 사람 전화번호
    }
}
