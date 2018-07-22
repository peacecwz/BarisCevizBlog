using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySQL.Data.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.EF
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base()
        {

        }

        public DbSet<Tables.Post> Posts { get; set; }
        public DbSet<Tables.Category> Categories { get; set; }
        public DbSet<Tables.Author> Authors { get; set; }
        public DbSet<Tables.IoTHub> Hubs { get; set; }
        public DbSet<Tables.StepInsight> StepInsights { get; set; }
        public DbSet<Tables.Page> Pages { get; set; }
        public DbSet<Tables.Event> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conStr = Startup.WebConfiguration.GetConnectionString("DbConnection");
#if DEBUG
            conStr = Startup.WebConfiguration.GetConnectionString("DbConnection-dev");
#endif
            optionsBuilder.UseSqlServer(conStr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tables.Post>()
            .HasOne(p => p.Author)
            .WithMany(b => b.Posts)
            .HasForeignKey(p => p.AuthorId);

            /*modelBuilder.Entity<Tables.Post>()
            .HasMany(p => p.PostToCategories)
            .WithOne(b => b.Post)
            .HasForeignKey(p => p.CategoryId);*/

            /*modelBuilder.Entity<Tables.Category>()
            .HasMany(p => p.PostsToCategory)
            .WithMany(b => b)
            .HasForeignKey(p => p.PostId);*/
            
        }
    }
}
