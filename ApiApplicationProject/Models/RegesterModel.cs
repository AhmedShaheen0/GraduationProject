using System.ComponentModel.DataAnnotations;

namespace ApiApplicationProject.Models
{
    public class RegesterModel
    {
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [MaxLength(100)]
        
        public string Password { get; set; }
        public int Age { get; set; } = 17;


    }
}
