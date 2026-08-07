namespace DanfeSharp.NFSe.Modelo
{
    /// <summary>
    /// Dados da Tributação Municipal (ISSQN), item 2.1.8 do manual.
    /// </summary>
    public class TributacaoMunicipalViewModel
    {
        /// <summary>
        /// Quando falso, o bloco inteiro deve exibir apenas a mensagem de operação não sujeita ao ISSQN (item 2.3.1, nota 4).
        /// </summary>
        public bool SujeitoAoIssqn { get; set; } = true;

        public string TipoTributacao { get; set; }
        public string MunicipioUfPaisIncidencia { get; set; }

        /// <summary>
        /// Linha com Regime Especial / Imunidade / Suspensão / Nº Processo (nota 5 - pode ser suprimida se todos ausentes).
        /// </summary>
        public bool MostrarLinhaRegimeImunidadeSuspensao { get; set; }
        public string RegimeEspecial { get; set; }
        public string TipoImunidade { get; set; }
        public string SuspensaoExigibilidade { get; set; }
        public string NumeroProcessoSuspensao { get; set; }

        /// <summary>
        /// Linha com Benefício Municipal / Cálculo do BM / Total Deduções-Reduções / Desconto Incondicionado (nota 5).
        /// </summary>
        public bool MostrarLinhaBeneficioDeducoes { get; set; }
        public string BeneficioMunicipal { get; set; }
        public string CalculoBM { get; set; }
        public string TotalDeducoesReducoes { get; set; }
        public string DescontoIncondicionado { get; set; }

        public string BaseCalculoIssqn { get; set; }
        public string AliquotaAplicada { get; set; }
        public string RetencaoIssqn { get; set; }
        public string IssqnApurado { get; set; }
    }
}
