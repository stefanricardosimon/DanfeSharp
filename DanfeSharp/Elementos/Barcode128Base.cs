using System.Collections.Generic;
using System.Drawing;
using DanfeSharp.Graphics;

namespace DanfeSharp
{
    /// <summary>
    /// Base para o desenho de código de barra em padrão Code 128.
    /// </summary>
    internal abstract class Barcode128Base : ElementoBase
    {
        protected static byte[][] Dic;

        public static readonly float MargemVertical = 2;

        /// <summary>
        /// Margem horizontal mínima (zona de silêncio) usada quando <see cref="Largura"/>
        /// não é definida explicitamente, fazendo o código de barras ocupar toda a largura disponível.
        /// </summary>
        private const float MargemHorizontalMinima = 2F;

        /// <summary>
        /// Valor de <see cref="Largura"/> que indica que o código de barras deve ocupar
        /// toda a largura disponível do elemento, em vez de uma largura fixa.
        /// </summary>
        public const float LarguraAutomatica = -1F;

        /// <summary>
        /// Código a ser desenhado.
        /// </summary>
        public string Code { get; protected set; }

        /// <summary>
        /// Largura do código de barras.
        /// </summary>
        public float Largura { get; set; }

        static Barcode128Base()
        {
            Dic = new byte[][]
            {
                new byte[] { 2,1,2,2,2,2},
                new byte[] { 2,2,2,1,2,2},
                new byte[] { 2,2,2,2,2,1},
                new byte[] { 1,2,1,2,2,3},
                new byte[] { 1,2,1,3,2,2},
                new byte[] { 1,3,1,2,2,2},
                new byte[] { 1,2,2,2,1,3},
                new byte[] { 1,2,2,3,1,2},
                new byte[] { 1,3,2,2,1,2},
                new byte[] { 2,2,1,2,1,3},
                new byte[] { 2,2,1,3,1,2},
                new byte[] { 2,3,1,2,1,2},
                new byte[] { 1,1,2,2,3,2},
                new byte[] { 1,2,2,1,3,2},
                new byte[] { 1,2,2,2,3,1},
                new byte[] { 1,1,3,2,2,2},
                new byte[] { 1,2,3,1,2,2},
                new byte[] { 1,2,3,2,2,1},
                new byte[] { 2,2,3,2,1,1},
                new byte[] { 2,2,1,1,3,2},
                new byte[] { 2,2,1,2,3,1},
                new byte[] { 2,1,3,2,1,2},
                new byte[] { 2,2,3,1,1,2},
                new byte[] { 3,1,2,1,3,1},
                new byte[] { 3,1,1,2,2,2},
                new byte[] { 3,2,1,1,2,2},
                new byte[] { 3,2,1,2,2,1},
                new byte[] { 3,1,2,2,1,2},
                new byte[] { 3,2,2,1,1,2},
                new byte[] { 3,2,2,2,1,1},
                new byte[] { 2,1,2,1,2,3},
                new byte[] { 2,1,2,3,2,1},
                new byte[] { 2,3,2,1,2,1},
                new byte[] { 1,1,1,3,2,3},
                new byte[] { 1,3,1,1,2,3},
                new byte[] { 1,3,1,3,2,1},
                new byte[] { 1,1,2,3,1,3},
                new byte[] { 1,3,2,1,1,3},
                new byte[] { 1,3,2,3,1,1},
                new byte[] { 2,1,1,3,1,3},
                new byte[] { 2,3,1,1,1,3},
                new byte[] { 2,3,1,3,1,1},
                new byte[] { 1,1,2,1,3,3},
                new byte[] { 1,1,2,3,3,1},
                new byte[] { 1,3,2,1,3,1},
                new byte[] { 1,1,3,1,2,3},
                new byte[] { 1,1,3,3,2,1},
                new byte[] { 1,3,3,1,2,1},
                new byte[] { 3,1,3,1,2,1},
                new byte[] { 2,1,1,3,3,1},
                new byte[] { 2,3,1,1,3,1},
                new byte[] { 2,1,3,1,1,3},
                new byte[] { 2,1,3,3,1,1},
                new byte[] { 2,1,3,1,3,1},
                new byte[] { 3,1,1,1,2,3},
                new byte[] { 3,1,1,3,2,1},
                new byte[] { 3,3,1,1,2,1},
                new byte[] { 3,1,2,1,1,3},
                new byte[] { 3,1,2,3,1,1},
                new byte[] { 3,3,2,1,1,1},
                new byte[] { 3,1,4,1,1,1},
                new byte[] { 2,2,1,4,1,1},
                new byte[] { 4,3,1,1,1,1},
                new byte[] { 1,1,1,2,2,4},
                new byte[] { 1,1,1,4,2,2},
                new byte[] { 1,2,1,1,2,4},
                new byte[] { 1,2,1,4,2,1},
                new byte[] { 1,4,1,1,2,2},
                new byte[] { 1,4,1,2,2,1},
                new byte[] { 1,1,2,2,1,4},
                new byte[] { 1,1,2,4,1,2},
                new byte[] { 1,2,2,1,1,4},
                new byte[] { 1,2,2,4,1,1},
                new byte[] { 1,4,2,1,1,2},
                new byte[] { 1,4,2,2,1,1},
                new byte[] { 2,4,1,2,1,1},
                new byte[] { 2,2,1,1,1,4},
                new byte[] { 4,1,3,1,1,1},
                new byte[] { 2,4,1,1,1,2},
                new byte[] { 1,3,4,1,1,1},
                new byte[] { 1,1,1,2,4,2},
                new byte[] { 1,2,1,1,4,2},
                new byte[] { 1,2,1,2,4,1},
                new byte[] { 1,1,4,2,1,2},
                new byte[] { 1,2,4,1,1,2},
                new byte[] { 1,2,4,2,1,1},
                new byte[] { 4,1,1,2,1,2},
                new byte[] { 4,2,1,1,1,2},
                new byte[] { 4,2,1,2,1,1},
                new byte[] { 2,1,2,1,4,1},
                new byte[] { 2,1,4,1,2,1},
                new byte[] { 4,1,2,1,2,1},
                new byte[] { 1,1,1,1,4,3},
                new byte[] { 1,1,1,3,4,1},
                new byte[] { 1,3,1,1,4,1},
                new byte[] { 1,1,4,1,1,3},
                new byte[] { 1,1,4,3,1,1},
                new byte[] { 4,1,1,1,1,3},
                new byte[] { 4,1,1,3,1,1},
                new byte[] { 1,1,3,1,4,1},
                new byte[] { 1,1,4,1,3,1},
                new byte[] { 3,1,1,1,4,1},
                new byte[] { 4,1,1,1,3,1},
                new byte[] { 2,1,1,4,1,2},
                new byte[] { 2,1,1,2,1,4},
                new byte[] { 2,1,1,2,3,2},
                new byte[] { 2,3,3,1,1,1,2}
            };
        }

