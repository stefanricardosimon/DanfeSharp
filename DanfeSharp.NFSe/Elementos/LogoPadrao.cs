using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace DanfeSharp.NFSe.Elementos
{
    /// <summary>
    /// Logomarca padrão da NFS-e, exibida no canto esquerdo do cabeçalho quando nenhuma logomarca
    /// própria é informada (item 2.4.3 do manual: "no canto esquerdo, a logomarca da NFS-e, disponível
    /// em: https://www.gov.br/nfse/pt-br/biblioteca/documentacao-tecnica/logos-da-nfs-e/...").
    /// </summary>
    internal static class LogoPadrao
    {
        private const string NomeRecurso = "DanfeSharp.NFSe.Resources.LogoNFSe.png";

        /// <summary>
        /// Carrega a logomarca padrão já convertida para JPEG (formato exigido pelo PDFClown para
        /// carregamento de imagens), composta sobre um fundo da mesma cor do sombreamento do cabeçalho,
        /// já que o arquivo original é um PNG com fundo transparente.
        /// </summary>
        public static Stream CarregarComoJpeg()
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var streamPng = assembly.GetManifestResourceStream(NomeRecurso))
            {
                if (streamPng == null) return null;

                using (var logo = Image.FromStream(streamPng))
                using (var composta = new Bitmap(logo.Width, logo.Height))
                {
                    using (var g = System.Drawing.Graphics.FromImage(composta))
                    {
                        g.Clear(Sombreamento.CorGdi);
                        g.DrawImage(logo, 0, 0, logo.Width, logo.Height);
                    }

                    var streamJpeg = new MemoryStream();
                    composta.Save(streamJpeg, ImageFormat.Jpeg);
                    streamJpeg.Position = 0;
                    return streamJpeg;
                }
            }
        }
    }
}
