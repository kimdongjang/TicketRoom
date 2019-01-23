using Newtonsoft.Json;

namespace TicketRoom.Models.Gift
{
    public class G_CategoryInfo
    {
        [JsonProperty("CategoryNum")]
        public string CategoryNum { get; set; } // 카테고리 번호
        [JsonProperty("Name")]
        public string Name { get; set; }// 카테고리 이름
        [JsonProperty("Image")]
        public string Image { get; set; }// 카테고리 이미지
        [JsonProperty("Error")]
        public string Error { get; set; }// 에러 문
    }
}
