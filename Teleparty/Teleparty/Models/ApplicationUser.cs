using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teleparty.Models
{
    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public string ConnectionId { get; set; }
    }
}
