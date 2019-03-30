using BookLibrary.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Models
{
    public class Book
    {
        [JsonProperty("_id")]
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