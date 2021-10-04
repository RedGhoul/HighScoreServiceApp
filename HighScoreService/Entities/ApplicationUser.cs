using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "nvarchar(36)")]
        public string FirstName { get; set; }
        [Column(TypeName = "nvarchar(36)")]
        public string LastName { get; set; }
        [Column(TypeName = "nvarchar(36)")]
        public string Company { get; set; }
        public ICollection<Game> Games { get; set; }
    }
}
