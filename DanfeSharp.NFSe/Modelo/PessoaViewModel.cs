namespace DanfeSharp.NFSe.Modelo
{
    /// <summary>
    /// Dados de uma pessoa (prestador, tomador, destinatário ou intermediário) exibidos no DANFSe.
    /// </summary>
    public class PessoaViewModel
    {
        public string CnpjCpfNif { get; set; }
        public string IndicadorMunicipal { get; set; }
        public string Telefone { get; set; }
        public string NomeRazaoSocial { get; set; }
        public string MunicipioUf { get; set; }
        public string CodigoIbgeCep { get; set; }
        public string Endereco { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// Situação perante o Simples Nacional na data de competência (somente Prestador).
        /// </summary>
        public string SimplesNacional { get; set; }

        /// <summary>
        /// Regime de apuração tributária pelo Simples Nacional (somente Prestador).
        /// </summary>
        public string RegimeApuracaoSN { get; set; }

        /// <summary>
        /// Indica se há dados desta pessoa na NFS-e (item 2.3.1/2.3.2 do manual).
        /// </summary>
        public bool Identificado { get; set; }
    }
}
