using Newtonsoft.Json;

namespace TicketRoom.Models.Gift
{
    public class G_DetailCategoryInfo
    {
        [JsonProperty("DETAILCATEGORYNUM")]
        public string DETAILCATEGORYNUM { get; set; } // 카테고리 번호
        [JsonProperty("PRODUCTTYPE")]
        public string PRODUCTTYPE { get; set; }// 카테고리 이름
    }
}
