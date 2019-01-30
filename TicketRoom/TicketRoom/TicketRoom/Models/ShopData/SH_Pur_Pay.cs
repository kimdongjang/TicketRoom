using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketRoom.Models.ShopData
{
    public class SH_Pur_Pay
    {
        [JsonProperty("SH_PUR_PAY_INDEX")]
        public int SH_PUR_PAY_INDEX { get; set; } // 자신 인덱스
        [JsonProperty("SH_PUR_PAY_OPTION")]
        public string SH_PUR_PAY_OPTION { get; set; } // 결제방식(1:카드결제2:현금결제(개인소득공제)3:현금결제(사업자)4:휴대폰)
        [JsonProperty("SH_PUR_PAY_VALUE")]
        public int SH_PUR_PAY_VALUE { get; set; } // 결제금액
        [JsonProperty("SH_PUR_PAY_USEPOINT")]
        public int SH_PUR_PAY_USEPOINT { get; set; } // 사용포인트
        [JsonProperty("SH_PUR_LIST_ID")]
        public string SH_PUR_LIST_ID { get; set; } // 구매자 아이디
        [JsonProperty("SH_PUR_LIST_INDEX")]
        public int SH_PUR_LIST_INDEX { get; set; } // 주문 번호 인덱스
    }
}
