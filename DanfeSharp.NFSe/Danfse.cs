using System;
using System.Collections.Generic;
using System.Drawing;
using DanfeSharp.NFSe.Blocos;
using DanfeSharp.NFSe.Interno;
using DanfeSharp.NFSe.Modelo;
using org.pdfclown.documents;
using org.pdfclown.documents.contents.composition;
using org.pdfclown.documents.contents.fonts;
using org.pdfclown.files;

namespace DanfeSharp.NFSe
{
    /// <summary>
    /// Gerador do DANFSe (Documento Auxiliar da NFS-e), conforme a Nota Técnica nº 008 (SE/CGNFS-e).
    /// O DANFSe é impresso obrigatoriamente em uma única página A4, em modo retrato (item 2.2 do manual).
    /// </summary>
    public class Danfse : IDisposable
    {
        /// <summary>
        /// Margem entre o corpo impresso do DANFSe e o final do formulário, em milímetros (item 2.2.2 do manual).
        /// </summary>
        public const float Margem = 2F;

        public DanfseViewModel ViewModel { get; }
        public File File { get; }
        internal Document PdfDocument { get; }

        /// <summary>
        /// Espessura, em pontos, da borda da página (item 2.2.3 do manual).
        /// </summary>
        public const float EspessuraBordaPagina = 1F;

        private readonly List<DanfseBlocoBase> _blocos;
        private readonly Estilo _estiloPadrao;
        private readonly Estilo _estiloIdentificacao;
        private readonly StandardType1Font _fonteRegular;
        private readonly StandardType1Font _fonteNegrito;
        private readonly StandardType1Font _fonteItalico;

        private BlocoCabecalho _blocoCabecalho;
        private bool _foiGerado;

        public Danfse(DanfseViewModel viewModel)
        {
            ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            _blocos = new List<DanfseBlocoBase>();
            File = new File();
            PdfDocument = File.Document;

            // De acordo com o item 2.4, as fontes devem ser Arial (títulos/labels) e Microsoft Sans Serif
            // (conteúdos). Helvetica é a fonte padrão do PDF metricamente equivalente à Arial, usada aqui
            // para ambos os casos (não há fonte padrão do PDF equivalente à Microsoft Sans Serif).
            var fonteFamilia = StandardType1Font.FamilyEnum.Helvetica;
            _fonteRegular = new StandardType1Font(PdfDocument, fonteFamilia, false, false);
            _fonteNegrito = new StandardType1Font(PdfDocument, fonteFamilia, true, false);
            _fonteItalico = new StandardType1Font(PdfDocument, fonteFamilia, false, true);

            // Item 2.4.2: títulos dos campos têm tamanho de seis (6) pontos. Itens 2.4.3/2.4.4: conteúdo
            // dos campos sempre em sete (7) pontos, formato normal. O negrito dos títulos e a espessura
            // das linhas divisórias (item 2.2.3) são decididos pelos próprios elementos do DANFSe
            // (CampoDanfse/TituloBlocoCampo), sem depender de alterações no Estilo/ElementoBase do DANFE.
            _estiloPadrao = new Estilo(_fonteRegular, _fonteNegrito, _fonteItalico, 6, 7);

            // Item 2.4.2 (exceção): títulos dos campos do item 2.1.2 (Dados de Identificação da NFS-e)
            // têm tamanho de 7 pontos e são impressos em caixa alta (ver BlocoIdentificacaoNFSe).
            _estiloIdentificacao = new Estilo(_fonteRegular, _fonteNegrito, _fonteItalico, 7, 7);

            MontarBlocos();
            AdicionarMetadata();
            CarregarLogoPadrao();

            _foiGerado = false;
        }

        /// <summary>
        /// Carrega, no canto esquerdo do cabeçalho, a logomarca padrão da NFS-e (item 2.4.3 do manual).
        /// Pode ser substituída por <see cref="AdicionarLogoImagem(System.IO.Stream)"/>.
        /// </summary>
        private void CarregarLogoPadrao()
        {
            using (var stream = DanfeSharp.NFSe.Elementos.LogoPadrao.CarregarComoJpeg())
            {
                if (stream != null) AdicionarLogoImagem(stream);
            }
        }

        private void MontarBlocos()
        {
            _blocoCabecalho = AdicionarBloco(new BlocoCabecalho(ViewModel, _estiloPadrao));
            AdicionarBloco(new BlocoIdentificacaoNFSe(ViewModel, _estiloIdentificacao));
            AdicionarBloco(new BlocoPessoa("Prestador / Fornecedor", ViewModel.Prestador,
                "PRESTADOR/FORNECEDOR DA OPERAÇÃO NÃO IDENTIFICADO NA NFS-e", true, ViewModel, _estiloPadrao));
            AdicionarBloco(new BlocoPessoa("Tomador / Adquirente", ViewModel.Tomador,
                "TOMADOR/ADQUIRENTE DA OPERAÇÃO NÃO IDENTIFICADO NA NFS-e", false, ViewModel, _estiloPadrao));
            AdicionarBloco(new BlocoDestinatario(ViewModel, _estiloPadrao));
            AdicionarBloco(new BlocoPessoa("Intermediário da Operação", ViewModel.Intermediario,
                "INTERMEDIÁRIO DA OPERAÇÃO NÃO IDENTIFICADO NA NFS-e", false, ViewModel, _estiloPadrao, linhaSuperior: false));
            AdicionarBloco(new BlocoServico(ViewModel, _estiloPadrao));
            AdicionarBloco(new BlocoTributacaoMunicipal(ViewModel, _estiloPadrao));
            AdicionarBloco(new BlocoTributacaoFederal(ViewModel, _estiloPadrao));
            AdicionarBloco(new BlocoTributacaoIBSCBS(ViewModel, _estiloPadrao));
            AdicionarBloco(new BlocoValorTotal(ViewModel, _estiloPadrao));
            AdicionarBloco(new BlocoInformacoesComplementares(ViewModel, _estiloPadrao));

            if (ViewModel.ExibirCanhoto)
                AdicionarBloco(new BlocoCanhoto(ViewModel, _estiloPadrao));
        }

