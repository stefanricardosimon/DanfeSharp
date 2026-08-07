using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using DanfeSharp.NFSe.Esquemas;

namespace DanfeSharp.NFSe.Modelo
{
    /// <summary>
    /// Cria um <see cref="DanfseViewModel"/> a partir do XML da NFS-e, conforme o mapeamento de campos
    /// descrito no item 2.4.5 da Nota Técnica nº 008 (SE/CGNFS-e).
    /// </summary>
    public static class DanfseViewModelCreator
    {
        private const string SemInformacao = "-";

        /// <summary>
        /// Cria o modelo a partir de um arquivo xml.
        /// </summary>
        /// <param name="caminho">Caminho do arquivo XML da NFS-e.</param>
        /// <param name="resolverMunicipio">
        /// Função opcional que resolve o nome de um município a partir do seu código do IBGE (7 dígitos).
        /// Necessária porque o leiaute da NFS-e não traz o nome do município do tomador/adquirente,
        /// destinatário, intermediário e prestador, apenas o código. Quando omitida, o código é exibido.
        /// </param>
        public static DanfseViewModel CriarDeArquivoXml(string caminho, Func<string, string> resolverMunicipio = null)
        {
            using (StreamReader sr = new StreamReader(caminho, true))
            {
                return CriarDeArquivoXmlInternal(sr, resolverMunicipio);
            }
        }

        /// <summary>
        /// Cria o modelo a partir de um arquivo xml contido num stream.
        /// </summary>
        public static DanfseViewModel CriarDeArquivoXml(Stream stream, Func<string, string> resolverMunicipio = null)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            using (StreamReader sr = new StreamReader(stream, true))
            {
                return CriarDeArquivoXmlInternal(sr, resolverMunicipio);
            }
        }

