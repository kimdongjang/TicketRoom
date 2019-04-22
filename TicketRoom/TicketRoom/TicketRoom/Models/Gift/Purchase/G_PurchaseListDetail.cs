using Newtonsoft.Json;

namespace TicketRoom.Models.Gift.Purchase
{
    public class G_PurchaseListDetail
    {
        //-- 구매 테이블 Insert용
        [JsonProperty("PDL_NUM")]
        public string PDL_NUM { get; set; } // 구매상세내역 번호
        [JsonProperty("PDL_PRONUM")]
        public string PDL_PRONUM { get; set; } // 상품번호
        [JsonProperty("PDL_PROTYPE")]
        public string PDL_PROTYPE { get; set; } // 상품타입(1:지류 , 2:핀번호)
        [JsonProperty("PDL_PLNUM")]
        public string PDL_PLNUM { get; set; } // 구매번호
        [JsonProperty("PDL_PRICE")]
        public string PDL_PRICE { get; set; } // 상품 금액
        [JsonProperty("PDL_ISAVAILABLE")]
        public string PDL_ISAVAILABLE { get; set; } // (1: 수량있음 2: 수량부족)
        [JsonProperty("PDL_PINNUM")]
        public string PDL_PINNUM { get; set; } // 핀번호 인덱스
        [JsonProperty("PDL_PAPERNUM")]
        public string PDL_PAPERNUM { get; set; } // 상품권 인덱스
        [JsonProperty("PDL_PIN_STATE")]
        public string PDL_PIN_STATE { get; set; } // 핀번호 상태값  1: 구매 대기 2:배송/발송 완료 3: 구매실패(시스템에러)

    }
}
