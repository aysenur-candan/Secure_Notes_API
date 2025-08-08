using SecureNotesAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecureNotesAPI.DTOs;

namespace SecureNotesAPI.Services
{
    public interface INoteService
    {
        Task<List<NoteReadDto>> GetAllNotes(Guid userId);
        Task<Note> GetNoteByIdAsync(Guid id, Guid userId);
        Task<bool> UpdateNoteAsync(NoteUpdateDto noteDto, Guid userId);
        Task<bool> DeleteNoteAsync(Guid id, Guid userId);
        Task<Note> NoteCreate(NoteCreateDto dto, Guid userId);
    }
}
