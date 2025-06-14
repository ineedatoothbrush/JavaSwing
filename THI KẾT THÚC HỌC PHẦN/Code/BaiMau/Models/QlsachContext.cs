using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BaiMau.Models;

public partial class QlsachContext : DbContext
{
    //Scaffold-DbContext "" Microsoft.EntityFrameWorkCore.SqlServer -OutputDir Models
    public QlsachContext()
    {
    }

    public QlsachContext(DbContextOptions<QlsachContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Sach> Saches { get; set; }

    public virtual DbSet<TacGia> TacGia { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=Huyen;Initial Catalog=QLSach;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sach>(entity =>
        {
            entity.HasKey(e => e.MaSach).HasName("PK__Sach__B235742D6FE772DD");

            entity.ToTable("Sach");

            entity.Property(e => e.MaSach)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.MaTg)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MaTG");
            entity.Property(e => e.TenSach).HasMaxLength(50);

            entity.HasOne(d => d.TacGia).WithMany(p => p.Saches)
                .HasForeignKey(d => d.MaTg)
                .HasConstraintName("fk_S_TG");
        });

        modelBuilder.Entity<TacGia>(entity =>
        {
            entity.HasKey(e => e.MaTg).HasName("PK__TacGia__2725007494B90DDD");

            entity.Property(e => e.MaTg)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MaTG");
            entity.Property(e => e.TenTacGia).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
