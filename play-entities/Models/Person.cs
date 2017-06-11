using System;

namespace entities.Models
{
    public partial class Person
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string ProfilePath { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
