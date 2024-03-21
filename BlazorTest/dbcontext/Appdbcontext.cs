using BlazorTest.Models;
using Microsoft.EntityFrameworkCore;


namespace Blazortest.dbcontext
{
    public class Appdbcontext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite(@"Data Source=Database\Demo.db");

        public DbSet<Chatlogs> chatlogs { get; set; }

    }
}
