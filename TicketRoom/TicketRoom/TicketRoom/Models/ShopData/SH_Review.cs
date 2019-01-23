using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.ShopData
{
    public class SH_Review
    {
        [JsonProperty("SH_REVIEW_INDEX")]
        public int SH_REVIEW_INDEX { get; set; } // 자기 인덱스
        [JsonProperty("SH_HOME_INDEX")]
        public int SH_HOME_INDEX { get; set; } // 상품 홈 페이지 인덱스
        [JsonProperty("SH_REVIEW_GRADE")]
        public int SH_REVIEW_GRADE { get; set; } // 리뷰 평점
        [JsonProperty("SH_REVIEW_ID")]
        public string SH_REVIEW_ID { get; set; } // 작성자 아이디
        [JsonProperty("SH_REVIEW_CONTENT")]
        public string SH_REVIEW_CONTENT { get; set; }// 리뷰 내용
    }
}
