namespace batch.Models
{
    public enum Language
    {
        VF, VOSTFR, None
    }

    public enum Quality
    {
        Low,        // Bdrip
        Medium,     // 720p
        High,       // 1080p
        VeryHigh,   // 4K
        None
    }

    public enum Category
    {
        Series, Movie, None
    }

    public class Torrent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Year { get; set; }

        public double Size { get; set; }
        public int Seeders { get; set; }
        public int Leechers { get; set; }
        public int Completed { get; set; }

        public Category Category { get; set; }
        public Language Language { get; set; }
        public Quality Quality { get; set; }

        public static Torrent NotFound = new Torrent
        {
            Id = 0,
            Name = string.Empty,
            Slug = string.Empty,
            Year = 0,
            Size = 0,
            Seeders = 0,
            Leechers = 0,
            Completed = 0,
            Category = Category.None,
            Language = Language.None,
            Quality = Quality.None
        };
    }
}
