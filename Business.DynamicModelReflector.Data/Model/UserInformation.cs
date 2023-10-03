using System.ComponentModel.DataAnnotations;

namespace Business.DynamicModelReflector.Data.Model
{
    public class UserInformation
    {
        [Key]
        public int Id { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}