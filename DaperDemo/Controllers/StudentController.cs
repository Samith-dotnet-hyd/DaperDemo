using DaperDemo.Models;
using DaperDemo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DaperDemo.Controllers
{
    

        [ApiController]
        [Route("api/[controller]")]
        public class StudentsController : ControllerBase
        {
            private readonly IStudentRepository _repo;

            public StudentsController(IStudentRepository repo)
            {
                _repo = repo;
            }

            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var students = await _repo.GetAllAsync();
                return Ok(students);
            }

            [HttpGet("{id:int}")]
            public async Task<IActionResult> Get(int id)
            {
                var student = await _repo.GetByIdAsync(id);
                if (student == null) return NotFound();
                return Ok(student);
            }

            [HttpPost]
            public async Task<IActionResult> Create([FromBody] Student student)
            {
                if (student == null) return BadRequest();
                var newId = await _repo.CreateAsync(student);
                student.Id = newId;
                return CreatedAtAction(nameof(Get), new { id = newId }, student);
            }

            [HttpPut("{id:int}")]
            public async Task<IActionResult> Update(int id, [FromBody] Student student)
            {
                if (student == null || id != student.Id) return BadRequest();
                var exists = await _repo.GetByIdAsync(id);
                if (exists == null) return NotFound();

                var updated = await _repo.UpdateAsync(student);
                if (!updated) return StatusCode(500, "A problem happened while updating the student.");
                return NoContent();
            }

            [HttpDelete("{id:int}")]
            public async Task<IActionResult> Delete(int id)
            {
                var exists = await _repo.GetByIdAsync(id);
                if (exists == null) return NotFound();

                var deleted = await _repo.DeleteAsync(id);
                if (!deleted) return StatusCode(500, "A problem happened while deleting the student.");
                return NoContent();
            }
        }
    }

