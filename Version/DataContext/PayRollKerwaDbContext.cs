using Microsoft.EntityFrameworkCore;
using Version.EntityModels;

namespace Version.DataContext
{
    public class PayRollKerwaDbContext : DbContext
    {
        public PayRollKerwaDbContext(DbContextOptions<PayRollKerwaDbContext> options) : base(options)
        {
        }

        public DbSet<KerwaEmployee> KerwaEmployee { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string con = @"Server=DESKTOP-90QKA0D\\SQLEXPRESS;Database=PayRollKerwa;User Id=sa;Password=Password@1234;Encrypt=False";
            //optionsBuilder.UseSqlServer("ConnectionString");
            //optionsBuilder.UseSqlServer("DefaultConnection");
            //optionsBuilder.UseSqlServer(con);

        }
    }
}
