using DanfeSharp.NFSe.Elementos;
using DanfeSharp.NFSe.Interno;
using DanfeSharp.NFSe.Modelo;

namespace DanfeSharp.NFSe.Blocos
{
    /// <summary>
    /// Bloco de Dados de Identificação da NFS-e, item 2.1.1 e 2.1.2 do manual, incluindo o QR Code
    /// de consulta pública (item 2.4.3).
    /// </summary>
    internal class BlocoIdentificacaoNFSe : DanfseBlocoBase
    {
        public const float LarguraColunaQRCode = 22F;

        /// <summary>
        /// Altura da linha "Emitente da NFS-e" / "Situação da NFS-e" / "Finalidade", um pouco maior que
        /// <see cref="Constantes.CampoAltura"/> para, somada às outras duas linhas (que usam a altura
        /// padrão), abrir espaço suficiente para o QR Code respeitar o tamanho mínimo de 1,52 cm exigido
        /// pelo item 2.4.3 do manual, sem reduzir o espaço do texto de autenticidade abaixo dele. Só esta
        /// linha usa altura maior — nas duas primeiras (item 2.1.1) isso deixava um vão vazio entre o
        /// título e o conteúdo de cada campo, já que o conteúdo de uma única linha é sempre ancorado à
        /// base do campo.
        /// </summary>
        private const float AlturaLinha3 = 9F;

        /// <summary>
        /// Tamanho do QR Code, em milímetros, um pouco acima do mínimo de 1,52 cm exigido pelo item 2.4.3
        /// do manual (mesmo valor observado em DANFSe emitidos pelo gerador nacional).
        /// </summary>
        private const float AlturaQRCode = 15.9F;

        public BlocoIdentificacaoNFSe(DanfseViewModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {
            AdicionarLinhaCampos()
                .ComCampoDanfse("Chave de Acesso da NFS-e", viewModel.ChaveAcesso, caixaAlta: true)
                .ComLarguras(0);

            var linhasCampos = new VerticalStack();

            // As duas primeiras colunas de cada uma das 3 linhas abaixo são alinhadas com as colunas de
            // título e "CNPJ / CPF / NIF" dos blocos abaixo (25% da largura do bloco cada, conforme o
            // modelo real do Anexo I), embora essas linhas estejam contidas em "linhasCampos", que ocupa
            // apenas (100 - LarguraColunaQRCode)% da largura do bloco por causa da coluna do QR Code ao lado.
            float larguraColunaAlinhada = 25F / (1F - LarguraColunaQRCode / 100F);

            var l1 = new LinhaCampos(Estilo, 0)
                .ComCampoDanfse("Número da NFS-e", viewModel.NumeroNFSe, caixaAlta: true)
                .ComCampoDanfse("Competência da NFS-e", viewModel.Competencia, caixaAlta: true)
                .ComCampoDanfse("Data e Hora da Emissão da NFS-e", viewModel.DataHoraEmissaoNFSe, caixaAlta: true)
                .ComLarguras(larguraColunaAlinhada, larguraColunaAlinhada, 0);

            var l2 = new LinhaCampos(Estilo, 0)
                .ComCampoDanfse("Número da DPS", viewModel.NumeroDPS, caixaAlta: true)
                .ComCampoDanfse("Série da DPS", viewModel.SerieDPS, caixaAlta: true)
                .ComCampoDanfse("Data e Hora da Emissão da DPS", viewModel.DataHoraEmissaoDPS, caixaAlta: true)
                .ComLarguras(larguraColunaAlinhada, larguraColunaAlinhada, 0);

            var l3 = new LinhaCampos(Estilo, 0, AlturaLinha3)
                .ComCampoDanfse("Emitente da NFS-e", viewModel.EmitenteNFSe, AlinhamentoHorizontal.Esquerda, caixaAlta: true, comSombreamento: true)
                .ComCampoDanfse("Situação da NFS-e", viewModel.SituacaoNFSe, caixaAlta: true)
                .ComCampoDanfse("Finalidade", viewModel.Finalidade, caixaAlta: true)
                .ComLarguras(larguraColunaAlinhada, larguraColunaAlinhada, 0);

            linhasCampos.Add(l1, l2, l3);

            var qrCode = new QRCodeElemento(viewModel.UrlConsultaPublica, estilo) { Height = AlturaQRCode };

            var textoConsulta = new TextoSimplesDanfse(estilo, "A autenticidade desta NFS-e pode ser verificada pela leitura deste código QR ou pela consulta da chave de acesso no portal nacional da NFS-e")
            {
                Height = linhasCampos.Height - qrCode.Height,
                AlinhamentoHorizontal = AlinhamentoHorizontal.Centro,
                TamanhoFonte = 5.5F
            };

            var colunaQrCode = new VerticalStack();
            colunaQrCode.Add(qrCode, textoConsulta);

            var flexLine = new FlexibleLine { Height = linhasCampos.Height }
                .ComElemento(linhasCampos)
                .ComElemento(colunaQrCode)
                .ComLarguras(0, LarguraColunaQRCode);

            Adicionar(flexLine);
        }

        protected override bool LinhaSuperior => false;
    }
}
