using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManagementContext _context;

        public AccountController(UserManagementContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username &&
                                            u.Password == model.Password);

                if (user != null)
                {
                    // In a real application, use proper authentication
                    return RedirectToAction("Index", "Dashboard");
                }

                ModelState.AddModelError("", "Invalid username or password");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}
