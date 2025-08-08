using System;
using System.ComponentModel.DataAnnotations;

namespace SecureNotesAPI.DTOs
{
    public class NoteUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }
    }
}
