namespace batch.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Character { get; set; }
        public int Order { get; set; }
        public string ProfilePath { get; set; }

        public static Person NotFound = new Person
        {
            Id = 0,
            Lastname = string.Empty,
            Firstname = string.Empty,
            Character = string.Empty,
            Order = 0,
            ProfilePath = string.Empty
        };
    }
}
