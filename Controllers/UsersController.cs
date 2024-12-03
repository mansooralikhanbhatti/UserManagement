using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using User = UserManagement.Models.User;

namespace UserManagement.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManagementContext _context;

        public UsersController(UserManagementContext context)
        {
            _context = context;
        }

        // UserList Method
        public async Task<IActionResult> UserList(string searchString, string sortOrder)
        {
            var usersQuery = from u in _context.Users
                             select u;

            // Filter users based on search string
            if (!string.IsNullOrEmpty(searchString))
            {
                usersQuery = usersQuery.Where(u => u.FirstName.Contains(searchString) ||
                                                    u.LastName.Contains(searchString) ||
                                                    u.Username.Contains(searchString));
            }

            // Sort users based on sort order
            switch (sortOrder)
            {
                case "name_desc":
                    usersQuery = usersQuery.OrderByDescending(u => u.FirstName);
                    break;
                default:
                    usersQuery = usersQuery.OrderBy(u => u.FirstName);
                    break;
            }

            // Include related Status data if needed (assuming you have a Status navigation property)
            var users = await usersQuery.Include(u => u.Status).ToListAsync();

            // Ensure you are passing the correct model (UserManagement.Models.User)
            var userList = users.Select(u => new UserManagement.Models.User
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Username = u.Username,
                EmailID = u.EmailId,
                PhoneNumber = u.PhoneNumber,
                City = u.City,
                Status = u.Status // Assuming Status is a navigation property
            }).ToList();

            return View(userList);  // Return the correct type to the view
        }


        // Create Method (GET)
        public IActionResult Create()
        {
            ViewBag.Statuses = new SelectList(_context.UserStatus, "StatusId", "StatusName");
            return View();
        }

        // Create Method (POST)
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(UserList)); // Ensure a valid return
            }

            ViewBag.Statuses = new SelectList(_context.UserStatus, "StatusId", "StatusName");
            return View(user); // Returning the same view if model is invalid
        }

        // Edit Method (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Statuses = new SelectList(_context.UserStatus, "StatusId", "StatusName");
            return View(user);
        }

        // Edit Method (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(UserList)); // Ensure a valid return
            }

            ViewBag.Statuses = new SelectList(_context.UserStatus, "StatusId", "StatusName");
            return View(user); // Returning the view with validation errors if ModelState is invalid
        }

        // Delete Method (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Status)
                .FirstOrDefaultAsync(m => m.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user); // Return view with the user to be deleted
        }

        // Delete Method (POST)
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UserList)); // Return to UserList after deletion
        }

        // Ensure that this method exists
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
