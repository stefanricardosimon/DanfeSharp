using DanfeSharp.NFSe.Elementos;
using DanfeSharp.NFSe.Interno;
using DanfeSharp.NFSe.Modelo;

namespace DanfeSharp.NFSe.Blocos
{
    /// <summary>
    /// Bloco do Destinatário da Operação (item 2.1.5 do manual).
    /// </summary>
    internal class BlocoDestinatario : DanfseBlocoBase
    {
        public BlocoDestinatario(DanfseViewModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {
            var destinatario = viewModel.Destinatario;

            if (viewModel.DestinatarioEhTomador)
            {
                AdicionarLinhaCampos().ComCampoDanfse(null, "O DESTINATÁRIO É O PRÓPRIO TOMADOR/ADQUIRENTE DA OPERAÇÃO", AlinhamentoHorizontal.Centro).ComLarguras(0);
                return;
            }

            if (!destinatario.Identificado)
            {
                AdicionarLinhaCampos().ComCampoDanfse(null, "DESTINATÁRIO DA OPERAÇÃO NÃO IDENTIFICADO NA NFS-e", AlinhamentoHorizontal.Centro).ComLarguras(0);
                return;
            }

            AdicionarLinhaComTitulo("Destinatário da Operação")
                .ComCampoDanfse("CNPJ / CPF / NIF", destinatario.CnpjCpfNif)
                .ComCampoDanfse("Telefone", destinatario.Telefone)
                .ComLargurasIguais();

            AdicionarLinhaCampos()
                .ComCampoDanfse("Nome / Nome Empresarial", destinatario.NomeRazaoSocial)
                .ComCampoDanfse("Município / Sigla UF", destinatario.MunicipioUf)
                .ComCampoDanfse("Código IBGE / CEP", destinatario.CodigoIbgeCep)
                .ComLarguras(50, 25, 25);

            AdicionarLinhaCampos()
                .ComCampoDanfse("Endereço", destinatario.Endereco)
                .ComCampoDanfse("E-mail", destinatario.Email)
                .ComLarguras(50, 50);
        }
    }
}
