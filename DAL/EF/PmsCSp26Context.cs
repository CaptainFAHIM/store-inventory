using System;
using System.Collections.Generic;
using DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public partial class PmsCSp26Context : DbContext
{
    public PmsCSp26Context()
    {
    }

    public PmsCSp26Context(DbContextOptions<PmsCSp26Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

    public virtual DbSet<StockMovement> StockMovements { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DbConn");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.CidNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Cid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Categories");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.Property(e => e.OrderNumber)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.Property(e => e.SupplierName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasMany(d => d.Items).WithOne(p => p.PurchaseOrder)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PurchaseOrderItems_PurchaseOrders");

            entity.HasMany(d => d.StockMovements).WithOne(p => p.PurchaseOrder)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_StockMovements_PurchaseOrders");
        });

        modelBuilder.Entity<PurchaseOrderItem>(entity =>
        {
            entity.Property(e => e.ProductName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_PurchaseOrderItems_Products");
        });

        modelBuilder.Entity<StockMovement>(entity =>
        {
            entity.Property(e => e.ProductName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.PurchaseOrderNumber)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.Property(e => e.MovementType)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Notes)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_StockMovements_Products");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.StockMovements)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_StockMovements_PurchaseOrders");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
