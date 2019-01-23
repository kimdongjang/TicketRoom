using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using TicketRoom.Models.USERS;

namespace TicketRoom.Services
{
    class FacebookServices
    {
        public async Task<FacebookProfile> GetFacebookProfileAsync(string accessToken)
        {
            var requestUrl =
                "https://graph.facebook.com/v3.2/me?fields=name,address,age_range,birthday,email,gender,hometown,id,location,first_name,last_name,languages,about&access_token="
                + accessToken;

            var httpClient = new HttpClient();

            var userJson = await httpClient.GetStringAsync(requestUrl);

            var facebookProfile = JsonConvert.DeserializeObject<FacebookProfile>(userJson);

            return facebookProfile;
        }
    }
}
