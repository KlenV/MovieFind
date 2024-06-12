using Newtonsoft.Json;

namespace ticket_office_WEB.Models
{
    public class GenreModel
    {
        [JsonProperty("genres")]
        public List<Genre> Genres { get; set; }
    }

    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}