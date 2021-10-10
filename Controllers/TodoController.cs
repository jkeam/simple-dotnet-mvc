using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimpleDotnetMvc.Data;
using SimpleDotnetMvc.Models;

namespace SimpleDotnetMvc
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TodoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Todo
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Todos.ToListAsync());
            return View(await _context.Todos
                .Where(t => t.Owner == new AppUser(User).GetId())
                .ToListAsync());
        }

        // GET: Todo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todos.FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null || !UserOwnsTodo(User, todo))
            {
                return NotFound();
            }

            return View(todo);
        }

        // GET: Todo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description")] Todo todo)
        {
            // set default values
            todo.CreatedAt = DateTime.UtcNow;
            SetOwner(User, ModelState, todo);
            if (ModelState.IsValid)
            {
                _context.Add(todo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todos.FindAsync(id);
            if (todo == null || !UserOwnsTodo(User, todo))
            {
                return NotFound();
            }
            return View(todo);
        }

        // POST: Todo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Description,CompletedAt")] Todo todo)
        {
            var existingTodo = await _context.Todos.FindAsync(id);
            if (existingTodo == null || !UserOwnsTodo(User, existingTodo))
            {
                return NotFound();
            }

            existingTodo.Title = todo.Title;
            existingTodo.Description = todo.Description;
            existingTodo.CompletedAt = todo.CompletedAt;
            try
            {
                _context.Update(existingTodo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(todo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            //return View(todo);
        }

        // GET: Todo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todos.FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null || !UserOwnsTodo(User, todo))
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: Todo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null || !UserOwnsTodo(User, todo))
            {
                return RedirectToAction(nameof(Index));
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void SetOwner(ClaimsPrincipal user, ModelStateDictionary modelState, Todo todo)
        {
            todo.Owner = new AppUser(user).GetId();
            modelState.Remove("Owner");
        }

        private bool TodoExists(int id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }

        private bool UserOwnsTodo(ClaimsPrincipal user, Todo todo)
        {
            if (user == null || todo == null)
            {
                return false;
            }
            var appUser = new AppUser(user);
            return appUser.GetId() == todo.Owner;
        }
    }
}
