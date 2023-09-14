using AdvancedC_TopicsGroupAssignment.Models;
using Microsoft.EntityFrameworkCore;
namespace AdvancedC_TopicsGroupAssignment.Data
{
    public class ContactInfoContext : DbContext
    {
        public ContactInfoContext(DbContextOptions<ContactInfoContext> options) : base(options) { }

        public DbSet<Person> Persons { get; set; } = default!;
        public DbSet<Address> Addresses { get; set; } = default!;
        public DbSet<Business> Businesses { get; set; } = default!;
    }
}
