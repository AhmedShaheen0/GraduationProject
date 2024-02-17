using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace ApiApplicationProject.Models

{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        public int Age { get; set; } = 17;

    }
}
