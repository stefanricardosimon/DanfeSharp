using System;

namespace DanfeSharp.NFSe.Modelo
{
    /// <summary>
    /// Modelo com todos os dados necessários para a geração do DANFSe, conforme a Nota Técnica nº 008 (SE/CGNFS-e).
    /// </summary>
    public class DanfseViewModel
    {
        #region Cabeçalho

        /// <summary>
        /// Chave de acesso da NFS-e (50 dígitos, sem o prefixo "NFS").
        /// </summary>
        public string ChaveAcesso { get; set; }

        /// <summary>
        /// Município do emitente, exibido no canto superior direito.
        /// </summary>
        public string MunicipioEmitente { get; set; }

        public string AmbienteGerador { get; set; }
        public string TipoAmbiente { get; set; }

        /// <summary>
        /// Verdadeiro quando a NFS-e foi emitida em ambiente de Homologação (tpAmb = 2).
        /// </summary>
        public bool Homologacao { get; set; }

        /// <summary>
        /// URL a ser codificada no QR Code de consulta pública.
        /// </summary>
        public string UrlConsultaPublica { get; set; }

        #endregion

        #region Dados de Identificação da NFS-e

        public string NumeroNFSe { get; set; }
        public string Competencia { get; set; }
        public string DataHoraEmissaoNFSe { get; set; }
        public string NumeroDPS { get; set; }
        public string SerieDPS { get; set; }
        public string DataHoraEmissaoDPS { get; set; }
        public string EmitenteNFSe { get; set; }
        public string SituacaoNFSe { get; set; }
        public string Finalidade { get; set; }

        #endregion

        public PessoaViewModel Prestador { get; set; } = new PessoaViewModel();
        public PessoaViewModel Tomador { get; set; } = new PessoaViewModel();
        public PessoaViewModel Destinatario { get; set; } = new PessoaViewModel();
        public PessoaViewModel Intermediario { get; set; } = new PessoaViewModel();

        /// <summary>
        /// Verdadeiro quando o destinatário da operação é o próprio tomador/adquirente (item 2.3.2 do manual).
        /// </summary>
        public bool DestinatarioEhTomador { get; set; }

        public ServicoViewModel Servico { get; set; } = new ServicoViewModel();

        public TributacaoMunicipalViewModel TributacaoMunicipal { get; set; } = new TributacaoMunicipalViewModel();
        public TributacaoFederalViewModel TributacaoFederal { get; set; } = new TributacaoFederalViewModel();
        public TributacaoIBSCBSViewModel TributacaoIBSCBS { get; set; } = new TributacaoIBSCBSViewModel();
        public ValorTotalViewModel ValorTotal { get; set; } = new ValorTotalViewModel();

        /// <summary>
        /// Texto consolidado do bloco "Informações Complementares" (item 2.1.12 do manual).
        /// </summary>
        public string InformacoesComplementares { get; set; }

        #region Canhoto

        /// <summary>
        /// Quando verdadeiro (padrão), o bloco opcional de canhoto é desenhado (item 2.1.13/2.3.3 do manual).
        /// </summary>
        public bool ExibirCanhoto { get; set; } = true;

        /// <summary>
        /// "Nº da NFS-e / Chave da NFS-e", exibido no canhoto.
        /// </summary>
        public string NumeroChaveNFSeCanhoto { get; set; }

        #endregion

        #region Marcas d'água (item 2.5 do manual)

        /// <summary>
        /// Quando verdadeiro, imprime a marca d'água "CANCELADA". Esta informação não consta da NFS-e e deve
        /// ser fornecida pelo integrador com base no evento de cancelamento correspondente.
        /// </summary>
        public bool Cancelada { get; set; }

        /// <summary>
        /// Quando verdadeiro, imprime a marca d'água "SUBSTITUÍDA". Esta informação não consta da NFS-e e deve
        /// ser fornecida pelo integrador com base no evento de substituição correspondente.
        /// </summary>
        public bool Substituida { get; set; }

        #endregion
    }
}
