using System.ComponentModel.DataAnnotations;

namespace Business.DynamicModelReflector.Data.Model
{
    public class Tournament
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string UnionName { get; set; }

        public string Address { get; set; }

        public string Duration { get; set; }

        public int PitsPlayable { get; set; }

        public string Type { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int? Extension { get; set; }

        public bool IsActive { get; set; }
    }
}