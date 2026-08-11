using DanfeSharp.NFSe.Interno;

namespace DanfeSharp.NFSe.Elementos
{
    /// <summary>
    /// Adiciona campos do DANFSe (<see cref="CampoDanfse"/>) a uma <see cref="LinhaCampos"/>, em vez dos
    /// campos padrão do DANFE (que forçam caixa alta, negrito no conteúdo e contorno em todo campo).
    /// </summary>
    internal static class LinhaCamposExtensions
    {
        public static LinhaCampos ComCampoDanfse(this LinhaCampos linha, string cabecalho, string conteudo,
            AlinhamentoHorizontal alinhamentoHorizontalConteudo = AlinhamentoHorizontal.Esquerda,
            bool caixaAlta = false, bool comBorda = false, bool comSombreamento = false)
        {
            linha.Elementos.Add(new CampoDanfse(cabecalho, conteudo, linha.Estilo, alinhamentoHorizontalConteudo, caixaAlta, comBorda, comSombreamento));
            return linha;
        }
    }
}
