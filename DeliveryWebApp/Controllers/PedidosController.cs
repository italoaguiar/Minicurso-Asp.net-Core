using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeliveryWebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace DeliveryWebApp.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public PedidosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            var pedido = await _context.Pedidos.Where(x => x.Cliente.Id == _userManager.GetUserId(User)).ToListAsync();
            return View(pedido);
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.Include("Items.Item").Include(x=> x.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        

        
    }
}
