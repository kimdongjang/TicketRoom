using Newtonsoft.Json;

namespace TicketRoom.Models.Gift
{
    public class G_BasketInfo
    {
        [JsonProperty("BASKETLISTTABLE_NUM")]
        public string BASKETLISTTABLE_NUM { get; set; } // 장바구니 인덱스
        [JsonProperty("BK_PRONUM")]
        public string BK_PRONUM { get; set; } // 장바구니 상품 번호
        [JsonProperty("BK_PROCOUNT")]
        public string BK_PROCOUNT { get; set; }// 장바구니 수량
        [JsonProperty("BK_TYPE")]
        public string BK_TYPE { get; set; } // 장바구니 상품 타입 (1:지류 2:핀번호)
        [JsonProperty("BK_PRODUCT_IMAGE")]
        public string BK_PRODUCT_IMAGE { get; set; } // 장바구니 상품이미지
        [JsonProperty("BK_PRODUCT_TYPE")]
        public string BK_PRODUCT_TYPE { get; set; } // 장바구니 상품권 종류 ( 문화상품권 , 컬쳐 등등)
        [JsonProperty("BK_PRODUCT_VALUE")]
        public string BK_PRODUCT_VALUE { get; set; } // 장바구니 상품권 가격( 1만원 , 3만원 등)
        [JsonProperty("BK_PRODUCT_PURCHASE_DISCOUNTPRICE")]
        public string BK_PRODUCT_PURCHASE_DISCOUNTPRICE { get; set; } // 장바구니 상품 할인후 가격
    }
}
