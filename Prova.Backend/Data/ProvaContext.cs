using Microsoft.EntityFrameworkCore;
using Prova.Poco;

namespace Prova.Data
{
    public partial class ProvaContext : DbContext
    {
        public ProvaContext()
        {
        }

        public ProvaContext(DbContextOptions<ProvaContext> options)
            : base(options)
        {
            this.Database.Migrate();
        }

        public virtual DbSet<Configuracoes> Configuracoes { get; set; }
        public virtual DbSet<HistoricoAprovacoes> HistoricoAprovacoes { get; set; }
        public virtual DbSet<NotaDeCompra> NotaDeCompra { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Configuracoes>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("idConfiguracoes");

                entity.Property(e => e.Aprovacoes).HasColumnName("aprovacoes");

                entity.Property(e => e.FaixaLimite)
                    .HasColumnName("faixaLimite")
                    .HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Vistos).HasColumnName("vistos");
            });

            modelBuilder.Entity<HistoricoAprovacoes>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("idHistoricoAprovacoes");

                entity.Property(e => e.Data)
                    .HasColumnName("data")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.NotaDeCompraId).HasColumnName("notaDeCompraId");

                entity.Property(e => e.Operacao)
                    .IsRequired()
                    .HasColumnName("operacao")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");

                entity.HasOne(d => d.NotaDeCompra)
                    .WithMany(p => p.HistoricoAprovacoes)
                    .HasForeignKey(d => d.NotaDeCompraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HistoricoAprovacoes_NotaDeCompra");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.HistoricoAprovacoes)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HistoricoAprovacoes_Usuario");
            });

            modelBuilder.Entity<NotaDeCompra>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("idNotaDeCompra");

                entity.Property(e => e.DataEmissao)
                    .HasColumnName("dataEmissao")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.ValorDesconto)
                    .HasColumnName("valorDesconto")
                    .HasColumnType("decimal(8, 2)");

                entity.Property(e => e.ValorFrete)
                    .HasColumnName("valorFrete")
                    .HasColumnType("decimal(8, 2)");

                entity.Property(e => e.ValorMercadorias)
                    .HasColumnName("valorMercadorias")
                    .HasColumnType("decimal(8, 2)");

                entity.Property(e => e.ValorTotal)
                    .HasColumnName("valorTotal")
                    .HasColumnType("decimal(8, 2)");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("idUsuario");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Papel)
                    .IsRequired()
                    .HasColumnName("papel")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Senha)
                    .IsRequired()
                    .HasColumnName("senha")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ValorMaximo)
                    .HasColumnName("valorMaximo")
                    .HasColumnType("decimal(8, 2)");

                entity.Property(e => e.ValorMinimo)
                    .HasColumnName("valorMinimo")
                    .HasColumnType("decimal(8, 2)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}