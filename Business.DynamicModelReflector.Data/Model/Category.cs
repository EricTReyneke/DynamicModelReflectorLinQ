using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Business.DynamicModelReflector.Data.Model
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("Tournament")]
        public Guid TournamentId { get; set; }

        [IgnoreDataMember]
        public Tournament Tournament { get; set; }
    }
}