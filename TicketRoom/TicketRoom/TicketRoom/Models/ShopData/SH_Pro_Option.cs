using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.ShopData
{
    public class SH_Pro_Option
    {
        [JsonProperty("SH_PRO_OPTION_INDEX")]
        public int SH_PRO_OPTION_INDEX { get; set; } // 자기 인덱스
        [JsonProperty("SH_PRODUCT_INDEX")]
        public int SH_PRODUCT_INDEX { get; set; } // 상품 상세 옵션의 인덱스
        [JsonProperty("SH_PRO_OPTION_COUNT")]
        public int SH_PRO_OPTION_COUNT { get; set; } // 상품 상세 옵션 중 수량
        [JsonProperty("SH_PRO_OPTION_SIZE")]
        public string SH_PRO_OPTION_SIZE { get; set; } // 상품 상세 옵션 중 사이즈
        [JsonProperty("SH_PRO_OPTION_COLOR")]
        public string SH_PRO_OPTION_COLOR { get; set; }// 상품 상세 옵션 중 색상
    }
}
