namespace DanfeSharp.NFSe.Modelo
{
    /// <summary>
    /// Dados da Tributação IBS/CBS, item 2.1.10 do manual.
    /// </summary>
    public class TributacaoIBSCBSViewModel
    {
        public string CstCClassTrib { get; set; }
        public string IndicadorOperacaoMunicipioIncidencia { get; set; }
        public string ExclusoesReducoesBaseCalculo { get; set; }
        public string BaseCalculoAposExclusoesReducoes { get; set; }
        public string ReducaoAliquotaIbsCbs { get; set; }
        public string AliquotaIbsUfMun { get; set; }
        public string AliquotaEfetivaMunicipalIbs { get; set; }
        public string ValorApuradoMunicipalIbs { get; set; }
        public string AliquotaEfetivaEstadualIbs { get; set; }
        public string ValorApuradoEstadualIbs { get; set; }
        public string ValorTotalApuradoIbs { get; set; }
        public string AliquotaCbs { get; set; }
        public string AliquotaEfetivaCbs { get; set; }
        public string ValorTotalApuradoCbs { get; set; }
    }
}
