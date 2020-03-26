using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Prova.Backend.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configuracoes",
                columns: table => new
                {
                    idConfiguracoes = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    faixaLimite = table.Column<decimal>(type: "decimal(8, 2)", nullable: false),
                    vistos = table.Column<short>(nullable: false),
                    aprovacoes = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracoes", x => x.idConfiguracoes);
                });

            migrationBuilder.CreateTable(
                name: "NotaDeCompra",
                columns: table => new
                {
                    idNotaDeCompra = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dataEmissao = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    valorMercadorias = table.Column<decimal>(type: "decimal(8, 2)", nullable: false),
                    valorDesconto = table.Column<decimal>(type: "decimal(8, 2)", nullable: false),
                    valorFrete = table.Column<decimal>(type: "decimal(8, 2)", nullable: false),
                    valorTotal = table.Column<decimal>(type: "decimal(8, 2)", nullable: false),
                    status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotaDeCompra", x => x.idNotaDeCompra);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    idUsuario = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    login = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    senha = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    papel = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    valorMinimo = table.Column<decimal>(type: "decimal(8, 2)", nullable: false),
                    valorMaximo = table.Column<decimal>(type: "decimal(8, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.idUsuario);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoAprovacoes",
                columns: table => new
                {
                    idHistoricoAprovacoes = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    data = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    usuarioId = table.Column<long>(nullable: false),
                    notaDeCompraId = table.Column<long>(nullable: false),
                    operacao = table.Column<string>(unicode: false, fixedLength: true, maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoAprovacoes", x => x.idHistoricoAprovacoes);
                    table.ForeignKey(
                        name: "FK_HistoricoAprovacoes_NotaDeCompra",
                        column: x => x.notaDeCompraId,
                        principalTable: "NotaDeCompra",
                        principalColumn: "idNotaDeCompra",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistoricoAprovacoes_Usuario",
                        column: x => x.usuarioId,
                        principalTable: "Usuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoAprovacoes_notaDeCompraId",
                table: "HistoricoAprovacoes",
                column: "notaDeCompraId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoAprovacoes_usuarioId",
                table: "HistoricoAprovacoes",
                column: "usuarioId");

            // Scripts
            string sql = @"
            -- CONFIGURACOES
            INSERT INTO [dbo].[Configuracoes]([faixaLimite],[vistos],[aprovacoes]) VALUES (		1000,	1,	0);
            INSERT INTO [dbo].[Configuracoes]([faixaLimite],[vistos],[aprovacoes]) VALUES (	   10000,	1,	1);
            INSERT INTO [dbo].[Configuracoes]([faixaLimite],[vistos],[aprovacoes]) VALUES (	   50000,	2,	1);
            INSERT INTO [dbo].[Configuracoes]([faixaLimite],[vistos],[aprovacoes]) VALUES (999999.99,	2,	2);

            -- USUARIOS

            INSERT INTO [dbo].[Usuario]([nome],[login],[senha],[papel],[valorMinimo],[valorMaximo]) VALUES('UsuarioV01','usuarioV01','usuarioV01','V',  0, 11000);
            INSERT INTO [dbo].[Usuario]([nome],[login],[senha],[papel],[valorMinimo],[valorMaximo]) VALUES('UsuarioA01','usuarioA01','usuarioA01','A',  0, 11000);

            INSERT INTO [dbo].[Usuario]([nome],[login],[senha],[papel],[valorMinimo],[valorMaximo]) VALUES('UsuarioV02','usuarioV02','usuarioV02','V', 1000, 999999.99);
            INSERT INTO [dbo].[Usuario]([nome],[login],[senha],[papel],[valorMinimo],[valorMaximo]) VALUES('UsuarioA02','usuarioA02','usuarioA02','A', 1000, 999999.99);

            -- NOTADECOMPRA

            INSERT INTO [dbo].[NotaDeCompra]([dataEmissao],[valorMercadorias],[valorDesconto],[valorFrete],[valorTotal],[status]) VALUES	(GETDATE(),	  300,		0,	   10,	  310,	0);
            INSERT INTO [dbo].[NotaDeCompra]([dataEmissao],[valorMercadorias],[valorDesconto],[valorFrete],[valorTotal],[status]) VALUES	(GETDATE(),	 7400,	    0,	  100,   7500,	0);
            INSERT INTO [dbo].[NotaDeCompra]([dataEmissao],[valorMercadorias],[valorDesconto],[valorFrete],[valorTotal],[status]) VALUES	(GETDATE(),	12500,	  500,	  200,  12200,	0);
            INSERT INTO [dbo].[NotaDeCompra]([dataEmissao],[valorMercadorias],[valorDesconto],[valorFrete],[valorTotal],[status]) VALUES	(GETDATE(),	800000, 10000,	17000, 807000,	0);

            DECLARE @counter smallint;  
            SET @counter = 1;  

            DECLARE @maxval DECIMAL(8,2), @minval DECIMAL(8,2),
		            @valorMercadorias decimal(8,2),
		            @valorDesconto decimal(8,2),
		            @valorFrete decimal(8,2),
		            @valorTotal decimal(8,2),
		            @percentualDesconto decimal(4,2);

            SELECT @maxval=900000,@minval=500;

            WHILE @counter < 30
            BEGIN   
	            BEGIN TRY  
		 
		            SET @valorMercadorias = (SELECT CAST(((@maxval + 1) - @minval) *  RAND(CHECKSUM(NEWID())) + @minval AS DECIMAL(8,2)));
		            SET @percentualDesconto = (SELECT CAST(((30 + 1) - 0) *  RAND(CHECKSUM(NEWID())) + 0 AS DECIMAL(4,2)));
		            SET @valorFrete = @valorMercadorias * 0.10;
		            SET @valorDesconto = @valorMercadorias * @percentualDesconto / 100;
		            SET @valorTotal = @valorMercadorias + @valorFrete - @valorDesconto;

		            INSERT INTO [dbo].[NotaDeCompra]([dataEmissao],[valorMercadorias],[valorDesconto],[valorFrete],[valorTotal],[status]) 
		            VALUES							(GETDATE(),		@valorMercadorias, @valorDesconto, @valorFrete, @valorTotal, 0);
		            SELECT @valorMercadorias 'merc' ,
			            @valorDesconto 'desconto',
			            @valorFrete 'frete',
			            @valorTotal 'total',
			            @percentualDesconto 'percentual' ;
		            --   SELECT RAND() Random_Number  
			            --SELECT CAST(((@maxval + 1) - @minval) *  RAND(CHECKSUM(NEWID())) + @minval AS DECIMAL(8,2))
	            END TRY  
	            BEGIN CATCH  
     
	            END CATCH  

	            SET @counter = @counter + 1  
            END;";
            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuracoes");

            migrationBuilder.DropTable(
                name: "HistoricoAprovacoes");

            migrationBuilder.DropTable(
                name: "NotaDeCompra");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
