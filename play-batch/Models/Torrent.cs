namespace batch.Models
{
    public class Torrent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Size { get; set; }
        public int Seeders { get; set; }
        public int Leechers { get; set; }
        public int Completed { get; set; }

        public static Torrent NotFound = new Torrent
        {
            Id = 0,
            Name = string.Empty,
            Size = 0,
            Seeders = 0,
            Leechers = 0,
            Completed = 0
        };
    }
}
