using ControlStoreAPI.Data.Interface;

namespace ControlStoreAPI.Data.Model
{

    public class ProdutoDTO
    {
        public int ID { get; set; }

        public int? UnidadeProdutoId { get; set; }

        public string? Gtin { get; set; }

        public string? CodigoInterno { get; set; }

        public string? Nome { get; set; }

        public string? Descricao { get; set; }

        public string? DescricaoPdv { get; set; }

        public decimal? ValorVenda { get; set; }

        public decimal? QuantidadeEstoque { get; set; }

        public decimal? QuantidadeEstoqueAnterior { get; set; }

        public decimal? EstoqueMin { get; set; }

        public decimal? EstoqueMax { get; set; }

        public string? Iat { get; set; }

        public string? Ippt { get; set; }

        public string? Ncm { get; set; }

        public string? TipoItemSped { get; set; }

        public DateTime? DataEstoque { get; set; }

        public string? HoraEstoque { get; set; }

        public decimal? TaxaIpi { get; set; }

        public decimal? TaxaIssqn { get; set; }

        public decimal? TaxaPis { get; set; }

        public decimal? TaxaCofins { get; set; }

        public decimal? TaxaIcms { get; set; }

        public string? Cst { get; set; }

        public string? Csosn { get; set; }

        public string? TotalizadorParcial { get; set; }

        public string? EcfIcmsSt { get; set; }

        public int? CodigoBalanca { get; set; }

        public string? PafPSt { get; set; }

        public string? HashTripa { get; set; }

        public string? HashIncremento { get; set; }

        public bool? Desativado { get; set; }

        public bool? Cozinha { get; set; }

        public int? CategoriaProdutoId { get; set; }

        public decimal? Lucro { get; set; }

        public decimal? ValorCompra { get; set; }

        public int? GrupoId { get; set; }

        public int? SubgrupoId { get; set; }

        public decimal? ValorPromocao { get; set; }

        public bool? Servico { get; set; }

        public int? FornecedorId { get; set; }

        public int? Cfop { get; set; }

        public decimal? AtacadoAcimade { get; set; }

        public decimal? AtacadoValorVenda { get; set; }

        public string? Cest { get; set; }

        public DateTime? DataVencimento { get; set; }

        public string? CstIpi { get; set; }

        public string? CstCofins { get; set; }

        public int? CfopSaida { get; set; }

        public decimal? QuantidadeEstoqueSemNota { get; set; }

        public string? Foto { get; set; }
    }
}
