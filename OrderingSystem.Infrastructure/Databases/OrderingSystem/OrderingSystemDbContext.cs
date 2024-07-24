using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace OrderingSystem.Infrastructure.Databases.OrderingSystem
{
    public class OrderingSystemDbContext: IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
    {
    
        public OrderingSystemDbContext(DbContextOptions<OrderingSystemDbContext> options)
        : base(options)
        {
        }
        protected virtual DbSet<TblBase> TblBase {  get; set; }
        protected virtual DbSet<TblBaseSoftDelete> TblBaseSoftDelete {  get; set; }
        public virtual DbSet<TblMenu> TblMenu { get; set; }
        public virtual DbSet<TblOrder> TblOrder { get; set; }
        public virtual DbSet<TblReceipt> TblPayment { get; set; }
        public virtual DbSet<TblTable> TblTable { get; set; }
        public virtual DbSet<TblMenuGroup> TblMenuGroup { get; set; }
        public virtual DbSet<TblOrderToReceipt> TblOrderToReceipt { get; set; }
        public virtual DbSet<TblAuditTrail> TblAuditTrail { get; set; }
        public virtual DbSet<TblFile> TblFile { get; set; }
        public virtual DbSet<TblMenuImage> TblMenuImage { get; set; }
        public virtual DbSet<TblReceipt> TblReceipt { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //table that implement softdelete, will only include non-flag isdeleted in query result
            modelBuilder.Entity<TblBaseSoftDelete>().HasQueryFilter(x => !x.IsDeleted);

            //map enum
            modelBuilder.HasPostgresEnum<MenuType>();
            modelBuilder.HasPostgresEnum<PaymentType>();
            modelBuilder.HasPostgresEnum<EntityState>();
            modelBuilder.HasPostgresEnum<OrderStatus>();
            modelBuilder.HasPostgresEnum<MenuStatus>();

            modelBuilder.Entity<TblBase>(entity =>
            {
                //entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.UseTpcMappingStrategy();
            });
            modelBuilder.Entity<TblBaseSoftDelete>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.IsDeleted);
                entity.UseTpcMappingStrategy();
            });

            modelBuilder.Entity<TblMenuGroup>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(1000);
            });
            modelBuilder.Entity<TblMenu>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.HasOne( e=> e.MenuGroup).WithMany(e => e.Menus).HasForeignKey(e => e.MenuGroupId)
                .OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<TblOrder>(entity =>
            {
                entity.Property(e => e.Note).HasMaxLength(200);
                entity.HasOne(e => e.Menu).WithMany(e => e.Orders)
                .HasForeignKey(e => e.MenuId)
                .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.OrderToReceipt).WithOne(e => e.Order)
                .HasForeignKey<TblOrderToReceipt>(e => e.OrderId);
                entity.HasOne(e => e.Table).WithMany(e => e.Orders)
                .HasForeignKey(e => e.TableId)
                .OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<TblReceipt>(entity =>
            {
                entity.HasMany(e => e.OrderToReceipts).WithOne(e => e.Receipt)
                .HasForeignKey(e => e.ReceiptId)
                .OnDelete(DeleteBehavior.NoAction);
                entity.Property(e => e.TransactionId).HasMaxLength(50);
            });
            modelBuilder.Entity<TblAuditTrail>(entity =>
            {
                entity.Property(e => e.TableName).HasMaxLength(100);
                entity.Property(e => e.Data).HasMaxLength(int.MaxValue);
            });
            modelBuilder.Entity<TblFile>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Extension).HasMaxLength(10);
                entity.Property(e => e.ContentType).HasMaxLength(30);
                entity.Property(e => e.Data).HasMaxLength(10000000);
            });
            modelBuilder.Entity<TblMenuImage>(entity =>
            {
                entity.HasOne(e => e.Menu).WithMany(e => e.MenuImages)
                .HasForeignKey(e => e.MenuId)
                .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Image).WithMany(e => e.MenuImages)
                .HasForeignKey(e => e.FileId)
                .OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<TblTable>(entity =>
            {
                entity.Property(e => e.Number).HasMaxLength(10);
            });
        }
    }
}
