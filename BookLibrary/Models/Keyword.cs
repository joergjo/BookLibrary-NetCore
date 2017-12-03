using Newtonsoft.Json;

namespace BookLibrary.Models
{
    public class Keyword
    {
        [JsonProperty("keyword")]
        public string Name { get; set; }
    }
}