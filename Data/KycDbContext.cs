using Microsoft.EntityFrameworkCore;
using moneygram_api.Models;

namespace moneygram_api.Data
{
    public class KycDbContext : DbContext
    {
        public KycDbContext(DbContextOptions<KycDbContext> options)
            : base(options)
        {
        }

        public DbSet<Clientele> tblClientele { get; set; }
    }
}