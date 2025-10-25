using Microsoft.EntityFrameworkCore;
using POS.Models;

namespace POS.Data
{
    public class WebApplicationContext : DbContext
    {
        public WebApplicationContext(DbContextOptions<WebApplicationContext> options) : base(options)
        {
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products {  get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>().HasIndex(p=>p.Name).IsUnique();
            modelBuilder.Entity<Invoice>().HasIndex(i => i.Number).IsUnique();
            modelBuilder.Entity<Client>().HasIndex(c => c.IdNumber).IsUnique();
            base.OnModelCreating(modelBuilder);
        }
        

        }
    }
    

