using GreetingService.Core;
using GreetingService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure
{
    public class GreetingDbContext: DbContext
    {
        public DbSet<Greeting> Greetings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        public GreetingDbContext()
        {
        }
        public GreetingDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("GreetingDbConnectionString"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Tell EF Core that the primary key of User table is email
            modelBuilder.Entity<User>()
                .HasKey(c => c.Email);

            modelBuilder.Entity<Greeting>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.From)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Greeting>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.To)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientCascade);
        }


    }
}
