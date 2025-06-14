using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DeThi.Models
{
    //Scaffold-DbContext "" Microsoft.EntityFrameWorkCore.SqlServer -OutputDir Models
    public partial class QLCongNhanContext : DbContext
    {
        public QLCongNhanContext()
        {
        }

        public QLCongNhanContext(DbContextOptions<QLCongNhanContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CongNhan> CongNhans { get; set; } = null!;
        public virtual DbSet<Phong> Phongs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=Huyen;Initial Catalog=QLCongNhan;Integrated Security=True;Trust Server Certificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CongNhan>(entity =>
            {
                entity.HasKey(e => e.MaCongNhan)
                    .HasName("PK__CongNhan__3DD895CA56EF07BD");

                entity.ToTable("CongNhan");

                entity.Property(e => e.MaCongNhan)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.HoTen).HasMaxLength(50);

                entity.Property(e => e.MaPhong)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.MaPhongNavigation)
                    .WithMany(p => p.CongNhans)
                    .HasForeignKey(d => d.MaPhong)
                    .HasConstraintName("fk_S_TG");
            });

            modelBuilder.Entity<Phong>(entity =>
            {
                entity.HasKey(e => e.MaPhong)
                    .HasName("PK__Phong__20BD5E5B113D5F36");

                entity.ToTable("Phong");

                entity.Property(e => e.MaPhong)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.TenPhong).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
