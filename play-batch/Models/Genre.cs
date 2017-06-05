namespace batch.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static Genre NotFound = new Genre
        {
            Id = 0,
            Name = string.Empty
        };
    }
}
