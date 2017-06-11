using System;

namespace entities.Models
{
    public partial class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
