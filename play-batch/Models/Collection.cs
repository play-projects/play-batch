namespace batch.Models
{
    public class Collection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PosterPath { get; set; }
        public string BackdropPath { get; set; }

        public static Collection NotFound = new Collection
        {
            Id = 0,
            Name = string.Empty,
            PosterPath = string.Empty,
            BackdropPath = string.Empty
        };
    }
}
