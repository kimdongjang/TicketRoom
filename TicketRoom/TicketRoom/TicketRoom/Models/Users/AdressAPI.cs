using Newtonsoft.Json;

namespace TicketRoom.Models.USERS
{
    public class AdressAPI
    {
        [JsonProperty("roadAddr")]
        public string roadAddr { get; set; } // 전체도로명 주소
        [JsonProperty("roadAddrPart1")]
        public string roadAddrPart1 { get; set; }// 도로명주소(참고항목 제외)
        [JsonProperty("roadAddrPart2")]
        public string roadAddrPart2 { get; set; }// 도로명주소 참고항목
        [JsonProperty("jibunAddr")]
        public string jibunAddr { get; set; } // 지번 정보
        [JsonProperty("engAddr")]
        public string engAddr { get; set; } // 도로명주소(영문)
        [JsonProperty("zipNo")]
        public string zipNo { get; set; } // 우편번호
        [JsonProperty("admCd")]
        public string admCd { get; set; } // 행정구역코드
        [JsonProperty("rnMgtSn")]
        public string rnMgtSn { get; set; } // 도로명코드
        [JsonProperty("bdMgtSn")]
        public string bdMgtSn { get; set; } // 건물관리번호
        [JsonProperty("detBdNmList")]
        public string detBdNmList { get; set; } // 상세건물명
        [JsonProperty("bdNm")]
        public string bdNm { get; set; } // 건물명
        [JsonProperty("bdKdcd")]
        public string bdKdcd { get; set; } // 공동주택여부 ( 1:  공동주택, 0: 비공동주택)
        [JsonProperty("siNm")]
        public string siNm { get; set; } // 시도명
        [JsonProperty("sggNm")]
        public string sggNm { get; set; } // 시군구명
        [JsonProperty("emdNm")]
        public string emdNm { get; set; } // 읍면동명
        [JsonProperty("liNm")]
        public string liNm { get; set; } // 법정리명
        [JsonProperty("rn")]
        public string rn { get; set; }// 도로명
        [JsonProperty("udrtYn")]
        public string udrtYn { get; set; }// 지하여부(0:지상, 1:지하)
        [JsonProperty("buldMnnm")]
        public string buldMnnm { get; set; } // 건물본번
        [JsonProperty("buldSlno")]
        public string buldSlno { get; set; } // 건물부번(부번이 없는 경우 0)
        [JsonProperty("mtYn")]
        public string mtYn { get; set; } // 산여부(0:대지, 1:산)
        [JsonProperty("lnbrMnnm")]
        public string lnbrMnnm { get; set; } // 지번본번(번지)
        [JsonProperty("lnbrSlno")]
        public string lnbrSlno { get; set; } // 지번본번(호) (부번이 없는 경우 0)
        [JsonProperty("emdNo")]
        public string emdNo { get; set; }  // 읍면동일련번호
    }
}
