using System;
using System.Drawing;
using DanfeSharp.Graphics;
using QRCoder;

namespace DanfeSharp.NFSe.Elementos
{
    /// <summary>
    /// Desenha um código de barras bidimensional (QR Code) para consulta pública da NFS-e,
    /// conforme o item 2.4.3 do manual (dimensões mínimas de 1,52 cm x 1,52 cm).
    /// </summary>
    internal class QRCodeElemento : DanfeSharp.ElementoBase
    {
        private readonly bool[][] _modulos;
        private readonly int _quantidadeModulos;

        public QRCodeElemento(string conteudo, DanfeSharp.Estilo estilo) : base(estilo)
        {
            if (string.IsNullOrWhiteSpace(conteudo)) throw new ArgumentException(nameof(conteudo));

            var dadosQRCode = QRCodeGenerator.GenerateQrCode(conteudo, QRCodeGenerator.ECCLevel.M);
            var matriz = dadosQRCode.ModuleMatrix;

            _quantidadeModulos = matriz.Count;
            _modulos = new bool[_quantidadeModulos][];

            for (int linha = 0; linha < _quantidadeModulos; linha++)
            {
                _modulos[linha] = new bool[_quantidadeModulos];

                for (int coluna = 0; coluna < _quantidadeModulos; coluna++)
                {
                    _modulos[linha][coluna] = matriz[linha][coluna];
                }
            }
        }

        public override bool PossuiContono => false;

        public override void Draw(Gfx gfx)
        {
            base.Draw(gfx);

            float lado = Math.Min(Width, Height);
            float tamanhoModulo = lado / _quantidadeModulos;
            float offsetX = X + (Width - lado) / 2F;
            float offsetY = Y + (Height - lado) / 2F;

            for (int linha = 0; linha < _quantidadeModulos; linha++)
            {
                for (int coluna = 0; coluna < _quantidadeModulos; coluna++)
                {
                    if (!_modulos[linha][coluna]) continue;

                    var retanguloModulo = new RectangleF(
                        offsetX + coluna * tamanhoModulo,
                        offsetY + linha * tamanhoModulo,
                        tamanhoModulo,
                        tamanhoModulo);

                    gfx.DrawRectangle(retanguloModulo);
                }
            }

            gfx.Fill();
        }
    }
}
