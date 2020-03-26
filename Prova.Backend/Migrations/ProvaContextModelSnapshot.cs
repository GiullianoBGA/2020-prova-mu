﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Prova.Data;

namespace Prova.Backend.Migrations
{
    [DbContext(typeof(ProvaContext))]
    partial class ProvaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Prova.Poco.Configuracoes", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("idConfiguracoes")
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<short>("Aprovacoes")
                        .HasColumnName("aprovacoes")
                        .HasColumnType("smallint");

                    b.Property<decimal>("FaixaLimite")
                        .HasColumnName("faixaLimite")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<short>("Vistos")
                        .HasColumnName("vistos")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.ToTable("Configuracoes");
                });

            modelBuilder.Entity("Prova.Poco.HistoricoAprovacoes", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("idHistoricoAprovacoes")
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Data")
                        .HasColumnName("data")
                        .HasColumnType("datetime2(0)");

                    b.Property<long>("NotaDeCompraId")
                        .HasColumnName("notaDeCompraId")
                        .HasColumnType("bigint");

                    b.Property<string>("Operacao")
                        .IsRequired()
                        .HasColumnName("operacao")
                        .HasColumnType("char(1)")
                        .IsFixedLength(true)
                        .HasMaxLength(1)
                        .IsUnicode(false);

                    b.Property<long>("UsuarioId")
                        .HasColumnName("usuarioId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("NotaDeCompraId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("HistoricoAprovacoes");
                });

            modelBuilder.Entity("Prova.Poco.NotaDeCompra", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("idNotaDeCompra")
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DataEmissao")
                        .HasColumnName("dataEmissao")
                        .HasColumnType("datetime2(0)");

                    b.Property<bool>("Status")
                        .HasColumnName("status")
                        .HasColumnType("bit");

                    b.Property<decimal>("ValorDesconto")
                        .HasColumnName("valorDesconto")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<decimal>("ValorFrete")
                        .HasColumnName("valorFrete")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<decimal>("ValorMercadorias")
                        .HasColumnName("valorMercadorias")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnName("valorTotal")
                        .HasColumnType("decimal(8, 2)");

                    b.HasKey("Id");

                    b.ToTable("NotaDeCompra");
                });

            modelBuilder.Entity("Prova.Poco.Usuario", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("idUsuario")
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnName("login")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnName("nome")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Papel")
                        .IsRequired()
                        .HasColumnName("papel")
                        .HasColumnType("char(1)")
                        .IsFixedLength(true)
                        .HasMaxLength(1)
                        .IsUnicode(false);

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnName("senha")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<decimal>("ValorMaximo")
                        .HasColumnName("valorMaximo")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<decimal>("ValorMinimo")
                        .HasColumnName("valorMinimo")
                        .HasColumnType("decimal(8, 2)");

                    b.HasKey("Id");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("Prova.Poco.HistoricoAprovacoes", b =>
                {
                    b.HasOne("Prova.Poco.NotaDeCompra", "NotaDeCompra")
                        .WithMany("HistoricoAprovacoes")
                        .HasForeignKey("NotaDeCompraId")
                        .HasConstraintName("FK_HistoricoAprovacoes_NotaDeCompra")
                        .IsRequired();

                    b.HasOne("Prova.Poco.Usuario", "Usuario")
                        .WithMany("HistoricoAprovacoes")
                        .HasForeignKey("UsuarioId")
                        .HasConstraintName("FK_HistoricoAprovacoes_Usuario")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
