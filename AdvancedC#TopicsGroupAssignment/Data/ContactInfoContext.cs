using Microsoft.EntityFrameworkCore;
namespace AdvancedC_TopicsGroupAssignment.Data
{
    public class ContactInfoContext : DbContext
    {
        public ContactInfoContext(DbContextOptions<ContactInfoContext> options) : base(options) { }


    }
}
