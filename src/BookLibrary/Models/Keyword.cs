using System.Text.Json.Serialization;

namespace BookLibrary.Models
{
    public class Keyword
    {
        [JsonPropertyName("keyword")]
        public string? Name { get; set; }
    }
}
