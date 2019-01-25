using Newtonsoft.Json;

namespace TicketRoom.Models.Gift.Purchase
{
    public class G_PurchasedetailInfo
    {
        [JsonProperty("PDL_PRONUM")]
        public string PDL_PRONUM { get; set; } // 상품번호
        [JsonProperty("PDL_PROCOUNT")]
        public string PDL_PROCOUNT { get; set; }// 상품수량
        [JsonProperty("PDL_PROTYPE")]
        public string PDL_PROTYPE { get; set; } // 상품타입(1:지류 , 2:핀번호)
        [JsonProperty("PDL_ALLPRICE")]
        public string PDL_ALLPRICE { get; set; } // 하나상품 총 금액(상품 가격 * 수량)
    }
}
