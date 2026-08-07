using DanfeSharp;
using DanfeSharp.NFSe.Elementos;
using DanfeSharp.NFSe.Modelo;

namespace DanfeSharp.NFSe.Blocos
{
    /// <summary>
    /// Bloco do Valor Total da NFS-e, item 2.1.11 do manual.
    /// </summary>
    internal class BlocoValorTotal : DanfseBlocoBase
    {
        public BlocoValorTotal(DanfseViewModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {
            var t = viewModel.ValorTotal;

            AdicionarLinhaComTitulo("Valor Total da NFS-e")
                .ComCampoDanfse("Valor da Operação / Serviço", t.ValorOperacaoServico, AlinhamentoHorizontal.Esquerda, caixaAlta: true)
                .ComCampoDanfse("Desconto Incondicionado", t.DescontoIncondicionado)
                .ComCampoDanfse("Desconto Condicionado", t.DescontoCondicionado)
                .ComLargurasIguais();

            AdicionarLinhaCampos()
                .ComCampoDanfse("Total das Retenções (ISSQN / Federais)", t.TotalRetencoes)
                .ComCampoDanfse("Valor Líquido da NFS-e", t.ValorLiquidoNFSe, AlinhamentoHorizontal.Esquerda, caixaAlta: true)
                .ComCampoDanfse("Total do IBS/CBS", t.TotalIbsCbs)
                .ComCampoDanfse("Valor Líquido da NFS-e + IBS/CBS", t.ValorLiquidoNFSeIbsCbs, AlinhamentoHorizontal.Esquerda, caixaAlta: true, comSombreamento: true)
                .ComLargurasIguais();
        }
    }
}
