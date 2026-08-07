using DanfeSharp;
using DanfeSharp.Graphics;

namespace DanfeSharp.NFSe.Elementos
{
    /// <summary>
    /// Campo multilinha do DANFSe, sem contorno (o modelo real do DANFSe não usa grade interna) e sem
    /// caixa alta forçada no título (itens 2.4.2/2.4.4 do manual). O conteúdo já é normal por padrão em
    /// <see cref="CampoMultilinha"/>.
    /// </summary>
    internal class CampoMultilinhaDanfse : CampoMultilinha
    {
        public CampoMultilinhaDanfse(string cabecalho, string conteudo, Estilo estilo, AlinhamentoHorizontal alinhamentoHorizontalConteudo = AlinhamentoHorizontal.Esquerda)
            : base(cabecalho, conteudo, estilo, alinhamentoHorizontalConteudo)
        {
        }

        public override bool PossuiContono => false;

        protected override void DesenharCabecalho(Gfx gfx)
        {
            if (string.IsNullOrWhiteSpace(Cabecalho)) return;

            // Item 2.4.2 do manual: título do campo em negrito.
            var fonteNegrito = Estilo.CriarFonteNegrito(Estilo.FonteCampoCabecalho.Tamanho);

            gfx.DrawString(Cabecalho, RetanguloDesenhvael, fonteNegrito, AlinhamentoHorizontal.Esquerda, AlinhamentoVertical.Topo);
        }
    }
}
