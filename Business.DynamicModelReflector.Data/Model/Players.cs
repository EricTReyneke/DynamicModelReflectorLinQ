using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Business.DynamicModelReflector.Data.Model
{
    public class Players
    {
        [Key]
        public int Player_Id { get; set; }

        public string Player_Name { get; set; }

        [ForeignKey("Teams")]
        public int Team_Id { get; set; }
    }
}