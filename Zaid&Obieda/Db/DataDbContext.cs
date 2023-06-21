using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zaid_Obieda.Models;

namespace Zaid_Obieda.Db
{
    public class DataDbContext: IdentityDbContext<AppUser>
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
    }
}
