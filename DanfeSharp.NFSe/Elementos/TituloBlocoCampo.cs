using DanfeSharp;
using DanfeSharp.Graphics;

namespace DanfeSharp.NFSe.Elementos
{
    /// <summary>
    /// Título de um bloco de campos, ocupando a primeira coluna da primeira linha do bloco (em vez de
    /// uma linha própria), com sombreamento em cinza claro, conforme o modelo real do Anexo I: título e
    /// título/conteúdo dos primeiros campos aparecem na mesma linha (itens 2.2.3/2.4.1 do manual).
    /// </summary>
    internal class TituloBlocoCampo : ElementoBase
    {
        public string Texto { get; }

        public TituloBlocoCampo(string texto, Estilo estilo) : base(estilo)
        {
            Texto = texto;
        }

        public override bool PossuiContono => false;

        public override void Draw(Gfx gfx)
        {
            Sombreamento.Desenhar(gfx, BoundingBox);
            base.Draw(gfx);

            var texto = CasingHelper.CaixaAltaComNFSe(Texto);

            // Item 2.4.1 do manual: título do bloco em negrito. Constrói a fonte em negrito diretamente
            // (Estilo.CriarFonteNegrito já é pública) em vez de depender de alterações no Estilo do DANFE.
            var fonteNegrito = Estilo.CriarFonteNegrito(Estilo.FonteBlocoCabecalho.Tamanho);

            var area = BoundingBox.InflatedRetangle(Estilo.PaddingSuperior, Estilo.PaddingInferior, Estilo.PaddingHorizontal);
            gfx.DrawString(texto, area, fonteNegrito, AlinhamentoHorizontal.Esquerda, AlinhamentoVertical.Topo);
        }
    }
}
