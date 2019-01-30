using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketRoom.Models.ShopData
{
    public class SH_Pur_Product
    {
        [JsonProperty("SH_PUR_PRODUCT_INDEX")]
        public int SH_PUR_PRODUCT_INDEX { get; set; } // 자신 인덱스
        [JsonProperty("SH_HOME_INDEX")]
        public int SH_HOME_INDEX { get; set; } // 홈 페이지 인덱스
        [JsonProperty("SH_PUR_PRODUCT_IMAGE")]
        public string SH_PUR_PRODUCT_IMAGE { get; set; } // 이미지 경로
        [JsonProperty("SH_PUR_PRODUCT_COUNT")]
        public int SH_PUR_PRODUCT_COUNT { get; set; } // 구매 수량
        [JsonProperty("SH_PUR_PRODUCT_COLOR")]
        public string SH_PUR_PRODUCT_COLOR { get; set; } // 구매 색상
        [JsonProperty("SH_PUR_PRODUCT_SIZE")]
        public string SH_PUR_PRODUCT_SIZE { get; set; } // 구매 사이즈
        [JsonProperty("SH_PUR_PRODUCT_NAME")]
        public string SH_PUR_PRODUCT_NAME { get; set; } // 상품 이름
        [JsonProperty("SH_PUR_LIST_ID")]
        public string SH_PUR_LIST_ID { get; set; } // 구매자 아이디
        [JsonProperty("SH_PUR_LIST_INDEX")]
        public int SH_PUR_LIST_INDEX { get; set; } // 주문 번호 인덱스
    }
}
