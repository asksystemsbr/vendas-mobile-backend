using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System;
using ControlStoreAPI.Data.Model;
using ControlStoreAPI.Models;

namespace ControlStoreAPI.Data
{
    public class APIDbContext : DbContext
    {
        // Define um DbSet para a entidade Cliente
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Composicao> Composicao { get; set; }
        public DbSet<ComposicaoToProduto> ComposicaoToProduto { get; set; }        
        public DbSet<GrupoProduto> GrupoProduto { get; set; }
        public DbSet<GrupoUsuario> GrupoUsuario { get; set; }
        public DbSet<ListaPrecoCabecalho> ListaPrecoCabecalho { get; set; }
        public DbSet<ListaPrecoDetalhe> ListaPrecoDetalhe { get; set; }
        public DbSet<Modulo> Modulo { get; set; }
        public DbSet<OperationLog> OperationLogs { get; set; }
        public DbSet<PedidoCabecalho> PedidoCabecalho { get; set; }
        public DbSet<PedidoDetalhe> PedidoDetalhe { get; set; }
        public DbSet<Permissao> Permissao { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<StatusCliente> StatusCliente { get; set; }
        public DbSet<TipoPermissao> TipoPermissao { get; set; }
        public DbSet<Usuario> Usuario { get; set; }




        // Construtor que recebe opções para configurar o DbContext
        public APIDbContext(DbContextOptions<APIDbContext> options) : base(options)
        {
        }

        // Método opcional para configurar o modelo de dados (mapeamento de entidades)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Cliente>()
            //.Property(c => c.LIMITE_SALDO)
            //.HasColumnType("decimal(10,2)");

            //modelBuilder.Entity<Cliente>()
            //    .Property(c => c.VALOR_FIXO)
            //    .HasColumnType("decimal(10,6)");

            //base.OnModelCreating(modelBuilder);

            //// Aqui você pode configurar o mapeamento da entidade Cliente, se necessário
            ////modelBuilder.Entity<Cliente>(entity =>
            ////{
            ////    // Define a tabela associada à entidade Cliente (se não seguir o padrão de nome)
            ////    entity.ToTable("cliente");

            ////    // Define a chave primária
            ////    entity.HasKey(e => e.ID);

            ////    // Exemplo de configuração de propriedades individuais
            ////    // Caso as propriedades tenham nomes diferentes das colunas da tabela,
            ////    // você pode configurar o mapeamento aqui:
            ////    // entity.Property(e => e.Nome).HasColumnName("NOME");
            ////    // entity.Property(e => e.DataCadastro).HasColumnName("DATA_CADASTRO");
            ////    // ... etc.
            ////});
            //// Aqui você pode configurar outras entidades, relações, índices, etc.
            ////modelBuilder.Entity<Imovel>(entity =>
            ////{
            ////    //entity.ToTable("imovel");

            ////    //entity.HasOne(imovel => imovel.Cliente)
            ////    //    .WithMany()
            ////    //    .HasForeignKey(imovel => imovel.ClienteId)
            ////    //    .OnDelete(DeleteBehavior.ClientSetNull) // Configure como necessário
            ////    //    .HasConstraintName("FK_Imovel_Cliente");

            ////    //entity.HasOne(imovel => imovel.SituacaoCliente)
            ////    //    .WithMany()
            ////    //    .HasForeignKey(imovel => imovel.SituacaoId)
            ////    //    .OnDelete(DeleteBehavior.ClientSetNull) // Configure como necessário
            ////    //    .HasConstraintName("FK_Imovel_SituacaoCliente");
            ////});
        }

        // Método opcional para configurar o modelo de dados (mapeamento de entidades)

    }
}
