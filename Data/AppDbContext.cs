using Microsoft.EntityFrameworkCore;
using TimeDepositAPI.Models;

namespace TimeDepositAPI.Data
{
    public class AppDbContext : DbContext
    {
        
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        // Define DbSets for each model/table
        public DbSet<User> Users { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<CrowdDepositOffer> CrowdDepositOffers { get; set; }
    }
}
