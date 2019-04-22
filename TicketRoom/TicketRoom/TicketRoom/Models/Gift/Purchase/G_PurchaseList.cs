using Newtonsoft.Json;
using System.Collections.Generic;

namespace TicketRoom.Models.Gift.Purchase
{
    public class G_PurchaseList
    {
        [JsonProperty("PL_NUM")]
        public int PL_NUM { get; set; } // 구매번호
        [JsonProperty("ID")]
        public string ID { get; set; } // 구매자 ID
        [JsonProperty("PL_DELIVERY_ADDRESS")]
        public string PL_DELIVERY_ADDRESS { get; set; } // 배송지 도로명 주소
        [JsonProperty("PL_PURCHASE_DATE")]
        public string PL_PURCHASE_DATE { get; set; } // 구매날짜
        [JsonProperty("PL_USED_POINT")]
        public string PL_USED_POINT { get; set; } // 사용 포인트
        [JsonProperty("PL_ISSUCCESS")]
        public string PL_ISSUCCESS { get; set; } // 성공여부
                                                 //(1: 구매대기(입금대기) 2: 구매중(배송/발송대기, 발송중) 3: 구매실패(입금시간초과) 4: 구매완료 5: 구매실패(수량부족) 6: 시스템에러)
        [JsonProperty("PL_PAYMENT_PRICE")]
        public string PL_PAYMENT_PRICE { get; set; } // 총 결제금액
        [JsonProperty("PL_DELIVERYPAY_TYPE")]
        public string PL_DELIVERYPAY_TYPE { get; set; } // 배송타입(1: 선불 2: 착불)
        [JsonProperty("AC_NUM")]
        public string AC_NUM { get; set; } // 계좌번호
        [JsonProperty("PL_ACCUSER_NAME")]
        public string PL_ACCUSER_NAME { get; set; } // 입금예정자이름
        [JsonProperty("PL_DV_NAME")]
        public string PL_DV_NAME { get; set; } // 배송받는사람이름
        [JsonProperty("PL_DV_PHONE")]
        public string PL_DV_PHONE { get; set; } // 배송시 연락처
        [JsonProperty("PL_DELIVERY_JIBUNADDR")]
        public string PL_DELIVERY_JIBUNADDR { get; set; } // 배송지 지번 주소
        [JsonProperty("PL_DELIVERY_ZIPNO")]
        public string PL_DELIVERY_ZIPNO { get; set; } // 배송지 우편번호
        [JsonProperty("PL_PAPERSTATE")]
        public string PL_PAPERSTATE { get; set; } // 지류 배송상태
                                                  //(30: 구매 대기 31: 배송/발송중 32:배송/발송 완료 33: 구매실패(반송) 34: 구매실패(수량부족) 35: 구매실패(시스템에러) 37: 교환 38: 환불
        [JsonProperty("PL_PAPER_DVNUM")]
        public string PL_PAPER_DVNUM { get; set; } // 등기번호
        [JsonProperty("PL_PAPER_COUNT")]
        public string PL_PAPER_COUNT { get; set; } // 지류 구매 수량

        // =====================================================
        [JsonProperty("G_TempProduct")]
        public List<G_TempBasketProduct> G_TempProduct { get; set; }
        [JsonProperty("ISUSER")]
        public string ISUSER { get; set; }
    }
}
