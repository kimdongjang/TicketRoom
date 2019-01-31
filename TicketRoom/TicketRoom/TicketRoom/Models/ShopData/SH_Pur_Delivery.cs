using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketRoom.Models.ShopData
{
    public class SH_Pur_Delivery
    {
        [JsonProperty("SH_PUR_DELIVERY_INDEX")]
        public int SH_PUR_DELIVERY_INDEX { get; set; } // 자신 인덱스
        [JsonProperty("SH_PUR_DELIVERY_PAY")]
        public int SH_PUR_DELIVERY_PAY { get; set; } // 배송비
        [JsonProperty("SH_PUR_DELIVERY_OPTION")]
        public string SH_PUR_DELIVERY_OPTION { get; set; } // 착불 선불 유무
        [JsonProperty("SH_PUR_DELIVERY_DETAIL")]
        public string SH_PUR_DELIVERY_DETAIL { get; set; } // 배송 요청사항
        [JsonProperty("SH_PUR_DELIVERY_ADRESS")]
        public string SH_PUR_DELIVERY_ADRESS { get; set; } // 배송지
        [JsonProperty("SH_PUR_DELIVERY_PHONE")]
        public string SH_PUR_DELIVERY_PHONE { get; set; } // 배송 연락번호
        [JsonProperty("SH_PUR_DELIVERY_STATE")]
        public string SH_PUR_DELIVERY_STATE { get; set; } // 배송 정보
        [JsonProperty("SH_PUR_LIST_ID")]
        public string SH_PUR_LIST_ID { get; set; } // 구매자 아이디
        [JsonProperty("SH_PUR_LIST_INDEX")]
        public int SH_PUR_LIST_INDEX { get; set; } // 주문 번호 인덱스
    }
}
