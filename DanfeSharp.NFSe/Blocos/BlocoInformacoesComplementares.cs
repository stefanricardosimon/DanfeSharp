using DanfeSharp;
using DanfeSharp.NFSe.Elementos;
using DanfeSharp.NFSe.Modelo;

namespace DanfeSharp.NFSe.Blocos
{
    /// <summary>
    /// Bloco de Informações Complementares, item 2.1.12 do manual.
    /// </summary>
    internal class BlocoInformacoesComplementares : DanfseBlocoBase
    {
        public BlocoInformacoesComplementares(DanfseViewModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {
            var linha = AdicionarLinhaCampos();
            linha.Elementos.Add(new TituloBlocoCampo("Informações Complementares", estilo));
            linha.ComLarguras(0);

            Adicionar(new CampoMultilinhaDanfse(null, viewModel.InformacoesComplementares, estilo) { Height = 20, Width = Width });
        }
    }
}
