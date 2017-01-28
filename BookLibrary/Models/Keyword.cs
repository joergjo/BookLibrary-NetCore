using Newtonsoft.Json;

namespace BookLibrary.Api.Models
{
    public class Keyword
    {
        [JsonProperty("keyword")]
        public string Name { get; set; }
    }
}