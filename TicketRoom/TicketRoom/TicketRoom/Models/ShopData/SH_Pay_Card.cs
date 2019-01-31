using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketRoom.Models.ShopData
{
    public class SH_Pay_Card
    {
        [JsonProperty("SH_PAY_CARD_INDEX")]
        public int SH_PAY_CARD_INDEX { get; set; } // 자신 인덱스
        [JsonProperty("SH_PUR_PAY_INDEX")]
        public int SH_PUR_PAY_INDEX { get; set; } // 결제수단 테이블의 인덱스
        [JsonProperty("SH_PAY_CARD_KINDS")]
        public string SH_PAY_CARD_KINDS { get; set; } // 결제카드 종류
    }
}
