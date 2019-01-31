using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.ShopData
{
    public class SH_BasketList
    {
        [JsonProperty("SH_BASKET_INDEX")]
        public int SH_BASKET_INDEX { get; set; } // 자신 인덱스
        [JsonProperty("SH_HOME_INDEX")]
        public int SH_HOME_INDEX { get; set; } // 홈 페이지 인덱스
        [JsonProperty("SH_BASKET_PRICE")]
        public int SH_BASKET_PRICE { get; set; } // 장바구니에 저장된 가격
        [JsonProperty("SH_BASKET_COUNT")]
        public int SH_BASKET_COUNT { get; set; } // 장바구니에 저장된 수량
        [JsonProperty("SH_BASKET_COLOR")]
        public string SH_BASKET_COLOR { get; set; } // 장바구니에 저장된 색상
        [JsonProperty("SH_BASKET_SIZE")]
        public string SH_BASKET_SIZE { get; set; } //장바구니에 저장된 사이즈
        [JsonProperty("SH_BASKET_ID")]
        public string SH_BASKET_ID { get; set; } // 장바구니에 저장된 사용자 아이디
        [JsonProperty("SH_BASKET_NAME")]
        public string SH_BASKET_NAME { get; set; } // 장바구니에 저장된 상품이름
        [JsonProperty("SH_BASKET_DATE")]
        public string SH_BASKET_DATE { get; set; } // 장바구니에 저장된 날짜
        [JsonProperty("SH_BASKET_IMAGE")]
        public string SH_BASKET_IMAGE { get; set; } //장바구니에 저장된 이미지
    }
}
