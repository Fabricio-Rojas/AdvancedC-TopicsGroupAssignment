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
    public class BusinessesController : Controller
    {
        private readonly ContactInfoContext _context;

        public BusinessesController(ContactInfoContext context)
        {
            _context = context;
        }

        // GET: Businesses
        public async Task<IActionResult> Index()
        {
              return _context.Businesses != null ? 
                          View(await _context.Businesses.ToListAsync()) :
                          Problem("Entity set 'ContactInfoContext.Businesses'  is null.");
        }

        // GET: Businesses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Businesses == null)
            {
                return NotFound();
            }

            var business = await _context.Businesses
                .Include(b => b.Addresses)
                .Include(b => b.People)
                .ThenInclude(bp => bp.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (business == null)
            {
                return NotFound();
            }

            ViewBag.Addresses = await _context.Addresses.ToListAsync();
            ViewBag.People = await _context.Persons.ToListAsync();

            return View(business);
        }

        // GET: Businesses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Businesses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PhoneNumber,Email")] Business business)
        {
            if (ModelState.IsValid)
            {
                _context.Add(business);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(business);
        }

        // GET: Businesses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Businesses == null)
            {
                return NotFound();
            }

            var business = await _context.Businesses.FindAsync(id);
            if (business == null)
            {
                return NotFound();
            }
            return View(business);
        }

        // POST: Businesses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PhoneNumber,Email")] Business business)
        {
            if (id != business.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(business);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusinessExists(business.Id))
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
            return View(business);
        }

        // GET: Businesses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Businesses == null)
            {
                return NotFound();
            }

            var business = await _context.Businesses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (business == null)
            {
                return NotFound();
            }

            return View(business);
        }

        // POST: Businesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Businesses == null)
            {
                return Problem("Entity set 'ContactInfoContext.Businesses'  is null.");
            }
            var business = await _context.Businesses.FindAsync(id);
            if (business != null)
            {
                _context.Businesses.Remove(business);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(int businessId, int addressId)
        {
            Business? newBusiness = await _context.Businesses
                .Include(b => b.Addresses)
                .FirstOrDefaultAsync(b => b.Id == businessId);

            Address? address = await _context.Addresses
                .Include(a => a.Business)
                .FirstOrDefaultAsync(a => a.Id == addressId);

            if (newBusiness == null || address == null)
            {
                return NotFound();
            }

            Business? oldBusiness = await _context.Businesses
                .Include(b => b.Addresses)
                .FirstOrDefaultAsync (b => b.Id == address.BusinessId);

            if (oldBusiness == null)
            {
                return NotFound();
            }

            address.BusinessId = newBusiness.Id;
            address.Business = newBusiness;
            oldBusiness.Addresses.Remove(address);
            newBusiness.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = businessId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPerson(int personId, int businessId)
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

            return RedirectToAction(nameof(Details), new { id = businessId });
        }

        public IActionResult AddNewAddress(int businessId)
        {
            ViewBag.BusinessId = businessId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewAddress([Bind("Id,StreetName,StreetNumber,UnitNumber,PostalCode,BusinessId")] Address address, int businessId)
        {
            if (ModelState.IsValid)
            {
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();

                Business business = _context.Businesses
                    .Include(b => b.Addresses)
                    .First(b => b.Id == businessId);

                business.Addresses.Add(address);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = businessId });
            }
            ViewBag.BusinessId = businessId;
            return View(address);
        }

        public IActionResult AddNewPerson(int businessId)
        {
            ViewBag.BusinessId = businessId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewPerson([Bind("Id,FirstName,LastName,Email,PhoneNumber")] Person person, int businessId)
        {
            if (ModelState.IsValid)
            {
                _context.Persons.Add(person);
                await _context.SaveChangesAsync();

                Business business = _context.Businesses
                    .Include(b => b.People)
                    .First(b => b.Id == businessId);

                BusinessPerson businessPerson = new() { PersonId = businessId, BusinessId = person.Id };
                person.Businesses.Add(businessPerson);
                business.People.Add(businessPerson);
                _context.BusinessPersons.Add(businessPerson);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = businessId });
            }
            ViewBag.BusinessId = businessId;
            return View(person);
        }

        public async Task<IActionResult> RemoveAddress(int businessId, int addressId)
        {
            Business? business = await _context.Businesses
                .Include(b => b.Addresses)
                .FirstOrDefaultAsync(b => b.Id == businessId);

            Address? address = await _context.Addresses
                .Include(a => a.Business)
                .FirstOrDefaultAsync(a => a.Id == addressId);

            if (business == null || address == null)
            {
                return NotFound();
            }

            business.Addresses.Remove(address);
            address.Business = default;
            address.BusinessId = default;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = businessId });
        }

        public async Task<IActionResult> RemovePerson(int personId, int businessId)
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

            BusinessPerson? businessPerson = business.People.FirstOrDefault(bp => bp.PersonId == personId);
            if (businessPerson != null)
            {
                person.Businesses.Remove(businessPerson);
                business.People.Remove(businessPerson);
                _context.BusinessPersons.Remove(businessPerson);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = businessId });
        }

        private bool BusinessExists(int id)
        {
          return (_context.Businesses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
