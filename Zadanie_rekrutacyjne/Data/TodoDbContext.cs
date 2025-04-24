using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

        public DbSet<ToDo> todos => Set<ToDo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDo>(entity =>
            {
                entity.ToTable("todos");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Title).HasColumnName("title");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Expiry).HasColumnName("expiry");
                entity.Property(e => e.PercentComplete).HasColumnName("percentcomplete");
            });
        }
    }
   
}
