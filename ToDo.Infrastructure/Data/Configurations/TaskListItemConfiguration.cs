using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Domain.Entities.TaskLists;

namespace ToDo.Infrastructure.Data.Configurations
{
    public class TaskListItemConfiguration : IEntityTypeConfiguration<TaskListItem>
    {
        public void Configure(EntityTypeBuilder<TaskListItem> builder)
        {
            builder.ToTable("TaskListItem");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(t => t.IsCompleted)
                .HasDefaultValueSql("0")
                .IsRequired();

            builder.Property(t => t.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.Property(t => t.DueDate)
                .IsRequired();

            builder.HasOne(t => t.TaskList)
                .WithMany(t => t.Tasks)
                .HasForeignKey(t => t.TaskListId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
