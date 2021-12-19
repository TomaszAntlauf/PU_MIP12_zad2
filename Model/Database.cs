using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Model
{
    public class Database : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<AuthorRate> AuthorRates { get; set; }
        public DbSet<BookRate> BookRates { get; set; }

        public Database()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=C:\\Users\\tomaa\\source\\repos\\PU-MIP12-zad2\\PU-MIP12-zad2\\database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rate>().HasDiscriminator(x => x.RateType)
                .HasValue<AuthorRate>(RateType.AuthorRate)
                .HasValue<BookRate>(RateType.BookRate);
        }
    }
}
