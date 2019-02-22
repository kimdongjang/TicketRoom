using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.Gift
{
    public class G_DealInfo
    {
        [JsonProperty("NUM")]
        public string NUM { get; set; } // 거래 번호
        [JsonProperty("TOTALDATE")]
        public string TOTALDATE { get; set; } // 거래 날짜
        [JsonProperty("ISCHECK")]
        public string ISCHECK { get; set; } // 거래 종류(1:구매 ,2:판매)
        [JsonProperty("NAME")]
        public string NAME { get; set; } // 거래자 이름
        [JsonProperty("TITLE")]
        public string TITLE { get; set; } // 거래제목
        [JsonProperty("PROTYPE_COUNT")]
        public string PROTYPE_COUNT { get; set; } // 상품 종류 갯수
    }
}
