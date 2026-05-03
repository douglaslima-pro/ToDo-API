using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDo.Infrastructure.Data.Configurations;
using ToDo.Infrastructure.Data.Identity.Entities;

namespace ToDo.Infrastructure.Data.Contexts
{
    public class ToDoDBContext : IdentityDbContext<ToDoUser, IdentityRole<int>, int>
    {
        public DbSet<ToDoUser> User { get; set; }

        public ToDoDBContext(DbContextOptions<ToDoDBContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ToDoUserConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
