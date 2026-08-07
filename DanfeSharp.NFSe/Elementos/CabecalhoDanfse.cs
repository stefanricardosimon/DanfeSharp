using System.Drawing;
using DanfeSharp;
using DanfeSharp.Graphics;
using DanfeSharp.NFSe.Modelo;
using org.pdfclown.documents.contents.colorSpaces;
using org.pdfclown.documents.contents.xObjects;

namespace DanfeSharp.NFSe.Elementos
{
    /// <summary>
    /// Cabeçalho do DANFSe: logomarca, título/aviso de homologação e identificação do município/ambiente
    /// (item 2.4.3 do manual).
    /// </summary>
    internal class CabecalhoDanfse : ElementoBase
    {
        public DanfseViewModel ViewModel { get; }
        public XObject Logo { get; set; }

        public CabecalhoDanfse(DanfseViewModel viewModel, Estilo estilo) : base(estilo)
        {
            ViewModel = viewModel;
        }

        public override bool PossuiContono => false;

        public override void Draw(Gfx gfx)
        {
            Sombreamento.Desenhar(gfx, BoundingBox);
            base.Draw(gfx);

            var r = BoundingBox.InflatedRetangle(0.5F);

            var rLogo = new RectangleF(r.X, r.Y, r.Width * 0.20F, r.Height);
            var rDireita = new RectangleF(r.Right - r.Width * 0.24F, r.Y, r.Width * 0.24F, r.Height);
            var rCentro = new RectangleF(rLogo.Right, r.Y, rDireita.X - rLogo.Right, r.Height);

            if (Logo != null)
            {
                gfx.ShowXObject(Logo, rLogo);
            }
            else
            {
                var rPlaceholder = rLogo.InflatedRetangle(2F);
                gfx.PrimitiveComposer.BeginLocalState();
                gfx.PrimitiveComposer.SetLineDash(new org.pdfclown.documents.contents.LineDash(new double[] { 1.5, 1.5 }));
                gfx.StrokeRectangle(rPlaceholder, 0.5F);
                gfx.PrimitiveComposer.End();
                gfx.DrawString("Logomarca", rPlaceholder, Estilo.CriarFonteRegular(7), AlinhamentoHorizontal.Centro, AlinhamentoVertical.Centro);
            }

            var linhaInferior = new LinhaSeparadora(1F);
            linhaInferior.SetPosition(BoundingBox.X, BoundingBox.Bottom);
            linhaInferior.Width = BoundingBox.Width;
            linhaInferior.Draw(gfx);

            var fTitulo = Estilo.CriarFonteNegrito(9);

            var tsCentro = new TextStack(rCentro) { LineHeightScale = 1.1F };
            tsCentro.AddLine("DANFSe v2.0", fTitulo);
            tsCentro.AddLine("Documento Auxiliar da NFS-e", fTitulo);
            tsCentro.Draw(gfx);

            if (ViewModel.Homologacao)
            {
                var rAviso = new RectangleF(rCentro.X, rCentro.Bottom - fTitulo.AlturaLinha, rCentro.Width, fTitulo.AlturaLinha);

                gfx.PrimitiveComposer.BeginLocalState();
                gfx.PrimitiveComposer.SetFillColor(new DeviceRGBColor(1, 0, 0));
                gfx.DrawString("NFS-e SEM VALIDADE JURÍDICA", rAviso, fTitulo, AlinhamentoHorizontal.Centro, AlinhamentoVertical.Base);
                gfx.PrimitiveComposer.End();
            }

            var fDireita1 = Estilo.CriarFonteRegular(8);
            var fDireita2 = Estilo.CriarFonteRegular(6);

            var tsDireita = new TextStack(rDireita) { AlinhamentoHorizontal = AlinhamentoHorizontal.Esquerda, AlinhamentoVertical = AlinhamentoVertical.Topo, LineHeightScale = 1.15F };
            tsDireita.AddLine("Município: " + ViewModel.MunicipioEmitente, fDireita1);
            tsDireita.AddLine("Ambiente Gerador: " + ViewModel.AmbienteGerador, fDireita2);
            tsDireita.AddLine("Tipo de Ambiente: " + ViewModel.TipoAmbiente, fDireita2);
            tsDireita.Draw(gfx);
        }
    }
}
