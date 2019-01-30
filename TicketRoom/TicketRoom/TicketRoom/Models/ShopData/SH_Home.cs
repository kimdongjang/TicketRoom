using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.ShopData
{

    public class SH_Home
    {
        [JsonProperty("SH_HOME_INDEX")]
        public int SH_HOME_INDEX { get; set; } // 자기 인덱스
        [JsonProperty("SH_HOME_FREEDELEVERY")]
        public int SH_HOME_FREEDELEVERY { get; set; } // 무료 배송금액
        [JsonProperty("SH_HOME_PAYWAY")]
        public string SH_HOME_PAYWAY { get; set; } // 포인트, 카드 사용 방법
        [JsonProperty("SH_HOME_GRADE")]
        public double SH_HOME_GRADE { get; set; } // 상품 평점
        [JsonProperty("SH_HOME_IMAGE")]
        public string SH_HOME_IMAGE { get; set; } // 메인 이미지
        [JsonProperty("SH_HOME_INSTA")]
        public string SH_HOME_INSTA { get; set; } // 인스타 주소
        [JsonProperty("SH_HOME_WEB")]
        public string SH_HOME_WEB { get; set; } // 웹 주소
        [JsonProperty("SH_HOME_DETAIL")]
        public string SH_HOME_DETAIL { get; set; } // 상품 설명
        [JsonProperty("SH_HOME_INFO")]
        public string SH_HOME_INFO { get; set; } // 사이트 정보
        [JsonProperty("SH_HOME_NAME")]
        public string SH_HOME_NAME { get; set; } // 사이트 이름
        [JsonProperty("SH_HOME_DELEVERY")]
        public int SH_HOME_DELEVERY { get; set; } // 배송비
    }
}
