using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketRoom.Models.ShopData
{
    public class SH_Pay_Business
    {
        [JsonProperty("SH_PAY_BUSINESS_INDEX")]
        public int SH_PAY_BUSINESS_INDEX { get; set; } // 현금영수증 사업자
        [JsonProperty("SH_PUR_PAY_INDEX")]
        public int SH_PUR_PAY_INDEX { get; set; } // 결제수단 테이블의 인덱스
        [JsonProperty("SH_PAY_BUSINESS_NUM")]
        public string SH_PAY_BUSINESS_NUM { get; set; } // 사업자 휴대폰
        [JsonProperty("SH_PAY_BUSINESS_NAME")]
        public string SH_PAY_BUSINESS_NAME { get; set; } // 사업자 이름
    }
}
