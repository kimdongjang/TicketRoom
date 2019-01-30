
using Newtonsoft.Json;

namespace TicketRoom.Models.Gift.Purchase
{
    public class AccountInfo
    {
        [JsonProperty("AC_NUM")]
        public string AC_NUM { get; set; } // 계좌 테이블 넘버
        [JsonProperty("AC_BANKNAME")]
        public string AC_BANKNAME { get; set; }// 계좌 은행명
        [JsonProperty("AC_NAME")]
        public string AC_NAME { get; set; } // 계좌 주인 이름
        [JsonProperty("AC_ACCOUNTNUM")]
        public string AC_ACCOUNTNUM { get; set; }// 계좌번호
        [JsonProperty("Error")]
        public string Error { get; set; }// 에러메세지
    }
}
