using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.API.Data;
using TodoApp.API.Models;

namespace TodoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoDbContext _todoDbContext;

        public TodoController(TodoDbContext todoDbContext)
        {
            _todoDbContext = todoDbContext;
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<IActionResult> GetAllTodos()
        {
            var todos = await _todoDbContext.Todos.ToListAsync();
            return Ok(todos);
        }

        // GET: api/Todo/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoById(Guid id)
        {
            var todo = await _todoDbContext.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(todo);
        }

        // POST: api/Todo
        [HttpPost]
        public async Task<IActionResult> AddTodo(Todo todo)
        {
            todo.Id = Guid.NewGuid();
            _todoDbContext.Todos.Add(todo);
            await _todoDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTodoById), new { id = todo.Id }, todo);
        }

        // PUT: api/Todo/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(Guid id, Todo todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }

            _todoDbContext.Entry(todo).State = EntityState.Modified;

            try
            {
                await _todoDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Todo/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(Guid id)
        {
            var todo = await _todoDbContext.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _todoDbContext.Todos.Remove(todo);
            await _todoDbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoExists(Guid id)
        {
            return _todoDbContext.Todos.Any(e => e.Id == id);
        }
    }
}