        public Barcode128Base(Estilo estilo, float largura) : base(estilo)
        {
            Largura = largura;
        }

        /// <summary>
        /// Monta a lista de valores (símbolos) que serão desenhados, incluindo o caractere de início, o dígito verificador e o caractere de parada.
        /// </summary>
        protected abstract List<byte> MontarCodeBytes();

        private void DrawBarcode(RectangleF rect, Gfx gfx)
        {
            List<byte> codeBytes = MontarCodeBytes();

            float n = codeBytes.Count * 11 + 2;
            float w = rect.Width / n;

            float x = 0;

            for (int i = 0; i < codeBytes.Count; i++)
            {
                byte[] pt = Dic[codeBytes[i]];

                for (int i2 = 0; i2 < pt.Length; i2++)
                {
                    if (i2 % 2 == 0)
                    {
                        gfx.DrawRectangle(rect.X + x, rect.Y, w * pt[i2], rect.Height);
                    }

                    x += w * pt[i2];
                }
            }

            gfx.Fill();
        }

        public override void Draw(Gfx gfx)
        {
            base.Draw(gfx);

            float larguraBarras = Largura == LarguraAutomatica ? Width - 2 * MargemHorizontalMinima : Largura;
            float w2 = (Width - larguraBarras) / 2F;
            DrawBarcode(new RectangleF(X + w2, Y + MargemVertical, larguraBarras, Height - 2 * MargemVertical), gfx);
        }
    }
}
