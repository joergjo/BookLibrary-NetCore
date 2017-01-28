using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Api.Models
{
    public class Book
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }
        
        public long? ReleaseDate { get; set; }
        
        public List<Keyword> Keywords { get; set; } = new List<Keyword>();
    }
}