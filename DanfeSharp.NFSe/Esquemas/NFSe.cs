using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DanfeSharp.NFSe.Esquemas
{
    /// <summary>
    /// NFS-e (Nota Fiscal de Serviço Eletrônica Nacional). Elemento raiz do arquivo assinado.
    /// </summary>
    [XmlRoot("NFSe", Namespace = Namespaces.NFSe, IsNullable = false)]
    public class NFSeDocumento
    {
        public InfNFSe infNFSe { get; set; }
    }

    /// <summary>
    /// Informações da NFS-e (TCInfNFSe)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class InfNFSe
    {
        /// <summary>
        /// Descrição do código do IBGE do município emissor da NFS-e.
        /// </summary>
        public string xLocEmi { get; set; }

        /// <summary>
        /// Descrição do local da prestação do serviço.
        /// </summary>
        public string xLocPrestacao { get; set; }

        /// <summary>
        /// Número sequencial da NFS-e.
        /// </summary>
        public string nNFSe { get; set; }

        /// <summary>
        /// Código do IBGE do município de incidência do ISSQN.
        /// </summary>
        public string cLocIncid { get; set; }

        /// <summary>
        /// Descrição do município de incidência do ISSQN.
        /// </summary>
        public string xLocIncid { get; set; }

        /// <summary>
        /// Descrição do código de tributação nacional do ISSQN.
        /// </summary>
        public string xTribNac { get; set; }

        /// <summary>
        /// Descrição do código de tributação municipal do ISSQN.
        /// </summary>
        public string xTribMun { get; set; }

        /// <summary>
        /// Descrição do código da NBS.
        /// </summary>
        public string xNBS { get; set; }

        /// <summary>
        /// Ambiente gerador da NFS-e. 1 - Prefeitura; 2 - Sistema Nacional da NFS-e.
        /// </summary>
        public string ambGer { get; set; }

        /// <summary>
        /// Situação da NFS-e.
        /// </summary>
        public string cStat { get; set; }

        public DateTime dhProc { get; set; }

        public Emitente emit { get; set; }

        public ValoresNFSe valores { get; set; }

        /// <summary>
        /// Outras informações do município.
        /// </summary>
        public string xOutInf { get; set; }

        [XmlElement("IBSCBS")]
        public IBSCBSTotais IBSCBS { get; set; }

        public DPS DPS { get; set; }

        /// <summary>
        /// Identificador único da NFS-e (Id do elemento raiz assinado).
        /// </summary>
        [XmlAttribute(DataType = "ID")]
        public string Id { get; set; }
    }

    /// <summary>
    /// Emitente da NFS-e (TCEmitente). Usado apenas para a identificação de município/UF no cabeçalho do DANFSe.
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class Emitente
    {
        public string IM { get; set; }
        public string xNome { get; set; }
        public string xFant { get; set; }
        public EnderecoEmitente enderNac { get; set; }
        public string fone { get; set; }
        public string email { get; set; }
    }

    /// <summary>
    /// Endereço nacional do emitente (TCEnderecoEmitente)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class EnderecoEmitente
    {
        public string xLgr { get; set; }
        public string nro { get; set; }
        public string xCpl { get; set; }
        public string xBairro { get; set; }
        public string cMun { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
    }

    /// <summary>
    /// Valores apurados (computados) da NFS-e referentes ao ISSQN (TCValoresNFSe)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class ValoresNFSe
    {
        public double? vCalcDR { get; set; }
        public string tpBM { get; set; }
        public double? vCalcBM { get; set; }
        public double? vBC { get; set; }
        public double? pAliqAplic { get; set; }
        public double? vISSQN { get; set; }
        public double? vTotalRet { get; set; }
        public double vLiq { get; set; }
    }

    #region IBS/CBS - Grupo computado (NFSe/infNFSe/IBSCBS)

    /// <summary>
    /// Totais apurados de IBS/CBS (TCRTCIBSCBS)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class IBSCBSTotais
    {
        /// <summary>
        /// Código do IBGE do município de incidência.
        /// </summary>
        public string cLocalidadeIncid { get; set; }

        /// <summary>
        /// Descrição do município de incidência.
        /// </summary>
        public string xLocalidadeIncid { get; set; }

        public ValoresIBSCBS valores { get; set; }

        public TotalCIBS totCIBS { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class ValoresIBSCBS
    {
        public double vBC { get; set; }

        /// <summary>
        /// Redução da base de cálculo (exclusões/reduções e reduções por regime específico).
        /// </summary>
        public double? vCalcReeRepRes { get; set; }

        [XmlElement("uf")]
        public ValoresIBSCBSUF uf { get; set; }

        [XmlElement("mun")]
        public ValoresIBSCBSMun mun { get; set; }

        [XmlElement("fed")]
        public ValoresIBSCBSFed fed { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class ValoresIBSCBSUF
    {
        public double pIBSUF { get; set; }
        public double? pRedAliqUF { get; set; }
        public double pAliqEfetUF { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class ValoresIBSCBSMun
    {
        public double pIBSMun { get; set; }
        public double? pRedAliqMun { get; set; }
        public double pAliqEfetMun { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class ValoresIBSCBSFed
    {
        public double pCBS { get; set; }
        public double? pRedAliqCBS { get; set; }
        public double pAliqEfetCBS { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class TotalCIBS
    {
        public double vTotNF { get; set; }
        public TotalIBS gIBS { get; set; }
        public TotalCBS gCBS { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class TotalIBS
    {
        public double vIBSTot { get; set; }
        public TotalIBSUF gIBSUFTot { get; set; }
        public TotalIBSMun gIBSMunTot { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class TotalIBSUF
    {
        public double? vDifUF { get; set; }
        public double vIBSUF { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class TotalIBSMun
    {
        public double? vDifMun { get; set; }
        public double vIBSMun { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class TotalCBS
    {
        public double? vDifCBS { get; set; }
        public double vCBS { get; set; }
    }

    #endregion

    /// <summary>
    /// DPS (Declaração de Prestação de Serviços) - TCDPS
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class DPS
    {
        public InfDPS infDPS { get; set; }
    }

    /// <summary>
    /// Informações da DPS (TCInfDPS)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class InfDPS
    {
        /// <summary>
        /// 1 - Produção; 2 - Homologação.
        /// </summary>
        public string tpAmb { get; set; }

        public DateTime dhEmi { get; set; }

        /// <summary>
        /// Série da DPS.
        /// </summary>
        public string serie { get; set; }

        /// <summary>
        /// Número da DPS.
        /// </summary>
        public string nDPS { get; set; }

        /// <summary>
        /// Competência da NFS-e (Ano, Mês e Dia).
        /// </summary>
        public DateTime dCompet { get; set; }

        /// <summary>
        /// Emitente da DPS. 1 - Prestador; 2 - Tomador; 3 - Intermediário.
        /// </summary>
        public string tpEmit { get; set; }

        public Substituicao subst { get; set; }

        public InfoPrestador prest { get; set; }

        public InfoPessoa toma { get; set; }

        public InfoPessoa interm { get; set; }

        public Serv serv { get; set; }

        public InfoValores valores { get; set; }

        [XmlElement("IBSCBS")]
        public InfoIBSCBSDeclarado IBSCBS { get; set; }
    }

    /// <summary>
    /// Grupo de substituição de NFS-e (TCSubstituicao)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class Substituicao
    {
        /// <summary>
        /// Chave de acesso da NFS-e substituída.
        /// </summary>
        public string chSubstda { get; set; }

        public string cMotivo { get; set; }
        public string xMotivo { get; set; }
    }

    /// <summary>
    /// Informações do prestador de serviços (TCInfoPrestador)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class InfoPrestador
    {
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string NIF { get; set; }
        public string cNaoNIF { get; set; }

        public string IM { get; set; }
        public string xNome { get; set; }
        public Endereco end { get; set; }
        public string fone { get; set; }
        public string email { get; set; }
        public RegTrib regTrib { get; set; }

        [XmlIgnore]
        public string CnpjCpfNif => !string.IsNullOrWhiteSpace(CNPJ) ? CNPJ : (!string.IsNullOrWhiteSpace(CPF) ? CPF : NIF);
    }

    /// <summary>
    /// Regime de tributação do prestador (TCRegTrib)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class RegTrib
    {
        /// <summary>
        /// Situação perante o Simples Nacional na data de competência.
        /// </summary>
        public string opSimpNac { get; set; }

        /// <summary>
        /// Regime de apuração tributária pelo Simples Nacional.
        /// </summary>
        public string regApTribSN { get; set; }

        /// <summary>
        /// Regime especial de tributação do ISSQN.
        /// </summary>
        public string regEspTrib { get; set; }
    }

    /// <summary>
    /// Informações de pessoa (tomador ou intermediário) - TCInfoPessoa
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class InfoPessoa
    {
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string NIF { get; set; }
        public string cNaoNIF { get; set; }

        public string IM { get; set; }
        public string xNome { get; set; }
        public Endereco end { get; set; }
        public string fone { get; set; }
        public string email { get; set; }

        [XmlIgnore]
        public string CnpjCpfNif => !string.IsNullOrWhiteSpace(CNPJ) ? CNPJ : (!string.IsNullOrWhiteSpace(CPF) ? CPF : NIF);
    }

    /// <summary>
    /// Endereço nacional ou no exterior (TCEndereco)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class Endereco
    {
        public EnderNac endNac { get; set; }
        public EnderExt endExt { get; set; }

        public string xLgr { get; set; }
        public string nro { get; set; }
        public string xCpl { get; set; }
        public string xBairro { get; set; }
    }

    /// <summary>
    /// Grupo de informações específicas de endereço nacional (TCEnderNac)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class EnderNac
    {
        public string cMun { get; set; }
        public string CEP { get; set; }
    }

    /// <summary>
    /// Grupo de informações específicas de endereço no exterior (TCEnderExt)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class EnderExt
    {
        public string cPais { get; set; }
        public string cEndPost { get; set; }
        public string xCidade { get; set; }
        public string xEstProvReg { get; set; }
    }

    /// <summary>
    /// Informações do destinatário da operação (TCRTCInfoDest)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class InfoDestinatario
    {
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string NIF { get; set; }
        public string cNaoNIF { get; set; }

        public string xNome { get; set; }
        public Endereco end { get; set; }
        public string fone { get; set; }
        public string email { get; set; }

        [XmlIgnore]
        public string CnpjCpfNif => !string.IsNullOrWhiteSpace(CNPJ) ? CNPJ : (!string.IsNullOrWhiteSpace(CPF) ? CPF : NIF);
    }

    /// <summary>
    /// Informações do serviço prestado (TCServ)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class Serv
    {
        public LocPrest locPrest { get; set; }
        public CServ cServ { get; set; }
        public InfoObra obra { get; set; }
        public AtvEvento atvEvento { get; set; }
        public InfoCompl infoCompl { get; set; }
    }

    /// <summary>
    /// Local da prestação do serviço (TCLocPrest)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class LocPrest
    {
        /// <summary>
        /// Código do IBGE do município de prestação do serviço.
        /// </summary>
        public string cLocPrestacao { get; set; }

        /// <summary>
        /// Código do país de prestação do serviço (Tabela de Países ISO), quando prestado no exterior.
        /// </summary>
        public string cPaisPrestacao { get; set; }
    }

    /// <summary>
    /// Código de tributação do serviço prestado (TCCServ)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class CServ
    {
        /// <summary>
        /// Código de tributação nacional do ISSQN.
        /// </summary>
        public string cTribNac { get; set; }

        /// <summary>
        /// Código de tributação municipal do ISSQN.
        /// </summary>
        public string cTribMun { get; set; }

        /// <summary>
        /// Descrição do serviço prestado.
        /// </summary>
        public string xDescServ { get; set; }

        /// <summary>
        /// Código da Nomenclatura Brasileira de Serviços (NBS).
        /// </summary>
        public string cNBS { get; set; }
    }

    /// <summary>
    /// Informações da obra (TCInfoObra)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class InfoObra
    {
        public string inscImobFisc { get; set; }
        public string cObra { get; set; }
        public string cCIB { get; set; }
    }

    /// <summary>
    /// Informações da atividade de evento (TCAtvEvento)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class AtvEvento
    {
        public string xNome { get; set; }
        public DateTime dtIni { get; set; }
        public DateTime dtFim { get; set; }
        public string idAtvEvt { get; set; }
    }

    /// <summary>
    /// Informações complementares do serviço prestado (TCInfoCompl)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class InfoCompl
    {
        public string idDocTec { get; set; }
        public string docRef { get; set; }
        public string xPed { get; set; }
        public InfoItemPed gItemPed { get; set; }
        public string xInfComp { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class InfoItemPed
    {
        [XmlElement("xItemPed")]
        public List<string> xItemPed { get; set; }

        public InfoItemPed()
        {
            xItemPed = new List<string>();
        }
    }

    /// <summary>
    /// Valores declarados na DPS (TCInfoValores)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class InfoValores
    {
        public VServPrest vServPrest { get; set; }
        public VDescCondIncond vDescCondIncond { get; set; }
        public InfoDedRed vDedRed { get; set; }
        public InfoTributacao trib { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class VServPrest
    {
        public double? vReceb { get; set; }
        public double vServ { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class VDescCondIncond
    {
        public double? vDescIncond { get; set; }
        public double? vDescCond { get; set; }
    }

    /// <summary>
    /// Dedução/redução padrão do valor do serviço (TCInfoDedRed)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class InfoDedRed
    {
        public double? pDR { get; set; }
        public double? vDR { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class InfoTributacao
    {
        public TribMunicipal tribMun { get; set; }
        public TribFederal tribFed { get; set; }
        public TribTotal totTrib { get; set; }
    }

    /// <summary>
    /// Tributação municipal (ISSQN) declarada (TCTribMunicipal)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class TribMunicipal
    {
        /// <summary>
        /// Tributação do ISSQN sobre o serviço prestado.
        /// </summary>
        public string tribISSQN { get; set; }

        /// <summary>
        /// Código do país de resultado do serviço (exportação).
        /// </summary>
        public string cPaisResult { get; set; }

        /// <summary>
        /// Identificação do tipo de imunidade do ISSQN.
        /// </summary>
        public string tpImunidade { get; set; }

        public ExigSuspensa exigSusp { get; set; }

        public BeneficioMunicipal BM { get; set; }

        /// <summary>
        /// Tipo de retenção do ISSQN.
        /// </summary>
        public string tpRetISSQN { get; set; }

        public double? pAliq { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class ExigSuspensa
    {
        public string tpSusp { get; set; }
        public string nProcesso { get; set; }
    }

    /// <summary>
    /// Redução da base de cálculo do ISSQN devido a Benefício Municipal (TCBeneficioMunicipal)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class BeneficioMunicipal
    {
        public string nBM { get; set; }

        /// <summary>
        /// Valor monetário de redução da BC do ISSQN devido ao Benefício Municipal.
        /// </summary>
        public double? vRedBCBM { get; set; }

        /// <summary>
        /// Valor percentual de redução da BC do ISSQN devido ao Benefício Municipal.
        /// </summary>
        public double? pRedBCBM { get; set; }
    }

    /// <summary>
    /// Tributação federal, exceto CBS (TCTribFederal)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class TribFederal
    {
        public TribOutrosPisCofins piscofins { get; set; }
        public double? vRetCP { get; set; }
        public double? vRetIRRF { get; set; }
        public double? vRetCSLL { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class TribOutrosPisCofins
    {
        public string CST { get; set; }
        public double? vBCPisCofins { get; set; }
        public double? pAliqPis { get; set; }
        public double? pAliqCofins { get; set; }
        public double? vPis { get; set; }
        public double? vCofins { get; set; }

        /// <summary>
        /// Tipo de retenção do PIS/COFINS/CSLL.
        /// </summary>
        public string tpRetPisCofins { get; set; }
    }

    /// <summary>
    /// Totais aproximados dos tributos (Lei 12.741/2012) (TCTribTotal)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class TribTotal
    {
        public TribTotalMonetario vTotTrib { get; set; }
        public TribTotalPercentual pTotTrib { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class TribTotalMonetario
    {
        public double vTotTribFed { get; set; }
        public double vTotTribEst { get; set; }
        public double vTotTribMun { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class TribTotalPercentual
    {
        public double pTotTribFed { get; set; }
        public double pTotTribEst { get; set; }
        public double pTotTribMun { get; set; }
    }

    #region IBS/CBS - Grupo declarado na DPS (DPS/infDPS/IBSCBS)

    /// <summary>
    /// Informações relativas ao IBS/CBS declaradas na DPS (TCRTCInfoIBSCBS)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class InfoIBSCBSDeclarado
    {
        /// <summary>
        /// Indicador da finalidade da emissão de NFS-e.
        /// </summary>
        public string finNFSe { get; set; }

        /// <summary>
        /// Código indicador da operação.
        /// </summary>
        public string cIndOp { get; set; }

        public InfoDestinatario dest { get; set; }

        public InfoImovel imovel { get; set; }

        public InfoValoresIBSCBSDeclarado valores { get; set; }
    }

    /// <summary>
    /// Informações do imóvel (TCRTCInfoImovel)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class InfoImovel
    {
        public string inscImobFisc { get; set; }
        public string cCIB { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class InfoValoresIBSCBSDeclarado
    {
        public InfoReeRepRes gReeRepRes { get; set; }
        public InfoTributosIBSCBS trib { get; set; }
    }

    /// <summary>
    /// Redução, isenção ou não incidência decorrente de decisão administrativa ou judicial (TCRTCInfoReeRepRes)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class InfoReeRepRes
    {
        [XmlElement("documentos")]
        public List<DocDedRedIBSCBS> documentos { get; set; }

        public InfoReeRepRes()
        {
            documentos = new List<DocDedRedIBSCBS>();
        }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class DocDedRedIBSCBS
    {
        public double vlrReeRepRes { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class InfoTributosIBSCBS
    {
        public InfoTributosSitClas gIBSCBS { get; set; }
    }

    /// <summary>
    /// Situação e classificação tributária do IBS/CBS (TCRTCInfoTributosSitClas)
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFSe)]
    public class InfoTributosSitClas
    {
        /// <summary>
        /// Código da Situação Tributária do IBS/CBS.
        /// </summary>
        public string CST { get; set; }

        /// <summary>
        /// Código de Classificação Tributária do IBS/CBS.
        /// </summary>
        public string cClassTrib { get; set; }
    }

    #endregion
}
