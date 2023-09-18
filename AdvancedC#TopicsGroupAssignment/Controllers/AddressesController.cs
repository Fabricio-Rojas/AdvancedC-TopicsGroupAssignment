using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdvancedC_TopicsGroupAssignment.Data;
using AdvancedC_TopicsGroupAssignment.Models;

namespace AdvancedC_TopicsGroupAssignment.Controllers
{
    public class AddressesController : Controller
    {
        private readonly ContactInfoContext _context;

        public AddressesController(ContactInfoContext context)
        {
            _context = context;
        }

        // GET: Addresses
        public async Task<IActionResult> Index()
        {
            var contactInfoContext = _context.Addresses.Include(a => a.Business);
            return View(await contactInfoContext.ToListAsync());
        }

        // GET: Addresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Addresses == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .Include(a => a.Business)
                .Include(a => a.People)
                .ThenInclude(pa => pa.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (address == null)
            {
                return NotFound();
            }

            ViewBag.People = await _context.Persons.ToListAsync();

            return View(address);
        }

        // GET: Addresses/Create
        public IActionResult Create()
        {
            ViewData["BusinessId"] = new SelectList(_context.Businesses, "Id", "Name");
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StreetName,StreetNumber,UnitNumber,PostalCode,BusinessId")] Address address)
        {
            if (ModelState.IsValid)
            {
                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BusinessId"] = new SelectList(_context.Businesses, "Id", "Name", address.BusinessId);
            return View(address);
        }

        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Addresses == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            ViewData["BusinessId"] = new SelectList(_context.Businesses, "Id", "Name", address.BusinessId);
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StreetName,StreetNumber,UnitNumber,PostalCode,BusinessId")] Address address)
        {
            if (id != address.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.Id))
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
            ViewData["BusinessId"] = new SelectList(_context.Businesses, "Id", "Email", address.BusinessId);
            return View(address);
        }

        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Addresses == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .Include(a => a.Business)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Addresses == null)
            {
                return Problem("Entity set 'ContactInfoContext.Addresses'  is null.");
            }
            var address = await _context.Addresses.FindAsync(id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPerson(int personId, int addressId)
        {
            Person? person = await _context.Persons
                .Include(p => p.Addresses)
                .FirstOrDefaultAsync(p => p.Id == personId);

            Address? address = await _context.Addresses
                .Include(a => a.People)
                .FirstOrDefaultAsync(a => a.Id == addressId);

            if (person == null || address == null)
            {
                return NotFound();
            }

            PersonAddress personAddress = new PersonAddress { PersonId = personId, AddressId = addressId };
            person.Addresses.Add(personAddress);
            address.People.Add(personAddress);
            _context.PersonAddresses.Add(personAddress);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = addressId });
        }

        public IActionResult AddNewPerson(int addressId)
        {
            ViewBag.AddressId = addressId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewPerson([Bind("Id,FirstName,LastName,Email,PhoneNumber")] Person person, int addressId)
        {
            if (ModelState.IsValid)
            {
                _context.Persons.Add(person);
                await _context.SaveChangesAsync();

                Address address = _context.Addresses
                    .Include(a => a.People)
                    .First(a => a.Id == addressId);

                PersonAddress personAddress = new() { PersonId = person.Id, AddressId = addressId };
                person.Addresses.Add(personAddress);
                address.People.Add(personAddress);
                _context.PersonAddresses.Add(personAddress);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = addressId });
            }
            ViewBag.AddressId = addressId;
            return View(person);
        }

        public async Task<IActionResult> RemovePerson(int personId, int addressId)
        {
            Person? person = await _context.Persons
                .Include(p => p.Addresses)
                .FirstOrDefaultAsync(p => p.Id == personId);

            Address? address = await _context.Addresses
                .Include(a => a.People)
                .FirstOrDefaultAsync(a => a.Id == addressId);

            if (person == null || address == null)
            {
                return NotFound();
            }

            PersonAddress? personAddress = address.People.FirstOrDefault(pa => pa.PersonId == personId);
            if (personAddress != null)
            {
                person.Addresses.Remove(personAddress);
                address.People.Remove(personAddress);
                _context.PersonAddresses.Remove(personAddress);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = addressId });
        }

        private bool AddressExists(int id)
        {
          return (_context.Addresses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
