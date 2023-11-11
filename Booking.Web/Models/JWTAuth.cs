using Newtonsoft.Json;

namespace Booking.Web.Models
{
    [JsonObject("Jwt")]
    public class JWTAuth
    {
        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("audience")]
        public string Audience { get; set; }

        [JsonProperty("accessExpiration")]
        public int AccessExpiration { get; set; }

    }
}
