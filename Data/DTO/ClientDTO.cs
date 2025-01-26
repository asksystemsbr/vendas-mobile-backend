namespace ControlStoreAPI.Data.DTO
{
    public class ClientDTO
    {
        public int ID { get; set; }
        public int? ID_SITUACAO_CLIENTE { get; set; }

        public string? NOME { get; set; }
        public string? FANTASIA { get; set; }
        public string? EMAIL { get; set; }
        public string? CpfCnpj { get; set; }
        public string? RG { get; set; }
        public string? ORGAO_RG { get; set; }
        public string? INSCRICAO_ESTADUAL { get; set; }
        public string? INSCRICAO_MUNICIPAL { get; set; }
        public string? TIPO_PESSOA { get; set; }
        public DateTime? DATA_CADASTRO { get; set; }
        public DateTime? DATA_EMISSAO_RG { get; set; } // Adicionado
        public string? SEXO { get; set; }
        public string? LOGRADOURO { get; set; }
        public string? NUMERO { get; set; }
        public string? COMPLEMENTO { get; set; }
        public string? CEP { get; set; }
        public string? BAIRRO { get; set; }
        public string? CIDADE { get; set; }
        public string? UF { get; set; }
        public string? FoneOne { get; set; }
        public string? FoneTwo { get; set; }
        public string? CELULAR { get; set; }
        public string? CONTATO { get; set; }
        public int? CODIGO_IBGE_CIDADE { get; set; }
        public int? CODIGO_IBGE_UF { get; set; }
        public DateTime? DATA_ULTIMA_COMPRA { get; set; }
        public string? ULTIMANFE { get; set; }
        public string? ULTIMAOS { get; set; }
        public string? PLACA { get; set; }
        public string? MENSALISTA { get; set; }
        public string? CONVENIADO { get; set; }
        public string? DESCONTO1HORA { get; set; }
        public string? DESCONTO2HORA { get; set; }
        public string? TURNO { get; set; }
        public string? PONTOS { get; set; } = "0";
        public decimal VALOR_FIXO { get; set; } = 0.000000M;

        public string? FORMA_PGTO { get; set; }
        public string? DIAS_PGTO { get; set; }
        public int? FUNCIONARIO_ID { get; set; }

        public decimal LIMITE_SALDO { get; set; } = 0.00M;

        public string? OBS { get; set; }
        public DateTime? DATA_NASCIMENTO { get; set; }

        public string? Usuario { get; set; }

        public string? Senha { get; set; }

        // Relacionamento com SituacaoCliente (se necessário)
        // public virtual SituacaoCliente? SituacaoCliente { get; set; }
    }
}
