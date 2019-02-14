using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.ShopData
{
    public class SH_Product
    {
        [JsonProperty("SH_PRODUCT_INDEX")]
        public int SH_PRODUCT_INDEX { get; set; } // 자기 인덱스
        [JsonProperty("SH_HOME_INDEX")]
        public int SH_HOME_INDEX { get; set; } // 상품 홈 페이지 인덱스(조회 여부)
        [JsonProperty("SH_PRODUCT_NAME")]
        public string SH_PRODUCT_NAME { get; set; } // 상품 상세 이름(항공점퍼, 와이셔츠)
        [JsonProperty("SH_PRODUCT_PRICE")]
        public int SH_PRODUCT_PRICE { get; set; } // 가격
        [JsonProperty("SH_PRODUCT_ADDOPTION")]
        public string SH_PRODUCT_ADDOPTION { get; set; } // 상품 추가 옵션(널 가능)(가방 추가 15000원)
        [JsonProperty("SH_PRODUCT_CONTENT")]
        public string SH_PRODUCT_CONTENT { get; set; } // 상품 설명
        [JsonProperty("SH_PRODUCT_MAINIMAGE")]
        public string SH_PRODUCT_MAINIMAGE { get; set; } // 상품 메인 이미지
        [JsonProperty("SH_PRODUCT_ISBEST")]
        public string SH_PRODUCT_ISBEST { get; set; } // 상품 베스트 분류 유무
        [JsonProperty("SH_PRODUCT_DETAIL")]
        public string SH_PRODUCT_DETAIL { get; set; } // 상품 구매시 유의사항

    }
}
