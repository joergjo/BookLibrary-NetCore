using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BookLibrary.Common;

namespace BookLibrary.Models
{
    public class Book
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }
        
        [JsonConverter(typeof(EpochDateTimeConverter))]
        public DateTime? ReleaseDate { get; set; }
        
        public List<Keyword> Keywords { get; set; } = new List<Keyword>();
    }
}
