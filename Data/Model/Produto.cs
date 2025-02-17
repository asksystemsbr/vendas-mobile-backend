using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ControlStoreAPI.Data.Interface;

namespace ControlStoreAPI.Data.Model
{
    [Table("produto")]
    public class Produto : IIdentifiable
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("ID_UNIDADE_PRODUTO")]
        public int? UnidadeProdutoId { get; set; }

        [Column("GTIN")]
        public string? Gtin { get; set; }

        [Column("CODIGO_INTERNO")]
        public string? CodigoInterno { get; set; }

        [Column("NOME")]
        public string? Nome { get; set; }

        [Column("DESCRICAO")]
        public string? Descricao { get; set; }

        [Column("DESCRICAO_PDV")]
        public string? DescricaoPdv { get; set; }

        [Column("VALOR_VENDA", TypeName = "decimal(10, 3)")]
        public decimal? ValorVenda { get; set; }

        [Column("QTD_ESTOQUE", TypeName = "decimal(10, 3)")]
        public decimal? QuantidadeEstoque { get; set; }

        [Column("QTD_ESTOQUE_ANTERIOR", TypeName = "decimal(10, 3)")]
        public decimal? QuantidadeEstoqueAnterior { get; set; }

        [Column("ESTOQUE_MIN", TypeName = "decimal(10, 3)")]
        public decimal? EstoqueMin { get; set; }

        [Column("ESTOQUE_MAX", TypeName = "decimal(10, 3)")]
        public decimal? EstoqueMax { get; set; }

        [Column("IAT")]
        public string? Iat { get; set; }

        [Column("IPPT")]
        public string? Ippt { get; set; }

        [Column("NCM")]
        public string? Ncm { get; set; }

        [Column("TIPO_ITEM_SPED")]
        public string? TipoItemSped { get; set; }

        [Column("DATA_ESTOQUE")]
        public DateTime? DataEstoque { get; set; }

        [Column("HORA_ESTOQUE")]
        public string? HoraEstoque { get; set; }

        [Column("TAXA_IPI", TypeName = "decimal(10, 2)")]
        public decimal? TaxaIpi { get; set; }

        [Column("TAXA_ISSQN", TypeName = "decimal(10, 2)")]
        public decimal? TaxaIssqn { get; set; }

        [Column("TAXA_PIS", TypeName = "decimal(10, 2)")]
        public decimal? TaxaPis { get; set; }

        [Column("TAXA_COFINS", TypeName = "decimal(10, 2)")]
        public decimal? TaxaCofins { get; set; }

        [Column("TAXA_ICMS", TypeName = "decimal(10, 2)")]
        public decimal? TaxaIcms { get; set; }

        [Column("CST")]
        public string? Cst { get; set; }

        [Column("CSOSN")]
        public string? Csosn { get; set; }

        [Column("TOTALIZADOR_PARCIAL")]
        public string? TotalizadorParcial { get; set; }

        [Column("ECF_ICMS_ST")]
        public string? EcfIcmsSt { get; set; }

        [Column("CODIGO_BALANCA")]
        public int? CodigoBalanca { get; set; }

        [Column("PAF_P_ST")]
        public string? PafPSt { get; set; }

        [Column("HASH_TRIPA")]
        public string? HashTripa { get; set; }

        [Column("HASH_INCREMENTO")]
        public string? HashIncremento { get; set; }

        [Column("DESATIVADO")]
        public bool? Desativado { get; set; }

        [Column("COZINHA")]
        public bool? Cozinha { get; set; }

        [Column("ID_CATEGORIA_PRODUTO")]
        public int? CategoriaProdutoId { get; set; }

        [Column("LUCRO", TypeName = "decimal(10, 2)")]
        public decimal? Lucro { get; set; }

        [Column("VALOR_COMPRA", TypeName = "decimal(10, 2)")]
        public decimal? ValorCompra { get; set; }

        [Column("GRUPO_ID")]
        public int? GrupoId { get; set; }

        [Column("SUBGRUPO_ID")]
        public int? SubgrupoId { get; set; }

        [Column("VALOR_PROMOCAO", TypeName = "decimal(10, 2)")]
        public decimal? ValorPromocao { get; set; }

        [Column("SERVICO")]
        public bool? Servico { get; set; }

        [Column("FORNECEDOR_ID")]
        public int? FornecedorId { get; set; }

        [Column("CFOP")]
        public int? Cfop { get; set; }

        [Column("ATACADO_ACIMADE", TypeName = "decimal(10, 2)")]
        public decimal? AtacadoAcimade { get; set; }

        [Column("ATACADO_VALOR_VENDA", TypeName = "decimal(10, 2)")]
        public decimal? AtacadoValorVenda { get; set; }

        [Column("CEST")]
        public string? Cest { get; set; }

        [Column("DATA_VENCIMENTO")]
        public DateTime? DataVencimento { get; set; }

        [Column("CSTIPI")]
        public string? CstIpi { get; set; }

        [Column("CSTCOFINS")]
        public string? CstCofins { get; set; }

        [Column("CFOP_SAIDA")]
        public int? CfopSaida { get; set; }

        [Column("QTD_ESTOQUE_SNOTA", TypeName = "decimal(10, 2)")]
        public decimal? QuantidadeEstoqueSemNota { get; set; }


        public string? Foto { get; set; }
    }
}
