using Newtonsoft.Json;

namespace TicketRoom.Models.ShopData
{
    public class MainCate
    {
        [JsonProperty("SH_MAINCATE_INDEX")]
        public int SH_MAINCATE_INDEX { get; set; } // 메인 카테고리 인덱스
        [JsonProperty("SH_MAINCATE_NAME")]
        public string SH_MAINCATE_NAME { get; set; }// 메인 카테고리 이름
    }
}
