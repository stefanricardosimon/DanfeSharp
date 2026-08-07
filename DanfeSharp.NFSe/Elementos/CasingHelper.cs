using System.Text.RegularExpressions;

namespace DanfeSharp.NFSe.Elementos
{
    /// <summary>
    /// Coloca um texto em caixa alta preservando o "e" minúsculo de "NFS-e", conforme o próprio
    /// modelo do Anexo I (ex.: "NÚMERO DA NFS-e", "VALOR TOTAL DA NFS-e").
    /// </summary>
    internal static class CasingHelper
    {
        public static string CaixaAltaComNFSe(string texto)
        {
            return Regex.Replace(texto.ToUpper(), "NFS-E", "NFS-e", RegexOptions.IgnoreCase);
        }
    }
}
