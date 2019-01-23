using System.Collections.Generic;

namespace TicketRoom.Models.USERS
{
    public class USERSData
    {
        public string ID { get; set; }
        public string PW { get; set; }
        public string Email { get; set; }
        public Dictionary<string, bool> Termsdata { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string roadAddr { get; set; }
        public string jibunAddr { get; set; }
        public string zipNo { get; set; }
        public string RecommenderID { get; set; }

        public USERSData(string id, string pw, string email, string roadAddr, string jibunAddr, string zipNo, Dictionary<string, bool> termsdata, string recommenderid)
        {
            this.ID = id;
            this.PW = pw;
            this.Email = email;
            this.Termsdata = termsdata;
            this.roadAddr = roadAddr;
            this.jibunAddr = jibunAddr;
            this.zipNo = zipNo;
            this.RecommenderID = recommenderid;
        }

        public override string ToString()
        {
            return "ID: " + ID + " PW: " + PW + " EMail: " + Email + "Name: " + Name + "Phone: " + Phone + "Adress: " + Termsdata.ToString();
        }
    }
}
