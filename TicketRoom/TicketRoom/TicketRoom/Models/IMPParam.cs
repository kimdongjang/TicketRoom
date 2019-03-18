using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models
{
    public class IMPParam
    {
        public string pg; // 이니시스 기본
        public string pay_method; // 결제방법
        public string merchant_uid; // 구매시각
        public string name; // 주문한 상품이름
        public int amount; // 결제가격
        public string buyer_email; // 구매자 이메일
        public string buyer_name; // 구매자 이름
        public string buyer_tel; // 구매자 전화 번호
        public string buyer_addr; // 구매자 주소
        public string buyer_postcode; // 구매자 우편번호
    }
}
