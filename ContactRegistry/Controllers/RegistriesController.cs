using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContactRegistry.Data;
using ContactRegistry.Models;

namespace ContactRegistry.Controllers
{
    public class RegistriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Registries
        public async Task<IActionResult> Index()
        {
              return _context.Registry != null ? 
                          View(await _context.Registry.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Registry'  is null.");
        }

        // GET: Registries/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            //Returns all of the items requested by the user 
            //If information isn't found that's what the output will be. 
            if (id == null || _context.Registry == null)
            {
                return NotFound();
            }

            var registry = await _context.Registry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registry == null)
            {
                return NotFound();
            }

            return View(registry);
        }

        // GET: Registries/Create
        //Returns the view for the create overload
        public IActionResult Create()
        {
            return View();
        }

        // POST: Registries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Bind is used so that all of the properties listed will populate. It is also used to increase security 
        // and unauthorized data to be posted on a server. 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Phone,Email")] Registry registry)
        {
            //ModelState.IsValid indicates if it is possible to bind the incoming values from the request to the model correctly
            // Tells you if there are issues with your data posted to the server
            if (ModelState.IsValid)
            {
                _context.Add(registry);
                await _context.SaveChangesAsync(); // This saves the information to the database
                return RedirectToAction(nameof(Index)); // Repopulates the information to the Index view. Will change this later
            }
            return View(registry);
        }

        // GET: Registries/Edit/5
        // Simpling selecting the item that you want to edit and returning its view 
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Registry == null)
            {
                return NotFound();
            }

            var registry = await _context.Registry.FindAsync(id);
            if (registry == null)
            {
                return NotFound();
            }
            return View(registry);
        }

        // POST: Registries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // We have the option to edit id, firstname, lastname, phone, and email because of the bind. We are accessing the information from the Registry

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Phone,Email")] Registry registry)
        {
            if (id != registry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistryExists(registry.Id))
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
            return View(registry);
        }

        // GET: Registries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Registry == null)
            {
                return NotFound();
            }

            var registry = await _context.Registry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registry == null)
            {
                return NotFound();
            }

            return View(registry);
        }

        // POST: Registries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Registry == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Registry'  is null.");
            }
            var registry = await _context.Registry.FindAsync(id);
            if (registry != null)
            {
                _context.Registry.Remove(registry); //Removes the item from the registry if not null
            }
            
            await _context.SaveChangesAsync(); //Saves it to the database 
            return RedirectToAction(nameof(Index));
        }

        //Lets you know if the item exists inside of the registry 
        private bool RegistryExists(int id)
        {
          return (_context.Registry?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
