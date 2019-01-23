using Newtonsoft.Json;

namespace TicketRoom.Models.Gift
{
    public class G_ProductCount
    {
        [JsonProperty("PRONUM")]
        public string PRONUM { get; set; } // 상품 번호
        [JsonProperty("PAPER_GC_COUNT")]
        public string PAPER_GC_COUNT { get; set; }// 지류 상품권 수량
        [JsonProperty("PIN_GC_COUNT")]
        public string PIN_GC_COUNT { get; set; }// 핀번호 상품권 수량
    }
}
