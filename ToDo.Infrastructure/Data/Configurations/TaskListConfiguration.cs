using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Domain.Entities.TaskLists;
using ToDo.Infrastructure.Data.Identity.Entities;

namespace ToDo.Infrastructure.Data.Configurations
{
    public class TaskListConfiguration : IEntityTypeConfiguration<TaskList>
    {
        public void Configure(EntityTypeBuilder<TaskList> builder)
        {
            builder.ToTable("TaskList");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.UserId)
                .IsRequired();

            builder.Property(t => t.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.HasMany(t => t.Tasks)
                .WithOne(t => t.TaskList)
                .HasForeignKey(t => t.TaskListId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ToDoUser>()
                .WithMany()
                .HasForeignKey(t => t.UserId);
        }
    }
}
