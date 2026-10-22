using DanfeSharp.Modelo;

namespace DanfeSharp.Blocos
{
    /// <summary>
    /// Bloco "Total dos produtos e total da nota" (NT 2026.010).
    /// </summary>
    internal class BlocoTotalProdutosNota : BlocoBase
    {
        public BlocoTotalProdutosNota(DanfeViewModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {
            var m = ViewModel.CalculoImposto;

            AdicionarLinhaCampos()
            .ComCampoNumerico("Valor total dos produtos", m.ValorTotalProdutos)
            .ComCampoNumerico("Valor do frete", m.ValorFrete)
            .ComCampoNumerico("Valor do seguro", m.ValorSeguro)
            .ComCampoNumerico("Desconto", m.Desconto)
            .ComCampoNumerico("Outras despesas", m.OutrasDespesas)
            .ComLargurasIguais();

            AdicionarLinhaCampos()
            .ComCampoNumerico("Valor total da nota", m.ValorTotalNota)
            .ComLargurasIguais();
        }

        public override PosicaoBloco Posicao => PosicaoBloco.Topo;
        public override string Cabecalho => "Total dos produtos e total da nota";
    }

    /// <summary>
    /// Bloco "Total do ICMS / IPI" (NT 2026.010). As linhas de FCP, DIFAL e ICMS monofásico são facultativas e só
    /// exibem os campos existentes no XML.
    /// </summary>
    internal class BlocoTotalIcmsIpi : BlocoBase
    {
        public BlocoTotalIcmsIpi(DanfeViewModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {
            var m = ViewModel.CalculoImposto;

            AdicionarLinhaCampos()
            .ComCampoNumerico("Base de cálculo do ICMS", m.BaseCalculoIcms)
            .ComCampoNumerico("Valor do ICMS", m.ValorIcms)
            .ComCampoNumerico("Base de cálculo do ICMS ST", m.BaseCalculoIcmsSt)
            .ComCampoNumerico("Valor do ICMS ST", m.ValorIcmsSt)
            .ComCampoNumerico("Valor do IPI", m.ValorIpi)
            .ComLargurasIguais();

            if (ViewModel.ExibirIcmsInterestadual)
            {
                AdicionarLinhaCamposFacultativos(
                    ("Valor do FCP", m.vFCP, 2),
                    ("Valor do FCP retido por ST", m.vFCPST, 2),
                    ("Valor do DIFAL na UF de destino", m.vICMSUFDest, 2),
                    ("Valor do FCP na UF de destino", m.vFCPUFDest, 2));
            }

            AdicionarLinhaCamposFacultativos(
                ("BC do ICMS monofásico", m.QuantidadeIcmsMonofasico, 4),
                ("Valor do ICMS monofásico", m.ValorIcmsMonofasico, 2),
                ("BC do ICMS monofásico por retenção", m.QuantidadeIcmsMonofasicoRetencao, 4),
                ("Valor do ICMS monofásico por retenção", m.ValorIcmsMonofasicoRetencao, 2));
        }

        public override PosicaoBloco Posicao => PosicaoBloco.Topo;
        public override string Cabecalho => "Total do ICMS / IPI";
    }

    /// <summary>
    /// Bloco "Total do IBS / CBS / IS" (NT 2026.010). Os campos principais são sempre apresentados, vazios quando
    /// o XML não informa o grupo correspondente. A linha de monofasia é facultativa.
    /// </summary>
    internal class BlocoTotalIbsCbsIs : BlocoBase
    {
        public BlocoTotalIbsCbsIs(DanfeViewModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {
            var m = ViewModel.CalculoImposto;

            AdicionarLinhaCampos()
            .ComCampoNumerico("Valor da CBS", m.ValorCbs)
            .ComCampoNumerico("Valor do IBS UF", m.ValorIbsUf)
            .ComCampoNumerico("Valor do IBS Município", m.ValorIbsMun)
            .ComCampoNumerico("Valor do Imposto Seletivo", m.ValorIs)
            .ComLargurasIguais();

            AdicionarLinhaCamposFacultativos(
                ("Valor do IBS monofásico", m.ValorIbsMonofasico, 2),
                ("Valor da CBS monofásica", m.ValorCbsMonofasica, 2),
                ("Valor do IBS monofásico por retenção", m.ValorIbsMonofasicoRetencao, 2),
                ("Valor da CBS monofásica por retenção", m.ValorCbsMonofasicaRetencao, 2));
        }

        public override PosicaoBloco Posicao => PosicaoBloco.Topo;
        public override string Cabecalho => "Total do IBS / CBS / IS";
    }
}
