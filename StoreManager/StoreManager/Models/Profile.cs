using System;
using System.ComponentModel.DataAnnotations;

namespace StoreManager.Models
{
    public class Profile
    {
        [Required]
        public string EMail { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
    }
}
