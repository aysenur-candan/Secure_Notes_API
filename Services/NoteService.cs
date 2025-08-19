using Microsoft.EntityFrameworkCore;
using SecureNotesAPI.Data;
using SecureNotesAPI.DTOs;
using SecureNotesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureNotesAPI.Services
{
    public class NoteService : INoteService
    {
        private readonly AppDbContext dbContext;
        public NoteService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Note> NoteCreate(NoteCreateDto dto, Guid userId)
        {
            var not = new Note
            {
                Title = dto.Title,
                Content = dto.Content,
                CreationDate = DateTime.UtcNow,
                UserId = userId
            };

            dbContext.Notes.Add(not);
            await dbContext.SaveChangesAsync();
            return not;
        }
        public async Task<List<NoteReadDto>> GetAllNotes(Guid userId)
        {
            var notes = await dbContext.Notes
                .Where(n => n.UserId == userId)
                .Select(n => new NoteReadDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    CreationDate = n.CreationDate
                })
                .ToListAsync();

            return notes;
        }
        public async Task<Note> GetNoteByIdAsync(Guid id, Guid UserId)
        {
            return await dbContext.Notes
                .Where(n => n.Id == id && n.UserId == UserId)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> UpdateNoteAsync(NoteUpdateDto noteDto, Guid userId)
        {
            var note = await dbContext.Notes.FirstOrDefaultAsync(n => n.Id == noteDto.Id && n.UserId == userId);
            if (note == null)
                return false;

            note.Title = noteDto.Title;
            note.Content = noteDto.Content;

            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteNoteAsync(Guid noteId, Guid userId)
        {
            var note = await dbContext.Notes.FirstOrDefaultAsync(n => n.Id == noteId && n.UserId == userId);
            if (note == null)
                return false;

            dbContext.Notes.Remove(note);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
