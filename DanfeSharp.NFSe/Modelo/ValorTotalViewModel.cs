namespace DanfeSharp.NFSe.Modelo
{
    /// <summary>
    /// Dados do Valor Total da NFS-e, item 2.1.11 do manual.
    /// </summary>
    public class ValorTotalViewModel
    {
        public string ValorOperacaoServico { get; set; }
        public string DescontoIncondicionado { get; set; }
        public string DescontoCondicionado { get; set; }
        public string TotalRetencoes { get; set; }
        public string ValorLiquidoNFSe { get; set; }
        public string TotalIbsCbs { get; set; }
        public string ValorLiquidoNFSeIbsCbs { get; set; }
    }
}
