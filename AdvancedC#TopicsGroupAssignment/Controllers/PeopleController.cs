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
    public class PeopleController : Controller
    {
        private readonly ContactInfoContext _context;

        public PeopleController(ContactInfoContext context)
        {
            _context = context;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
              return _context.Persons != null ? 
                          View(await _context.Persons.ToListAsync()) :
                          Problem("Entity set 'ContactInfoContext.Persons'  is null.");
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Persons == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .Include(p => p.Addresses)
                .ThenInclude(pa => pa.Address)
                .Include(p => p.Businesses)
                .ThenInclude(bp => bp.Business)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            ViewBag.Addresses = await _context.Addresses.ToListAsync();
            ViewBag.Businesses = await _context.Businesses.ToListAsync();

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PhoneNumber")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Persons == null)
            {
                return NotFound();
            }

            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PhoneNumber")] Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
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
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Persons == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Persons == null)
            {
                return Problem("Entity set 'ContactInfoContext.Persons'  is null.");
            }
            var person = await _context.Persons.FindAsync(id);
            if (person != null)
            {
                _context.Persons.Remove(person);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(int personId, int addressId)
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
            
            return RedirectToAction(nameof(Details), new { id = personId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBusiness(int personId, int businessId)
        {
            Person? person = await _context.Persons
                .Include(p => p.Businesses)
                .FirstOrDefaultAsync(p => p.Id == personId);

            Business? business = await _context.Businesses
                .Include(b => b.People)
                .FirstOrDefaultAsync(b => b.Id == businessId);

            if (person == null || business == null)
            {
                return NotFound();
            }

            BusinessPerson businessPerson = new BusinessPerson { PersonId = personId, BusinessId = businessId };
            person.Businesses.Add(businessPerson);
            business.People.Add(businessPerson);
            _context.BusinessPersons.Add(businessPerson);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = personId });
        }

        public IActionResult AddNewAddress(int personId)
        {
            ViewData["BusinessId"] = new SelectList(_context.Businesses, "Id", "Name");
            ViewBag.PersonId = personId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewAddress([Bind("Id,StreetName,StreetNumber,UnitNumber,PostalCode,BusinessId")] Address address, int personId)
        {
            if (ModelState.IsValid)
            {
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();

                Person person = _context.Persons
                    .Include(p => p.Addresses)
                    .First(p => p.Id == personId);

                PersonAddress personAddress = new() { PersonId = personId, AddressId = address.Id };
                person.Addresses.Add(personAddress);
                address.People.Add(personAddress);
                _context.PersonAddresses.Add(personAddress);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = personId });
            }
            ViewBag.PersonId = personId;
            ViewData["BusinessId"] = new SelectList(_context.Businesses, "Id", "Name", address.BusinessId);
            return View(address);
        }

        public IActionResult AddNewBusiness(int personId)
        {
            ViewBag.PersonId = personId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewBusiness([Bind("Id,Name,PhoneNumber,Email")] Business business, int personId)
        {
            if (ModelState.IsValid)
            {
                _context.Businesses.Add(business);
                await _context.SaveChangesAsync();

                Person person = _context.Persons
                    .Include(p => p.Businesses)
                    .First(p => p.Id == personId);

                BusinessPerson businessPerson = new() { PersonId = personId, BusinessId = business.Id};
                person.Businesses.Add(businessPerson);
                business.People.Add(businessPerson);
                _context.BusinessPersons.Add(businessPerson);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = personId });
            }
            ViewBag.PersonId = personId;
            return View(business);
        }

        public async Task<IActionResult> RemoveAddress(int personId, int addressId)
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

            PersonAddress? personAddress = person.Addresses.FirstOrDefault(pa => pa.AddressId == addressId);
            if (personAddress != null)
            {
                person.Addresses.Remove(personAddress);
                address.People.Remove(personAddress);
                _context.PersonAddresses.Remove(personAddress);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = personId });
        }

        public async Task<IActionResult> RemoveBusiness(int personId, int businessId)
        {
            Person? person = await _context.Persons
                .Include(p => p.Businesses)
                .FirstOrDefaultAsync(p => p.Id == personId);

            Business? business = await _context.Businesses
                .Include(b => b.People)
                .FirstOrDefaultAsync(b => b.Id == businessId);

            if (person == null || business == null)
            {
                return NotFound();
            }

            BusinessPerson? businessPerson = person.Businesses.FirstOrDefault(bp => bp.BusinessId == businessId);
            if (businessPerson != null)
            {
                person.Businesses.Remove(businessPerson);
                business.People.Remove(businessPerson);
                _context.BusinessPersons.Remove(businessPerson);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = personId });
        }

        private bool PersonExists(int id)
        {
          return (_context.Persons?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
