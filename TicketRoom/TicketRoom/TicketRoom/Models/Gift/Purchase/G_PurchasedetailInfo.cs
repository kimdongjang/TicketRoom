using Newtonsoft.Json;

namespace TicketRoom.Models.Gift.Purchase
{
    public class G_PurchasedetailInfo
    {
        //-- 구매 테이블 Insert용
        [JsonProperty("PDL_PRONUM")]
        public string PDL_PRONUM { get; set; } // 상품번호
        [JsonProperty("PDL_PROCOUNT")]
        public string PDL_PROCOUNT { get; set; }// 상품수량
        [JsonProperty("PDL_PROTYPE")]
        public string PDL_PROTYPE { get; set; } // 상품타입(1:지류 , 2:핀번호)
        [JsonProperty("PDL_ALLPRICE")]
        public string PDL_ALLPRICE { get; set; } // 하나상품 총 금액(상품 가격 * 수량)

        //-- 구매 목록 리스트 출력용
        [JsonProperty("PRODUCT_IMAGE")]
        public string PRODUCT_IMAGE { get; set; } // 상품이미지
        [JsonProperty("PRODUCT_TYPE")]
        public string PRODUCT_TYPE { get; set; } // 상품권 종류 ( 문화상품권 , 컬쳐 등등)
        [JsonProperty("PRODUCT_VALUE")]
        public string PRODUCT_VALUE { get; set; } // 상품권 가격( 1만원 , 3만원 등)
    }
}
