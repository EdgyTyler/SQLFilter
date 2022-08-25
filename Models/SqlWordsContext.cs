using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SQLFilter.Models
{
    public partial class SqlWordsContext : DbContext
    {
  
        public SqlWordsContext(DbContextOptions<SqlWordsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ProfanityFilter> ProfanityFilters { get; set; } = null!;
     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProfanityFilter>(entity =>
            {
                entity.ToTable("ProfanityFilter");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.BadWord).HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
