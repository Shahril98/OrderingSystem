using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure.Dtos
{
    public class AddUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 8)]
        public string Password { get; set; }
        public List<string> Roles { get; set; } = [];
    }
    public class UserCreatedDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}
