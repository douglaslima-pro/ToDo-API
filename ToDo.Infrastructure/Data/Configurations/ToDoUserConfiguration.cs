using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Infrastructure.Data.Identity.Entities;

namespace ToDo.Infrastructure.Data.Configurations
{
    public class ToDoUserConfiguration : IEntityTypeConfiguration<ToDoUser>
    {
        public void Configure(EntityTypeBuilder<ToDoUser> builder)
        {
            builder.ToTable("ToDoUsers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName)
                .HasColumnName("FirstName")
                .HasColumnType("nvarchar(256)");

            builder.Property(x => x.LastName)
                .HasColumnName("LastName")
                .HasColumnType("nvarchar(256)");

            builder.Property(x => x.Bio)
                .HasColumnName("Bio")
                .HasColumnType("text");
        }
    }
}
