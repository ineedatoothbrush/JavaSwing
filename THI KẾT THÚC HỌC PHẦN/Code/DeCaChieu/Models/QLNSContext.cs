using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DeCaChieu.Models
{
    public partial class QLNSContext : DbContext
    {
        //Scaffold-DbContext "" Microsoft.EntityFrameWorkCore.SqlServer -OutputDir Models
        public QLNSContext()
        {
        }

        public QLNSContext(DbContextOptions<QLNSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<NhanVien> NhanViens { get; set; } = null!;
        public virtual DbSet<PhongBan> PhongBans { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=Huyen;Initial Catalog=QLNS;Integrated Security=True;Trust Server Certificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasKey(e => e.MaNhanVien)
                    .HasName("PK__NhanVien__77B2CA472EAD8DD1");

                entity.ToTable("NhanVien");

                entity.Property(e => e.MaNhanVien)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.DenNgay).HasColumnType("date");

                entity.Property(e => e.MaPhongBan)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.TuNgay).HasColumnType("date");

                entity.HasOne(d => d.MaPhongBanNavigation)
                    .WithMany(p => p.NhanViens)
                    .HasForeignKey(d => d.MaPhongBan)
                    .HasConstraintName("fk_S_TG");
            });

            modelBuilder.Entity<PhongBan>(entity =>
            {
                entity.HasKey(e => e.MaPhongBan)
                    .HasName("PK__PhongBan__D0910CC853FA1208");

                entity.ToTable("PhongBan");

                entity.Property(e => e.MaPhongBan)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.TenPhongBan).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
