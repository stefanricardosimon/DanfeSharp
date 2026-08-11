namespace DanfeSharp.NFSe.Modelo
{
    /// <summary>
    /// Dados da Tributação Federal (exceto CBS), item 2.1.9 do manual.
    /// </summary>
    public class TributacaoFederalViewModel
    {
        public string Irrf { get; set; }
        public string ContribuicaoPrevidenciariaRetida { get; set; }
        public string ContribuicoesSociaisRetidas { get; set; }
        public string DescricaoContribuicoesSociaisRetidas { get; set; }

        /// <summary>
        /// Linha do PIS/COFINS - Débito Apuração Própria (nota 6, válida até o final do ano-calendário de 2026).
        /// </summary>
        public bool MostrarLinhaPisCofins { get; set; } = true;
        public string PisDebitoApuracaoPropria { get; set; }
        public string CofinsDebitoApuracaoPropria { get; set; }
    }
}
