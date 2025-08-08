using Microsoft.EntityFrameworkCore;
using SecureNotesAPI.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SecureNotesAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(builder =>
            {
                builder.HasKey(u => u.Id);

                builder.Property(u => u.Id)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Note>(builder =>
            {
                builder.HasKey(n => n.Id);

                builder.Property(n => n.Id)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .ValueGeneratedOnAdd();
            });
        }
    }
}

