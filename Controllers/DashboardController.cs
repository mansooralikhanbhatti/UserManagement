using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManagementContext _context;

        public DashboardController(UserManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalUsers = await _context.Users.CountAsync(),
                ActiveUsers = await _context.Users.CountAsync(u => u.StatusId == 1),
                InactiveUsers = await _context.Users.CountAsync(u => u.StatusId == 2)
            };

            return View(viewModel);
        }
    }
}
