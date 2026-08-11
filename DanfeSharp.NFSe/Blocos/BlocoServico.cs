using DanfeSharp.NFSe.Elementos;
using DanfeSharp.NFSe.Interno;
using DanfeSharp.NFSe.Modelo;

namespace DanfeSharp.NFSe.Blocos
{
    /// <summary>
    /// Bloco do Serviço Prestado (item 2.1.7 do manual).
    /// </summary>
    internal class BlocoServico : DanfseBlocoBase
    {
        public BlocoServico(DanfseViewModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {
            var servico = viewModel.Servico;

            AdicionarLinhaComTitulo("Serviço Prestado")
                .ComCampoDanfse("Código de Tributação Nacional / Municipal", servico.CodigoTributacao)
                .ComCampoDanfse("Código da NBS", servico.CodigoNBS)
                .ComCampoDanfse("Local da Prestação / Sigla UF / País", servico.LocalPrestacao)
                .ComLargurasIguais();

            Adicionar(new CampoMultilinhaDanfse(null, servico.DescricaoCodigoTributacao, estilo) { Height = 6, Width = Width });
            Adicionar(new CampoMultilinhaDanfse("Descrição do Serviço", servico.DescricaoServico, estilo) { Height = 20, Width = Width });
        }
    }
}
