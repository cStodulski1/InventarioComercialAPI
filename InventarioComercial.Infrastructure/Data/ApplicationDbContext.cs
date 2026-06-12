using InventarioComercial.Domain.Categorias;
using InventarioComercial.Domain.Produtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("public");
            modelBuilder.UseIdentityColumns();

            modelBuilder.Entity<Categoria>(static entity =>
            {
                entity.ToTable("categorias");
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Id)
                    .HasDefaultValueSql("gen_random_uuid()") 
                    .ValueGeneratedOnAdd();

                entity.Property(c => c.Nome)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasAnnotation("MinLength", 5);

                entity.Property(c => c.Descricao)
                    .HasMaxLength(500);

                entity.HasMany(c => c.Produtos)
                    .WithOne(p => p.Categoria)
                    .HasForeignKey(p => p.CategoriaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Produto>(static entity =>
            {
                entity.ToTable("produtos");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .ValueGeneratedOnAdd();

                entity.Property(p => p.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(p => p.Descricao)
                    .HasMaxLength(500);

                entity.Property(p => p.Preco)
                    .HasPrecision(18, 2)
                    .IsRequired();

                entity.Property(p => p.CategoriaId)
                    .IsRequired();

                entity.HasIndex(p => p.CategoriaId)
                    .HasDatabaseName("idx_produtos_categorias_id");

                entity.HasOne(p => p.Categoria)
                    .WithMany(c => c.Produtos)
                    .HasForeignKey(p => p.CategoriaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
