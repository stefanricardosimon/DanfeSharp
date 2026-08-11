using DanfeSharp.NFSe.Elementos;
using DanfeSharp.NFSe.Interno;
using DanfeSharp.NFSe.Modelo;

namespace DanfeSharp.NFSe.Blocos
{
    /// <summary>
    /// Bloco de Canhoto (opcional), item 2.1.13 do manual. Ao contrário dos demais blocos do DANFSe,
    /// o canhoto é desenhado em uma tabela com contorno (conforme o modelo real).
    /// </summary>
    internal class BlocoCanhoto : DanfseBlocoBase
    {
        public BlocoCanhoto(DanfseViewModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {
            AdicionarLinhaCampos()
                .ComCampoDanfse("Data Cientificação:", null, AlinhamentoHorizontal.Esquerda, caixaAlta: true, comBorda: true)
                .ComCampoDanfse("Identificação e Assinatura", null, AlinhamentoHorizontal.Esquerda, caixaAlta: true, comBorda: true)
                .ComCampoDanfse("Nº NFS-e / Chave NFS-e", viewModel.NumeroChaveNFSeCanhoto, AlinhamentoHorizontal.Esquerda, caixaAlta: true, comBorda: true)
                .ComLarguras(25, 25, 50);
        }

        protected override bool LinhaSuperior => false;
    }
}
