using DanfeSharp;
using DanfeSharp.NFSe.Elementos;
using DanfeSharp.NFSe.Modelo;

namespace DanfeSharp.NFSe.Blocos
{
    /// <summary>
    /// Bloco da Tributação Federal (exceto CBS), item 2.1.9 do manual.
    /// </summary>
    internal class BlocoTributacaoFederal : DanfseBlocoBase
    {
        public BlocoTributacaoFederal(DanfseViewModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {
            var f = viewModel.TributacaoFederal;

            AdicionarLinhaComTitulo("Tributação Federal (Exceto CBS)")
                .ComCampoDanfse("IRRF", f.Irrf)
                .ComCampoDanfse("Contribuição Previdenciária - Retida", f.ContribuicaoPrevidenciariaRetida)
                .ComCampoDanfse("Contribuições Sociais - Retidas", f.ContribuicoesSociaisRetidas)
                .ComLargurasIguais();

            if (f.MostrarLinhaPisCofins)
            {
                AdicionarLinhaCampos()
                    .ComCampoDanfse("PIS - Débito Apuração Própria", f.PisDebitoApuracaoPropria)
                    .ComCampoDanfse("COFINS - Débito Apuração Própria", f.CofinsDebitoApuracaoPropria)
                    .ComCampoDanfse("Descrição Contrib. Sociais - Retidas", f.DescricaoContribuicoesSociaisRetidas)
                    .ComLarguras(25, 25, 50);
            }
        }
    }
}
