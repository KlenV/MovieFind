using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;

namespace ticket_office_WEB.Models
{
    public class MovieModel
    {
        public int Id { get; set; }
        public string Title => MediaType == "movie" ? title : name;
        public string title { get; set; }
        public string name { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        [JsonProperty("first_air_date")]
        public string FirstAirDate { get; set; }

        [JsonProperty("vote_average")]
        public float VoteAverage { get; set; }

        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("genre_ids")]
        public List<int> Genres { get; set; }

        [JsonProperty("media_type")]
        public string MediaType { get; set; }

        public string ReleaseYear => !string.IsNullOrEmpty(ReleaseDate) ? ReleaseDate.Substring(0, 4) :
                                     (!string.IsNullOrEmpty(FirstAirDate) ? FirstAirDate.Substring(0, 4) : "N/A");
    }



    public class MovieResponseModel
    {
        public int Page { get; set; }
        public List<MovieModel> Results { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }
}