        /// <summary>
        /// Cria o modelo a partir de uma string xml.
        /// </summary>
        public static DanfseViewModel CriarDeStringXml(string str, Func<string, string> resolverMunicipio = null)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            using (StringReader sr = new StringReader(str))
            {
                return CriarDeArquivoXmlInternal(sr, resolverMunicipio);
            }
        }

        private static DanfseViewModel CriarDeArquivoXmlInternal(TextReader reader, Func<string, string> resolverMunicipio)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(NFSeDocumento));

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(reader.ReadToEnd());
            var idNode = doc.SelectSingleNode("//*[local-name()='infNFSe']/@Id");
            string chave = idNode?.InnerText ?? "desconhecida";

            using (TextReader sr = new StringReader(doc.OuterXml))
            {
                try
                {
                    var nfse = (NFSeDocumento)serializer.Deserialize(sr);
                    return CreateFromXml(nfse, resolverMunicipio);
                }
                catch (InvalidOperationException e)
                {
                    if (e.InnerException is XmlException ex)
                    {
                        throw new Exception($"Não foi possível interpretar o XML de chave {chave}. Linha {ex.LineNumber}, posição {ex.LinePosition}.", e);
                    }

                    throw new XmlException($"O XML de chave {chave} não parece ser uma NFS-e.", e);
                }
            }
        }

        public static DanfseViewModel CreateFromXml(NFSeDocumento documento, Func<string, string> resolverMunicipio = null)
        {
            if (documento == null) throw new ArgumentNullException(nameof(documento));

            var infNFSe = documento.infNFSe ?? throw new InvalidOperationException("O XML não contém o elemento infNFSe.");
            var infDPS = infNFSe.DPS?.infDPS ?? throw new InvalidOperationException("O XML não contém o elemento DPS/infDPS.");

            resolverMunicipio = resolverMunicipio ?? (codigo => codigo);

            var model = new DanfseViewModel();

            PreencherCabecalho(model, infNFSe, infDPS);
            PreencherIdentificacao(model, infNFSe, infDPS);
            PreencherPrestador(model, infDPS.prest, infNFSe, resolverMunicipio);
            PreencherTomador(model, infDPS.toma, resolverMunicipio);
            PreencherDestinatario(model, infDPS, resolverMunicipio);
            PreencherIntermediario(model, infDPS.interm, resolverMunicipio);
            PreencherServico(model, infNFSe, infDPS);
            PreencherTributacaoMunicipal(model, infNFSe, infDPS);
            PreencherTributacaoFederal(model, infDPS);
            PreencherTributacaoIBSCBS(model, infNFSe, infDPS);
            PreencherValorTotal(model, infNFSe, infDPS);
            PreencherInformacoesComplementares(model, infNFSe, infDPS);

            model.NumeroChaveNFSeCanhoto = $"{model.NumeroNFSe} / {model.ChaveAcesso}";

            return model;
        }

        #region Cabeçalho / Identificação

        private static void PreencherCabecalho(DanfseViewModel model, InfNFSe infNFSe, InfDPS infDPS)
        {
            model.ChaveAcesso = SemPrefixoNFS(infNFSe.Id);
            model.MunicipioEmitente = Formatador.FormatarMunicipioUf(infNFSe.xLocEmi, infNFSe.emit?.enderNac?.UF);
            model.AmbienteGerador = infNFSe.ambGer;
            model.TipoAmbiente = infDPS.tpAmb;
            model.Homologacao = infDPS.tpAmb == "2";
            model.UrlConsultaPublica = "https://www.nfse.gov.br/ConsultaPublica/?tpc=1&chave=" + model.ChaveAcesso;
        }

        private static string SemPrefixoNFS(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return id;
            return id.StartsWith("NFS", StringComparison.OrdinalIgnoreCase) ? id.Substring(3) : id;
        }

        private static void PreencherIdentificacao(DanfseViewModel model, InfNFSe infNFSe, InfDPS infDPS)
        {
            model.NumeroNFSe = infNFSe.nNFSe;
            model.Competencia = infDPS.dCompet.ToString("dd/MM/yyyy");
            model.DataHoraEmissaoNFSe = infNFSe.dhProc.ToString("dd/MM/yyyy HH:mm:ss");
            model.NumeroDPS = infDPS.nDPS;
            model.SerieDPS = infDPS.serie;
            model.DataHoraEmissaoDPS = infDPS.dhEmi.ToString("dd/MM/yyyy HH:mm:ss");
            model.EmitenteNFSe = Descricoes.TpEmit(infDPS.tpEmit);
            model.SituacaoNFSe = Descricoes.CStat(infNFSe.cStat);
            model.Finalidade = Descricoes.FinNFSe(infDPS.IBSCBS?.finNFSe);
        }

        #endregion

        #region Pessoas

        private static void PreencherPrestador(DanfseViewModel model, InfoPrestador prest, InfNFSe infNFSe, Func<string, string> resolverMunicipio)
        {
            var p = model.Prestador;
            p.Identificado = prest != null;
            if (prest == null) return;

            // O prestador declarado na DPS (infDPS/prest) pode omitir nome/endereço/telefone/e-mail
            // quando o prestador é o próprio emitente da NFS-e — nesse caso, esses dados só constam
            // em NFSe/infNFSe/emit, que é usado aqui como alternativa.
            var emit = infNFSe.emit;

            p.CnpjCpfNif = Formatador.FormatarCpfCnpj(prest.CnpjCpfNif);
            p.IndicadorMunicipal = Traco(prest.IM);
            p.Telefone = Formatador.FormatarTelefone(!string.IsNullOrWhiteSpace(prest.fone) ? prest.fone : emit?.fone);
            p.NomeRazaoSocial = Traco(!string.IsNullOrWhiteSpace(prest.xNome) ? prest.xNome : emit?.xNome);
            p.Email = Traco(!string.IsNullOrWhiteSpace(prest.email) ? prest.email : emit?.email);

            if (prest.end != null)
            {
                p.MunicipioUf = MunicipioUf(prest.end, resolverMunicipio);
                p.CodigoIbgeCep = CodigoIbgeCep(prest.end);
                p.Endereco = ConcatenarEndereco(prest.end);
            }
            else if (emit?.enderNac != null)
            {
                var end = emit.enderNac;
                p.MunicipioUf = Traco(Formatador.FormatarMunicipioUf(infNFSe.xLocEmi, end.UF));
                p.CodigoIbgeCep = Traco(JuntarNaoVazios(" / ", end.cMun, Formatador.FormatarCEP(end.CEP)));
                p.Endereco = Traco(JuntarNaoVazios(", ", end.xLgr, end.nro, end.xCpl, end.xBairro));
            }
            else
            {
                p.MunicipioUf = SemInformacao;
                p.CodigoIbgeCep = SemInformacao;
                p.Endereco = SemInformacao;
            }

            p.SimplesNacional = Descricoes.OpSimpNac(prest.regTrib?.opSimpNac);
            p.RegimeApuracaoSN = prest.regTrib?.opSimpNac == "3" ? Descricoes.RegApTribSN(prest.regTrib?.regApTribSN) : SemInformacao;
        }

        private static void PreencherTomador(DanfseViewModel model, InfoPessoa toma, Func<string, string> resolverMunicipio)
        {
            var t = model.Tomador;
            t.Identificado = toma != null;
            if (toma == null) return;

            t.CnpjCpfNif = Formatador.FormatarCpfCnpj(toma.CnpjCpfNif);
            t.IndicadorMunicipal = Traco(toma.IM);
            t.Telefone = Formatador.FormatarTelefone(toma.fone);
            t.NomeRazaoSocial = Traco(toma.xNome);
            t.MunicipioUf = MunicipioUf(toma.end, resolverMunicipio);
            t.CodigoIbgeCep = CodigoIbgeCep(toma.end);
            t.Endereco = ConcatenarEndereco(toma.end);
            t.Email = Traco(toma.email);
        }

        private static void PreencherDestinatario(DanfseViewModel model, InfDPS infDPS, Func<string, string> resolverMunicipio)
        {
            var dest = infDPS.IBSCBS?.dest;
            var d = model.Destinatario;

            d.Identificado = dest != null;
            model.DestinatarioEhTomador = dest == null && infDPS.toma != null;

            if (dest == null) return;

            d.CnpjCpfNif = Formatador.FormatarCpfCnpj(dest.CnpjCpfNif);
            d.Telefone = Formatador.FormatarTelefone(dest.fone);
            d.NomeRazaoSocial = Traco(dest.xNome);
            d.MunicipioUf = MunicipioUf(dest.end, resolverMunicipio);
            d.CodigoIbgeCep = CodigoIbgeCep(dest.end);
            d.Endereco = ConcatenarEndereco(dest.end);
            d.Email = Traco(dest.email);
        }

        private static void PreencherIntermediario(DanfseViewModel model, InfoPessoa interm, Func<string, string> resolverMunicipio)
        {
            var i = model.Intermediario;
            i.Identificado = interm != null;
            if (interm == null) return;

            i.CnpjCpfNif = Formatador.FormatarCpfCnpj(interm.CnpjCpfNif);
            i.IndicadorMunicipal = Traco(interm.IM);
            i.Telefone = Formatador.FormatarTelefone(interm.fone);
            i.NomeRazaoSocial = Traco(interm.xNome);
            i.MunicipioUf = MunicipioUf(interm.end, resolverMunicipio);
            i.CodigoIbgeCep = CodigoIbgeCep(interm.end);
            i.Endereco = ConcatenarEndereco(interm.end);
            i.Email = Traco(interm.email);
        }

        #endregion

        #region Serviço

        private static void PreencherServico(DanfseViewModel model, InfNFSe infNFSe, InfDPS infDPS)
        {
            var serv = infDPS.serv;
            var s = model.Servico;

            var cServ = serv?.cServ;
            s.CodigoTributacao = JuntarNaoVazios(" / ", cServ?.cTribNac, cServ?.cTribMun);
            s.CodigoNBS = Traco(cServ?.cNBS);

            var locPrest = serv?.locPrest;
            if (!string.IsNullOrWhiteSpace(locPrest?.cLocPrestacao))
            {
                var uf = TabelaUF.ObterUFPorCodigoIbge(locPrest.cLocPrestacao);
                s.LocalPrestacao = JuntarNaoVazios(" / ", infNFSe.xLocPrestacao, uf, "BR");
            }
            else
            {
                s.LocalPrestacao = JuntarNaoVazios(" / ", infNFSe.xLocPrestacao, locPrest?.cPaisPrestacao);
            }

            s.DescricaoCodigoTributacao = !string.IsNullOrWhiteSpace(infNFSe.xTribMun) ? infNFSe.xTribMun : infNFSe.xTribNac;
            s.DescricaoServico = cServ?.xDescServ;
        }

        #endregion

        #region Tributação Municipal (ISSQN)

        private static void PreencherTributacaoMunicipal(DanfseViewModel model, InfNFSe infNFSe, InfDPS infDPS)
        {
            var tribMun = infDPS.valores?.trib?.tribMun;
            var valoresNFSe = infNFSe.valores;
            var m = model.TributacaoMunicipal;

            if (tribMun == null) return;

            m.TipoTributacao = Descricoes.TribISSQN(tribMun.tribISSQN);

            var uf = TabelaUF.ObterUFPorCodigoIbge(infNFSe.cLocIncid);
            m.MunicipioUfPaisIncidencia = JuntarNaoVazios(" / ", infNFSe.xLocIncid, uf, tribMun.cPaisResult);

            m.RegimeEspecial = Descricoes.RegEspTrib(infDPS.prest?.regTrib?.regEspTrib);
            m.TipoImunidade = Descricoes.TpImunidade(tribMun.tpImunidade);
            m.SuspensaoExigibilidade = Descricoes.TpSusp(tribMun.exigSusp?.tpSusp);
            m.NumeroProcessoSuspensao = Traco(tribMun.exigSusp?.nProcesso);
            m.MostrarLinhaRegimeImunidadeSuspensao = AlgumPresente(
                infDPS.prest?.regTrib?.regEspTrib, tribMun.tpImunidade, tribMun.exigSusp?.tpSusp, tribMun.exigSusp?.nProcesso);

            m.BeneficioMunicipal = Descricoes.TpBM(valoresNFSe?.tpBM);

            if (valoresNFSe?.vCalcBM != null)
                m.CalculoBM = Moeda(valoresNFSe.vCalcBM);
            else if (tribMun.BM?.vRedBCBM != null)
                m.CalculoBM = Moeda(tribMun.BM.vRedBCBM);
            else if (tribMun.BM?.pRedBCBM != null)
                m.CalculoBM = Percentual(tribMun.BM.pRedBCBM);
            else
                m.CalculoBM = SemInformacao;

            double? totalDeducoes = SomarOpcionais(infDPS.valores?.vDedRed?.vDR, valoresNFSe?.vCalcDR, infNFSe.IBSCBS?.valores?.vCalcReeRepRes);
            m.TotalDeducoesReducoes = Moeda(totalDeducoes);

            m.DescontoIncondicionado = Moeda(infDPS.valores?.vDescCondIncond?.vDescIncond);

            m.MostrarLinhaBeneficioDeducoes = AlgumPresente(
                valoresNFSe?.tpBM, valoresNFSe?.vCalcBM, tribMun.BM?.vRedBCBM, tribMun.BM?.pRedBCBM,
                totalDeducoes, infDPS.valores?.vDescCondIncond?.vDescIncond);

            m.BaseCalculoIssqn = Moeda(valoresNFSe?.vBC);
            m.AliquotaAplicada = Percentual(valoresNFSe?.pAliqAplic);
            m.RetencaoIssqn = Descricoes.TpRetISSQN(tribMun.tpRetISSQN);
            m.IssqnApurado = Moeda(valoresNFSe?.vISSQN);
        }

        #endregion

        #region Tributação Federal (exceto CBS)

        private static void PreencherTributacaoFederal(DanfseViewModel model, InfDPS infDPS)
        {
            var tribFed = infDPS.valores?.trib?.tribFed;
            var f = model.TributacaoFederal;

            if (tribFed == null) return;

            var pisCofins = tribFed.piscofins;
            bool retido = pisCofins?.tpRetPisCofins == "1";

            f.Irrf = Moeda(tribFed.vRetIRRF);
            f.ContribuicaoPrevidenciariaRetida = Moeda(tribFed.vRetCP);

            double? contribSociais = retido
                ? SomarOpcionais(tribFed.vRetCSLL, pisCofins?.vPis, pisCofins?.vCofins)
                : tribFed.vRetCSLL;

            f.ContribuicoesSociaisRetidas = Moeda(contribSociais);
            f.DescricaoContribuicoesSociaisRetidas = Descricoes.TpRetPisCofins(pisCofins?.tpRetPisCofins);

            f.PisDebitoApuracaoPropria = retido ? Moeda(0) : Moeda(pisCofins?.vPis);
            f.CofinsDebitoApuracaoPropria = retido ? Moeda(0) : Moeda(pisCofins?.vCofins);
            f.MostrarLinhaPisCofins = true;
        }

        #endregion

        #region Tributação IBS/CBS

        private static void PreencherTributacaoIBSCBS(DanfseViewModel model, InfNFSe infNFSe, InfDPS infDPS)
        {
            var declarado = infDPS.IBSCBS;
            var computado = infNFSe.IBSCBS;
            var i = model.TributacaoIBSCBS;

            if (declarado == null || computado == null) return;

            var sitClas = declarado.valores?.trib?.gIBSCBS;
            i.CstCClassTrib = JuntarNaoVazios(" / ", sitClas?.CST, sitClas?.cClassTrib);

            var uf = TabelaUF.ObterUFPorCodigoIbge(computado.cLocalidadeIncid);
            i.IndicadorOperacaoMunicipioIncidencia = JuntarNaoVazios(" / ", declarado.cIndOp, computado.cLocalidadeIncid, computado.xLocalidadeIncid, uf);

            var tribFed = infDPS.valores?.trib?.tribFed;
            double? exclusoesReducoes = SomarOpcionais(
                infDPS.valores?.vDescCondIncond?.vDescIncond,
                infDPS.valores?.vDescCondIncond?.vDescCond,
                computado.valores?.vCalcReeRepRes,
                infNFSe.valores?.vISSQN,
                tribFed?.piscofins?.vPis,
                tribFed?.piscofins?.vCofins);

            i.ExclusoesReducoesBaseCalculo = Moeda(exclusoesReducoes);
            i.BaseCalculoAposExclusoesReducoes = Moeda(computado.valores?.vBC);

            var valoresUF = computado.valores?.uf;
            var valoresMun = computado.valores?.mun;
            var valoresFed = computado.valores?.fed;

            i.ReducaoAliquotaIbsCbs = JuntarNaoVazios(" / ",
                Percentual(valoresUF?.pRedAliqUF), Percentual(valoresMun?.pRedAliqMun), Percentual(valoresFed?.pRedAliqCBS));

            i.AliquotaIbsUfMun = JuntarNaoVazios(" / ", Percentual(valoresUF?.pIBSUF), Percentual(valoresMun?.pIBSMun));

            i.AliquotaEfetivaMunicipalIbs = Percentual(valoresMun?.pAliqEfetMun);
            i.ValorApuradoMunicipalIbs = Moeda(computado.totCIBS?.gIBS?.gIBSMunTot?.vIBSMun);
            i.AliquotaEfetivaEstadualIbs = Percentual(valoresUF?.pAliqEfetUF);
            i.ValorApuradoEstadualIbs = Moeda(computado.totCIBS?.gIBS?.gIBSUFTot?.vIBSUF);
            i.ValorTotalApuradoIbs = Moeda(computado.totCIBS?.gIBS?.vIBSTot);

            i.AliquotaCbs = Percentual(valoresFed?.pCBS);
            i.AliquotaEfetivaCbs = Percentual(valoresFed?.pAliqEfetCBS);
            i.ValorTotalApuradoCbs = Moeda(computado.totCIBS?.gCBS?.vCBS);
        }

        #endregion

        #region Valor Total da NFS-e

        private static void PreencherValorTotal(DanfseViewModel model, InfNFSe infNFSe, InfDPS infDPS)
        {
            var t = model.ValorTotal;
            var valoresNFSe = infNFSe.valores;
            var totCIBS = infNFSe.IBSCBS?.totCIBS;

            t.ValorOperacaoServico = Moeda(infDPS.valores?.vServPrest?.vServ);
            t.DescontoIncondicionado = Moeda(infDPS.valores?.vDescCondIncond?.vDescIncond);
            t.DescontoCondicionado = Moeda(infDPS.valores?.vDescCondIncond?.vDescCond);
            t.TotalRetencoes = Moeda(valoresNFSe?.vTotalRet);
            t.ValorLiquidoNFSe = Moeda(valoresNFSe?.vLiq);
            t.TotalIbsCbs = Moeda(SomarOpcionais(totCIBS?.gIBS?.vIBSTot, totCIBS?.gCBS?.vCBS));
            t.ValorLiquidoNFSeIbsCbs = Moeda(totCIBS?.vTotNF);
        }

        #endregion

        #region Informações Complementares

        private static void PreencherInformacoesComplementares(DanfseViewModel model, InfNFSe infNFSe, InfDPS infDPS)
        {
            var partes = new List<string>();
            var infoCompl = infDPS.serv?.infoCompl;

            if (!string.IsNullOrWhiteSpace(infoCompl?.xInfComp))
                partes.Add("Inf. Cont.: " + infoCompl.xInfComp);

            if (!string.IsNullOrWhiteSpace(infDPS.subst?.chSubstda))
                partes.Add("NFS-e Subst.: " + infDPS.subst.chSubstda);

            if (!string.IsNullOrWhiteSpace(infoCompl?.docRef))
                partes.Add("Doc. Ref.: " + infoCompl.docRef);

            var codObra = infDPS.serv?.obra?.cObra ?? infDPS.serv?.obra?.cCIB;
            if (!string.IsNullOrWhiteSpace(codObra))
                partes.Add("Cod. Obra: " + codObra);

            var inscImob = infDPS.IBSCBS?.imovel?.inscImobFisc ?? infDPS.serv?.obra?.inscImobFisc;
            if (!string.IsNullOrWhiteSpace(inscImob))
                partes.Add("Insc. Imob.: " + inscImob);

            if (!string.IsNullOrWhiteSpace(infDPS.serv?.atvEvento?.idAtvEvt))
                partes.Add("Cod. Evt.: " + infDPS.serv.atvEvento.idAtvEvt);

            if (!string.IsNullOrWhiteSpace(infoCompl?.idDocTec))
                partes.Add("Doc. Tec.: " + infoCompl.idDocTec);

            if (!string.IsNullOrWhiteSpace(infoCompl?.xPed))
                partes.Add("Núm. Ped.: " + infoCompl.xPed);

            if (infoCompl?.gItemPed?.xItemPed?.Count > 0)
                partes.Add("Item Ped.: " + string.Join(", ", infoCompl.gItemPed.xItemPed));

            if (!string.IsNullOrWhiteSpace(infNFSe.xOutInf))
                partes.Add("Inf. A. T. Mun.: " + infNFSe.xOutInf);

            partes.Add(FormatarTotaisAproximadosTributos(infDPS.valores?.trib?.totTrib));

            model.InformacoesComplementares = string.Join(" | ", partes.Where(p => !string.IsNullOrWhiteSpace(p)));
        }

        private static string FormatarTotaisAproximadosTributos(TribTotal totTrib)
        {
            if (totTrib?.vTotTrib != null)
            {
                var v = totTrib.vTotTrib;
                return $"Totais Aproximados dos Tributos cfe. Lei nº 12.741/2012: Federais: {Moeda(v.vTotTribFed)}; Estaduais: {Moeda(v.vTotTribEst)}; Municipais: {Moeda(v.vTotTribMun)}";
            }

            if (totTrib?.pTotTrib != null)
            {
                var p = totTrib.pTotTrib;
                return $"Totais Aproximados dos Tributos cfe. Lei nº 12.741/2012: Federais: {Percentual(p.pTotTribFed)}; Estaduais: {Percentual(p.pTotTribEst)}; Municipais: {Percentual(p.pTotTribMun)}";
            }

            return "Totais Aproximados dos Tributos cfe. Lei nº 12.741/2012: Federais: -; Estaduais: -; Municipais: -";
        }

        #endregion

        #region Auxiliares de formatação

        private static string Moeda(double? valor) => valor.HasValue ? "R$ " + valor.Value.ToString("#,0.00", Formatador.Cultura) : SemInformacao;

        private static string Percentual(double? valor) => valor.HasValue ? valor.Value.ToString("#,0.00", Formatador.Cultura) + "%" : SemInformacao;

        private static string Traco(string valor) => string.IsNullOrWhiteSpace(valor) ? SemInformacao : valor;

        private static double? SomarOpcionais(params double?[] valores)
        {
            var presentes = valores.Where(v => v.HasValue).Select(v => v.Value).ToList();
            return presentes.Count > 0 ? presentes.Sum() : (double?)null;
        }

        private static bool AlgumPresente(params object[] valores)
        {
            foreach (var v in valores)
            {
                if (v is string s && !string.IsNullOrWhiteSpace(s)) return true;
                if (v is double) return true;
            }

            return false;
        }

        private static string JuntarNaoVazios(string separador, params string[] partes)
        {
            var naoVazias = partes.Where(p => !string.IsNullOrWhiteSpace(p));
            return string.Join(separador, naoVazias);
        }

        private static string ConcatenarEndereco(Endereco end)
        {
            if (end == null) return SemInformacao;
            return Traco(JuntarNaoVazios(", ", end.xLgr, end.nro, end.xCpl, end.xBairro));
        }

        private static string MunicipioUf(Endereco end, Func<string, string> resolverMunicipio)
        {
            if (end == null) return SemInformacao;

            if (end.endNac != null)
            {
                var nome = resolverMunicipio(end.endNac.cMun);
                var uf = TabelaUF.ObterUFPorCodigoIbge(end.endNac.cMun);
                return Traco(Formatador.FormatarMunicipioUf(nome, uf, " / "));
            }

            if (end.endExt != null)
            {
                return Traco(Formatador.FormatarMunicipioUf(end.endExt.xCidade, end.endExt.xEstProvReg, " / "));
            }

            return SemInformacao;
        }

        private static string CodigoIbgeCep(Endereco end)
        {
            if (end == null) return SemInformacao;

            if (end.endNac != null)
                return Traco(JuntarNaoVazios(" / ", end.endNac.cMun, Formatador.FormatarCEP(end.endNac.CEP)));

            if (end.endExt != null)
                return Traco(JuntarNaoVazios(" / ", end.endExt.cPais, end.endExt.cEndPost));

            return SemInformacao;
        }

        #endregion
    }
}
