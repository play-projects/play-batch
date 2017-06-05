namespace batch.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Character { get; set; }
        public int Order { get; set; }
        public string ProfilePath { get; set; }

        public static Person NotFound = new Person
        {
            Id = 0,
            Name = string.Empty,
            Character = string.Empty,
            Order = 0,
            ProfilePath = string.Empty
        };
    }
}
