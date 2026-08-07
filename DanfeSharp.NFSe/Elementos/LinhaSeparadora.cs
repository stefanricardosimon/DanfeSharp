using System;
using System.Drawing;
using DanfeSharp;
using DanfeSharp.Graphics;

namespace DanfeSharp.NFSe.Elementos
{
    /// <summary>
    /// Linha horizontal sólida, usada para separar os blocos de campos do DANFSe (o modelo real não usa
    /// grade interna completa, apenas linhas separadoras entre blocos, conforme o Anexo I do manual).
    /// </summary>
    internal class LinhaSeparadora : DrawableBase
    {
        public float Espessura { get; }

        public LinhaSeparadora(float espessura = 0.5F)
        {
            Espessura = espessura;
        }

        public override void Draw(Gfx gfx)
        {
            base.Draw(gfx);

            gfx.SetLineWidth(Espessura);
            gfx.PrimitiveComposer.DrawLine(new PointF(X, Y).ToPointMeasure(), new PointF(X + Width, Y).ToPointMeasure());
            gfx.Stroke();
        }

        public override float Height { get => Espessura; set => throw new NotSupportedException(); }
    }
}
