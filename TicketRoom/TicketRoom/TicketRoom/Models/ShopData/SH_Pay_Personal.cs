using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketRoom.Models.ShopData
{
    public class SH_Pay_Personal
    {
        [JsonProperty("SH_PAY_PERSONAL_INDEX")]
        public int SH_PAY_PERSONAL_INDEX { get; set; } // 현금영수증 개인 
        [JsonProperty("SH_PUR_PAY_INDEX")]
        public int SH_PUR_PAY_INDEX { get; set; } // 결제수단 테이블의 인덱스
        [JsonProperty("SH_PAY_PERSONAL_NUM")]
        public string SH_PAY_PERSONAL_NUM { get; set; } // 개인휴대폰번호
        [JsonProperty("SH_PAY_PERSONAL_NAME")]
        public string SH_PAY_PERSONAL_NAME { get; set; } // 개인소득공제
    }
}
