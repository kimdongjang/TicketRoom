using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketRoom.Models.ShopData
{
    public class SH_Pay_Phone
    {
        [JsonProperty("SH_PAY_PHONE_INDEX")]
        public int SH_PAY_PHONE_INDEX { get; set; } // 휴대폰 결제
        [JsonProperty("SH_PUR_PAY_INDEX")]
        public int SH_PUR_PAY_INDEX { get; set; } // 결제수단 테이블의 인덱스
        [JsonProperty("SH_PAY_PHONE_KINDS")]
        public string SH_PAY_PHONE_KINDS { get; set; } // 통신사 종류

    }
}
