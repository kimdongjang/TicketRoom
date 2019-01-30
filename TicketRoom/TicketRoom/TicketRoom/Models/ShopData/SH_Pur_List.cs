using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketRoom.Models.ShopData
{
    public class SH_Pur_List
    {
        [JsonProperty("SH_PUR_LIST_INDEX")]
        public int SH_PUR_LIST_INDEX { get; set; } // 자신 인덱스
        [JsonProperty("SH_PUR_PRODUCT_INDEX")]
        public int SH_PUR_PRODUCT_INDEX { get; set; } // 구매 상품 정보
        [JsonProperty("SH_PUR_DELIVERY_INDEX")]
        public int SH_PUR_DELIVERY_INDEX { get; set; } // 배송 정보
        [JsonProperty("SH_PUR_PAY_INDEX")]
        public int SH_PUR_PAY_INDEX { get; set; } // 결제 정보
        [JsonProperty("SH_PUR_LIST_ID")]
        public string SH_PUR_LIST_ID { get; set; } // 구매자 아이디
        [JsonProperty("SH_PUR_LIST_DATE")]
        public string SH_PUR_LIST_DATE { get; set; } // 구매 날짜
    }
}
