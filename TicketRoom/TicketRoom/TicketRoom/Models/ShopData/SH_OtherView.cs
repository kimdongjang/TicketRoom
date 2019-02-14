using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.ShopData
{
    public class SH_OtherView
    {
        [JsonProperty("SH_OTHERVIEW_INDEX")]
        public int SH_OTHERVIEW_INDEX { get; set; } // 자기 인덱스
        [JsonProperty("SH_OTHERVIEW_IMAGE")]
        public string SH_OTHERVIEW_IMAGE { get; set; } // 이미지 소스
        [JsonProperty("SH_HOME_INDEX")]
        public int SH_HOME_INDEX { get; set; } // 상품 홈 페이지의 인덱스
        [JsonProperty("SH_OTHERHOME_INDEX")]
        public int SH_OTHERHOME_INDEX { get; set; } // 다른 고객이 본 상품의 인덱스
    }
}
