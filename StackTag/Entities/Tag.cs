﻿using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace StackTag.Entities
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Tag
    {
        public int? Id { get; set; }
        public int? Count { get; set; }
        public bool? HasSynonyms { get; set; }
        public bool? IsModeratorOnly { get; set; }
        public bool? IsRequired { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public string? Name { get; set; }
        public List<string>? Synonyms { get; set; }
        public int? UserId { get; set; }
        public List<Collective>? Collectives { get; set; }
        public double? Percentage { get; set; }
    }    
}
