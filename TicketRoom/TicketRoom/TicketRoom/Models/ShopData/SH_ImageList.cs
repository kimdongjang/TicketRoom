using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.ShopData
{
    public class SH_ImageList
    {
        [JsonProperty("SH_IMAGELIST_INDEX")]
        public int SH_IMAGELIST_INDEX { get; set; } // 자기 인덱스
        [JsonProperty("SH_PRODUCT_INDEX")]
        public int SH_PRODUCT_INDEX { get; set; } // 상품 상세 인덱스
        [JsonProperty("SH_IMAGELIST_SOURCE")]
        public string SH_IMAGELIST_SOURCE { get; set; } // 이미지 리스트 소스
    }
}
