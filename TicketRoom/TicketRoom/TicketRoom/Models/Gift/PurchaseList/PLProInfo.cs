using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.Gift.PurchaseList
{
    public class PLProInfo
    {
        [JsonProperty("PRODUCTTYPE")]
        public string PRODUCTTYPE { get; set; } // 상품권 종류( 문화상품권, 컬쳐랜드상품권)
        [JsonProperty("PRODUCTVALUE")]
        public string PRODUCTVALUE { get; set; } // 상품권 가격 (1만원권, 5만원권)
        [JsonProperty("PDL_PROTYPE")]
        public string PDL_PROTYPE { get; set; } // 상품권 타입(1: 지류 2:핀번호)
        [JsonProperty("PRODUCTIMAGE")]
        public string PRODUCTIMAGE { get; set; } // 상품 이미지
        [JsonProperty("PDL_PROCOUNT")]
        public string PDL_PROCOUNT { get; set; } // 상품 수량
        [JsonProperty("PDL_PRICE")]
        public string PDL_PRICE { get; set; } // 상품 가격
    }
}
