using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SecureNotesAPI.DTOs;
using SecureNotesAPI.Services;

namespace SecureNotesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly NoteService noteService;

        public NotesController(NoteService noteService)
        {
            this.noteService = noteService;
        }
        private Guid GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("The token is invalid or the user ID was not found.");
            return Guid.Parse(userIdClaim.Value);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> NoteAdd(NoteCreateDto dto)
        {
            var userId = GetUserIdFromToken();
            await noteService.NoteCreate(dto, userId);
            return Ok(new
            {
                success = true,
                message = "Note added successfully.",
            });
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllNotes()
        {
            var userId = GetUserIdFromToken();
            var notes = await noteService.GetAllNotes(userId);
            return Ok(new
            {
                success = true,
                message = "Notes listed successfully.",
                data = notes
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoteById(Guid id)
        {
            var userId = GetUserIdFromToken();
            var note = await noteService.GetNoteByIdAsync(id, userId);
            if (note == null)
                return NotFound("The note was not found or you do not have access permission.");
            return Ok(note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(Guid id, [FromBody] NoteUpdateDto noteDto)
        {
            var userId = GetUserIdFromToken();

            if (noteDto.Id != id)
                return BadRequest("Note ID mismatch.");

            var result = await noteService.UpdateNoteAsync(noteDto, userId);
            if (!result)
                return NotFound("The note was not found or you do not have access permission.");

            return Ok("Note updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            var userId = GetUserIdFromToken();

            var result = await noteService.DeleteNoteAsync(id, userId);
            if (!result)
                return NotFound("The note was not found or you do not have access permission.");

            return Ok("Note deleted successfully.");
        }
    }
}
