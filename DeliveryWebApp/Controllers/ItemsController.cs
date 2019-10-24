using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeliveryWebApp.Data;
using Microsoft.AspNetCore.Authorization;

namespace DeliveryWebApp.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }





        // GET: Items
        public async Task<IActionResult> Index()
        {
            return View(await _context.ItemsCardapio.ToListAsync());
        }





        // GET: Items/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemCardapio = await _context.ItemsCardapio
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemCardapio == null)
            {
                return NotFound();
            }

            return View(itemCardapio);
        }






        // GET: Items/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }






        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao,Preco,Imagem")] ItemCardapio itemCardapio)
        {
            if (ModelState.IsValid)
            {
                itemCardapio.Id = Guid.NewGuid();
                _context.Add(itemCardapio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(itemCardapio);
        }






        // GET: Items/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemCardapio = await _context.ItemsCardapio.FindAsync(id);
            if (itemCardapio == null)
            {
                return NotFound();
            }
            return View(itemCardapio);
        }






        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nome,Descricao,Preco,Imagem")] ItemCardapio itemCardapio)
        {
            if (id != itemCardapio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemCardapio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemCardapioExists(itemCardapio.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(itemCardapio);
        }






       

        private bool ItemCardapioExists(Guid id)
        {
            return _context.ItemsCardapio.Any(e => e.Id == id);
        }
    }
}
