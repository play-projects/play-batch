using System;

namespace entities.Models
{
    public partial class Character
    {
        public int PersonId { get; set; }
        public int MovieId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
