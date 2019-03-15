using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketRoom.Models
{
    public class IMP_RValue
    {
        [JsonProperty("SH_CHARGE_INDEX")]
        public string SH_CHARGE_INDEX;
        [JsonProperty("SH_IMP_UID")]
        public string SH_IMP_UID;
        [JsonProperty("SH_MERCHANT_UID")]
        public string SH_MERCHANT_UID;
        [JsonProperty("SH_PAY_METHOD")]
        public string SH_PAY_METHOD;
        [JsonProperty("SH_BANK_NAME")]
        public string SH_BANK_NAME;
        [JsonProperty("SH_CARD_NAME")]
        public string SH_CARD_NAME;
        [JsonProperty("SH_NAME")]
        public string SH_NAME;
        [JsonProperty("SH_AMOUNT")]
        public string SH_AMOUNT;
        [JsonProperty("SH_BUYER_NAME")]
        public string SH_BUYER_NAME;
        [JsonProperty("SH_BUYER_EMAIL")]
        public string SH_BUYER_EMAIL;
        [JsonProperty("SH_BUYER_TEL")]
        public string SH_BUYER_TEL;
        [JsonProperty("SH_BUYER_ADDR")]
        public string SH_BUYER_ADDR;
        [JsonProperty("SH_BUYER_POSTCODE")]
        public string SH_BUYER_POSTCODE;
        [JsonProperty("SH_STATUS")]
        public string SH_STATUS;
        [JsonProperty("SH_DATE")]
        public string SH_DATE;
        [JsonProperty("SH_POINT_INDEX")]
        public string SH_POINT_INDEX;
    }
}
