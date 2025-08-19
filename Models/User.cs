using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SecureNotesAPI.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public List<Note> Notes { get; set; }
    }
}
