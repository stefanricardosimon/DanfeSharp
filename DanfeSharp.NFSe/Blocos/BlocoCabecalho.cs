using org.pdfclown.documents.contents.xObjects;
using DanfeSharp.NFSe.Elementos;
using DanfeSharp.NFSe.Interno;
using DanfeSharp.NFSe.Modelo;

namespace DanfeSharp.NFSe.Blocos
{
    /// <summary>
    /// Bloco de cabeçalho do DANFSe (item 2.4.3 do manual).
    /// </summary>
    internal class BlocoCabecalho : DanfseBlocoBase
    {
        private readonly CabecalhoDanfse _cabecalhoDanfse;

        public BlocoCabecalho(DanfseViewModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {
            _cabecalhoDanfse = new CabecalhoDanfse(viewModel, estilo) { Height = 12F };
            Adicionar(_cabecalhoDanfse);
        }

        public XObject Logo
        {
            get => _cabecalhoDanfse.Logo;
            set => _cabecalhoDanfse.Logo = value;
        }

        protected override bool LinhaSuperior => false;
    }
}
