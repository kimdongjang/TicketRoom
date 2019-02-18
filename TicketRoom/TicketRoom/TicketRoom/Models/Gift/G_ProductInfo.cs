using Newtonsoft.Json;

namespace TicketRoom.Models.Gift
{
    public class G_ProductInfo
    {
        [JsonProperty("PRONUM")]
        public string PRONUM { get; set; } // 상품 번호
        [JsonProperty("DETAILCATEGORYNUM")]
        public string DETAILCATEGORYNUM { get; set; } // 세부카테고리 테이블 번호
        [JsonProperty("PRODUCTTYPE")]
        public string PRODUCTTYPE { get; set; }// 상품 세부카테고리 이름
        [JsonProperty("PRODUCTIMAGE")]
        public string PRODUCTIMAGE { get; set; }// 상품 세부카테고리 이미지
        [JsonProperty("PRODUCTVALUE")]
        public string PRODUCTVALUE { get; set; } // 상품 가격 이름
        [JsonProperty("PROPRICE")]
        public string PROPRICE { get; set; }// 상품 가격
        [JsonProperty("PROCOUNT")]
        public string PROCOUNT { get; set; }// 상품 수량
        [JsonProperty("PURCHASEDISCOUNTRATE")]
        public string PURCHASEDISCOUNTRATE { get; set; } // 상품 구매 할인율
        [JsonProperty("PURCHASEDISCOUNTPRICE")]
        public string PURCHASEDISCOUNTPRICE { get; set; }// 상품 구매 할인 후 가격
        [JsonProperty("SALEDISCOUNTRATE")]
        public string SALEDISCOUNTRATE { get; set; } // 상품 판매 할인율
        [JsonProperty("SALEDISCOUNTPRICE")]
        public string SALEDISCOUNTPRICE { get; set; }// 상품 판매 할인 후 가격
    }
}
