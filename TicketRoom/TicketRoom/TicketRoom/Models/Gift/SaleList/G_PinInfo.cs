using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models.Gift.SaleList
{
    public class G_PinInfo
    {
        [JsonProperty("SDL_NUM")]
        public string SDL_NUM { get; set; } // 판매상세내역(핀번호) 테이블 번호
        [JsonProperty("SDL_SL_NUM")]
        public string SDL_SL_NUM { get; set; } // 판매테이블번호
        [JsonProperty("SDL_PIN1")]
        public string SDL_PIN1 { get; set; } // 핀번호 첫덩어리
        [JsonProperty("SDL_PIN2")]
        public string SDL_PIN2 { get; set; } // 핀번호 두번째덩어리
        [JsonProperty("SDL_PIN3")]
        public string SDL_PIN3 { get; set; } // 핀번호 세번째 덩어리
        [JsonProperty("SDL_PIN4")]
        public string SDL_PIN4 { get; set; } // "핀번호 네번째  덩어리
        [JsonProperty("SDL_CERTIFICATIONNUM")]
        public string SDL_CERTIFICATIONNUM { get; set; } // 발행일 및 인증번호
        [JsonProperty("SDL_ISUSED")]
        public string SDL_ISUSED { get; set; } // 사용가능여부(1:사용 가능 , 2: 불가능)
    }
}
