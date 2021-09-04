using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthorizationService.Models
{
    public class Login
    {
        [Key]
        [ForeignKey("Account")]
        public Guid AccountId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        public Account Account { get; set; }

    }
}
