using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.DynamicModelReflector.Data.Model
{
    public class Teams
    {
        [Key]
        public int Team_Id { get; set; }

        public string Team_Name{ get; set; }

        [ForeignKey("Tournaments")]
        public int Tournament_Id { get; set; }
    }
}