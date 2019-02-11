namespace TicketRoom.Models.ShopData
{
    public class ShopDataFunc
    {
        string info = "";
        int shop_home_best_cnt = 5;
        int shop_home_nature_cnt = 5;
        int shop_home_review = 5;
        public ShopDataFunc() { }
        public string GetShopDetailData(string titleName)
        {

            // DB연결 후 쇼핑몰 정보 가져옴
            string s = "설명 : " +
                    "asdasdwmdjvxckvxkdfmksdlfmkjsldfkxcz" +
                    "daksdmjknvkjxcnvbkjdxcmgkjdfsnghjisdljfgknesrg" +
                    "sdlkfgnxdjikgbkjfdghm klfgh";
            return s;
        }
        public string GetShopInfoData(string titleName)
        {
            // DB연결 후 쇼핑몰 정보 가져옴
            string s = "쇼핑몰 사이트 주소 : http://asda2mkfd.cod/" +
                    "asdasdwmdjvxckvxkdfmksdlfmkjsldfkxcz" +
                    "daksdmjknvkjxcnvbkjdxcmgkjdfsnghjisdljfgknesrg" +
                    "sdlkfgnxdjikgbkjfdghm klfgh";
            return s;
        }
        public int GetShopHomeBestCnt(string titleName)
        {
            // DB연결 후 쇼핑몰 홈의 베스트 정보 가져옴
            return shop_home_best_cnt;
        }

        public int GetShopHomeNatureCnt(string titleName)
        {
            // DB연결 후 쇼핑몰 홈의 일반 정보 가져옴
            return shop_home_nature_cnt;
        }

        public int GetShopReviewCnt(string titleName)
        {
            // DB연결 후 쇼핑몰 홈의 일반 정보 가져옴
            return shop_home_review;
        }

    }
}
