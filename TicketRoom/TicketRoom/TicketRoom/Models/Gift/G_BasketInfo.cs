﻿using Newtonsoft.Json;

namespace TicketRoom.Models.Gift
{
    public class G_BasketInfo
    {
        [JsonProperty("BK_PRONUM")]
        public string BK_PRONUM { get; set; } // 장바구니 상품 번호
        [JsonProperty("BK_PROCOUNT")]
        public string BK_PROCOUNT { get; set; }// 장바구니 수량
        [JsonProperty("BK_TYPE")]
        public string BK_TYPE { get; set; } // 장바구니 상품 타입 (1:지류 2:핀번호)
        [JsonProperty("BK_PRODUCT_IMAGE")]
        public string BK_PRODUCT_IMAGE { get; set; } // 장바구니 상품 타입 (1:지류 2:핀번호)
        [JsonProperty("BK_PRODUCT_TYPE")]
        public string BK_PRODUCT_TYPE { get; set; } // 장바구니 상품 타입 (1:지류 2:핀번호)
        [JsonProperty("BK_PRODUCT_VALUE")]
        public string BK_PRODUCT_VALUE { get; set; } // 장바구니 상품 타입 (1:지류 2:핀번호)
        [JsonProperty("BK_PRODUCT_PURCHASE_DISCOUNTPRICE")]
        public string BK_PRODUCT_PURCHASE_DISCOUNTPRICE { get; set; } // 장바구니 상품 타입 (1:지류 2:핀번호)
    }
}
