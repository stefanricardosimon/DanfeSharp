using DanfeSharp;
using DanfeSharp.NFSe.Elementos;
using DanfeSharp.NFSe.Modelo;

namespace DanfeSharp.NFSe.Blocos
{
    /// <summary>
    /// Bloco da Tributação Municipal (ISSQN), item 2.1.8 do manual.
    /// </summary>
    internal class BlocoTributacaoMunicipal : DanfseBlocoBase
    {
        public BlocoTributacaoMunicipal(DanfseViewModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {
            var t = viewModel.TributacaoMunicipal;

            if (!t.SujeitoAoIssqn)
            {
                AdicionarLinhaCampos().ComCampoDanfse(null, "TRIBUTAÇÃO MUNICIPAL (ISSQN) - OPERAÇÃO NÃO SUJEITA AO ISSQN", AlinhamentoHorizontal.Centro).ComLarguras(0);
                return;
            }

            AdicionarLinhaComTitulo("Tributação Municipal (ISSQN)")
                .ComCampoDanfse("Tipo de Tributação do ISSQN", t.TipoTributacao)
                .ComCampoDanfse("Município / Sigla UF / País de Incidência do ISSQN", t.MunicipioUfPaisIncidencia)
                .ComLarguras(25, 25, 50);

            if (t.MostrarLinhaRegimeImunidadeSuspensao)
            {
                AdicionarLinhaCampos()
                    .ComCampoDanfse("Regime Especial de Tributação do ISSQN", t.RegimeEspecial)
                    .ComCampoDanfse("Tipo de Imunidade do ISSQN", t.TipoImunidade)
                    .ComCampoDanfse("Suspensão da Exigibilidade do ISSQN", t.SuspensaoExigibilidade)
                    .ComCampoDanfse("Número Processo Suspensão", t.NumeroProcessoSuspensao)
                    .ComLargurasIguais();
            }

            if (t.MostrarLinhaBeneficioDeducoes)
            {
                AdicionarLinhaCampos()
                    .ComCampoDanfse("Benefício Municipal", t.BeneficioMunicipal)
                    .ComCampoDanfse("Cálculo do BM", t.CalculoBM)
                    .ComCampoDanfse("Total Deduções/Reduções", t.TotalDeducoesReducoes)
                    .ComCampoDanfse("Desconto Incondicionado", t.DescontoIncondicionado)
                    .ComLargurasIguais();
            }

            AdicionarLinhaCampos()
                .ComCampoDanfse("BC ISSQN", t.BaseCalculoIssqn)
                .ComCampoDanfse("Alíquota Aplicada", t.AliquotaAplicada)
                .ComCampoDanfse("Retenção do ISSQN", t.RetencaoIssqn)
                .ComCampoDanfse("ISSQN Apurado", t.IssqnApurado)
                .ComLargurasIguais();
        }
    }
}
