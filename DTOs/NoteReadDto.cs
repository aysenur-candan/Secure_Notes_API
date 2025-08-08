using System;
namespace SecureNotesAPI.DTOs
{
    public class NoteReadDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
