using System.Drawing;
using DanfeSharp.Graphics;
using org.pdfclown.documents.contents.colorSpaces;

namespace DanfeSharp.NFSe.Elementos
{
    /// <summary>
    /// Sombreamento em cinza claro (5% de densidade), usado no cabeçalho, nos títulos dos blocos de
    /// campos e nos campos "Emitente da NFS-e" e "Valor Líquido da NFS-e + IBS/CBS" (item 2.2.3 do manual).
    /// </summary>
    internal static class Sombreamento
    {
        public static readonly DeviceRGBColor Cor = new DeviceRGBColor(0.95, 0.95, 0.95);

        /// <summary>
        /// Mesma cor de <see cref="Cor"/>, como <see cref="Color"/> do GDI+ (usada para compor a
        /// logomarca padrão sobre um fundo da mesma cor do sombreamento do cabeçalho).
        /// </summary>
        public static readonly System.Drawing.Color CorGdi = System.Drawing.Color.FromArgb(242, 242, 242);

        public static void Desenhar(Gfx gfx, RectangleF area)
        {
            gfx.PrimitiveComposer.BeginLocalState();
            gfx.PrimitiveComposer.SetFillColor(Cor);
            gfx.DrawRectangle(area);
            gfx.Fill();
            gfx.PrimitiveComposer.End();
        }
    }
}