        private T AdicionarBloco<T>(T bloco) where T : DanfseBlocoBase
        {
            _blocos.Add(bloco);
            return bloco;
        }

        /// <summary>
        /// Substitui a logomarca padrão exibida no canto esquerdo do cabeçalho por uma imagem própria
        /// (JPEG não progressivo). O item 2.4.3 do manual prevê a logomarca da NFS-e nesse local; use
        /// este método apenas se o seu cenário justificar uma logomarca diferente da padrão.
        /// </summary>
        public void AdicionarLogoImagem(System.IO.Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var img = org.pdfclown.documents.contents.entities.Image.Get(stream);
            if (img == null) throw new InvalidOperationException("O logotipo não pode ser carregado, certifique-se que a imagem esteja no formato JPEG não progressivo.");
            _blocoCabecalho.Logo = img.ToXObject(PdfDocument);
        }

        public void AdicionarLogoImagem(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException(nameof(path));

            using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                AdicionarLogoImagem(fs);
            }
        }

        private void AdicionarMetadata()
        {
            var info = PdfDocument.Information;
            info[new org.pdfclown.objects.PdfName("ChaveAcesso")] = ViewModel.ChaveAcesso;
            info[new org.pdfclown.objects.PdfName("TipoDocumento")] = "DANFSe";
            info.CreationDate = DateTime.Now;
            info.Creator = string.Format("{0} {1} - {2}", "DanfeSharp.NFSe", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, "https://github.com/SilverCard/DanfeSharp");
            info.Title = "DANFSe (Documento Auxiliar da NFS-e)";
        }

        public void Gerar()
        {
            if (_foiGerado) throw new InvalidOperationException("O DANFSe já foi gerado.");

            var page = new Page(PdfDocument);
            PdfDocument.Pages.Add(page);

            var retangulo = new RectangleF(0, 0, Constantes.A4Largura, Constantes.A4Altura);
            var retanguloDesenhavel = retangulo.InflatedRetangle(Margem);

            page.Size = new SizeF(retangulo.Width.ToPoint(), retangulo.Height.ToPoint());

            var primitiveComposer = new PrimitiveComposer(page);
            var gfx = new Gfx(primitiveComposer);

            var corpo = new VerticalStack { Width = retanguloDesenhavel.Width };
            foreach (var bloco in _blocos)
            {
                corpo.Add(bloco);
            }

            corpo.SetPosition(retanguloDesenhavel.Location);
            corpo.Draw(gfx);

            // Item 2.2.3 do manual: a página deve ter borda de 1 (um) ponto de espessura, mais grossa
            // que as linhas divisórias dos blocos de conteúdo (0,5 ponto).
            gfx.StrokeRectangle(retanguloDesenhavel, EspessuraBordaPagina);

            DesenharCreditos(gfx, retanguloDesenhavel);
            DesenharMarcaDagua(gfx, retanguloDesenhavel);

            gfx.Stroke();
            gfx.Flush();

            _foiGerado = true;
        }

        /// <summary>
        /// Desenha a data e hora de geração no canto inferior direito, como no DANFE. Como a margem do
        /// DANFSe é muito estreita (item 2.2.2 do manual, 0,15 a 0,20 cm) para conter texto legível fora
        /// da borda da página, a informação é desenhada por dentro da borda, sem sobrepor nenhum campo.
        /// </summary>
        private void DesenharCreditos(Gfx gfx, RectangleF retanguloDesenhavel)
        {
            var fonte = _estiloPadrao.CriarFonteItalico(6);
            var texto = "Gerado em " + DateTime.Now.ToString("dd/MM/yyyy") + ", " + DateTime.Now.ToString("HH:mm:ss");

            var r = new RectangleF(
                retanguloDesenhavel.X,
                retanguloDesenhavel.Bottom - fonte.AlturaLinha - 1F,
                retanguloDesenhavel.Width - 1F,
                fonte.AlturaLinha);

            gfx.DrawString(texto, r, fonte, AlinhamentoHorizontal.Direita);
        }

        private void DesenharMarcaDagua(Gfx gfx, RectangleF retanguloCorpo)
        {
            string texto = ViewModel.Cancelada ? "CANCELADA" : ViewModel.Substituida ? "SUBSTITUÍDA" : null;
            if (texto == null) return;

            var ts = new TextStack(retanguloCorpo)
            {
                AlinhamentoVertical = AlinhamentoVertical.Centro,
                AlinhamentoHorizontal = AlinhamentoHorizontal.Centro
            }.AddLine(texto, _estiloPadrao.CriarFonteRegular(70));

            gfx.PrimitiveComposer.BeginLocalState();
            gfx.PrimitiveComposer.SetFillColor(new org.pdfclown.documents.contents.colorSpaces.DeviceRGBColor(0.65, 0.65, 0.65));
            ts.Draw(gfx);
            gfx.PrimitiveComposer.End();
        }

        public void Salvar(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException(nameof(path));

            File.Save(path, SerializationModeEnum.Incremental);
        }

        public void Salvar(System.IO.Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            File.Save(new org.pdfclown.bytes.Stream(stream), SerializationModeEnum.Incremental);
        }

        #region IDisposable Support

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    File.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
