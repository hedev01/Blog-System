using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using BlogSystem.Domian.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().ToTable("Post");
            modelBuilder.Entity<Tag>().ToTable("Tag");

            modelBuilder.Entity<Post>()
                .Property(p => p.Title)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<Post>()
                .HasMany<Tag>()
                .WithMany()
                .UsingEntity(j => j.ToTable("PostTag"));
            base.OnModelCreating(modelBuilder);
        }
    }
}
