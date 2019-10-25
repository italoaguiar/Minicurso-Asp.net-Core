using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryWebApp.Data;

namespace DeliveryWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MenuItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MenuItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemCardapio>>> GetItemsCardapio()
        {
            //return await _context.ItemsCardapio.ToListAsync();
            return await _context.ItemsCardapio
                .Where(x => x.Nome.Contains("Bacon") && x.Preco < 20)
                .Take(5)
                .ToListAsync();

        }

        // GET: api/MenuItem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemCardapio>> GetItemCardapio(Guid id)
        {
            var itemCardapio = await _context.ItemsCardapio.FindAsync(id);

            if (itemCardapio == null)
            {
                return NotFound();
            }

            return itemCardapio;
        }

        // PUT: api/MenuItem/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemCardapio(Guid id, ItemCardapio itemCardapio)
        {
            if (id != itemCardapio.Id)
            {
                return BadRequest();
            }

            _context.Entry(itemCardapio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemCardapioExists(id))
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

        // POST: api/MenuItem
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ItemCardapio>> PostItemCardapio(ItemCardapio itemCardapio)
        {
            _context.ItemsCardapio.Add(itemCardapio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemCardapio", new { id = itemCardapio.Id }, itemCardapio);
        }

        // DELETE: api/MenuItem/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemCardapio>> DeleteItemCardapio(Guid id)
        {
            var itemCardapio = await _context.ItemsCardapio.FindAsync(id);
            if (itemCardapio == null)
            {
                return NotFound();
            }

            _context.ItemsCardapio.Remove(itemCardapio);
            await _context.SaveChangesAsync();

            return itemCardapio;
        }

        private bool ItemCardapioExists(Guid id)
        {
            return _context.ItemsCardapio.Any(e => e.Id == id);
        }
    }
}
