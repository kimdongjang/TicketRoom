using Newtonsoft.Json;

namespace TicketRoom.Models.ShopData
{
    public class SubCate
    {
        [JsonProperty("SH_SUBCATE_INDEX")]
        public int SH_SUBCATE_INDEX { get; set; } // 서브 카테고리 인덱스
        [JsonProperty("SH_SUBCATE_NAME")]
        public string SH_SUBCATE_NAME { get; set; }// 서브 카테고리 이름
        [JsonProperty("SH_SUBCATE_GRADE")]
        public double SH_SUBCATE_GRADE { get; set; }// 서브 카테고리 평점
        [JsonProperty("SH_SUBCATE_DETAIL")]
        public string SH_SUBCATE_DETAIL { get; set; }// 서브 카테고리 설명
        [JsonProperty("SH_SUBCATE_IMAGE")]
        public string SH_SUBCATE_IMAGE { get; set; }// 서브 카테고리 이미지 경로
        [JsonProperty("SH_MAINCATE_INDEX")]
        public int SH_MAINCATE_INDEX { get; set; }// 메인 카테고리 인덱스
        [JsonProperty("SH_SUBCATE_ISBEST")]
        public string SH_SUBCATE_ISBEST { get; set; }// 서브 카테고리 이름
        [JsonProperty("SH_HOME_INDEX")]
        public int SH_HOME_INDEX { get; set; }//홈 페이지 인덱스
    }
}
