using DanfeSharp;
using DanfeSharp.Graphics;

namespace DanfeSharp.NFSe.Elementos
{
    /// <summary>
    /// Campo do DANFSe. Ao contrário de <see cref="Campo"/> (usado pelo DANFE):
    /// não desenha contorno por padrão — o modelo real do DANFSe não usa grade interna, apenas linhas
    /// separadoras entre blocos (item 2.2.4/Anexo I) — e não força o título a caixa alta — os itens
    /// 2.4.2/2.4.1 exigem que a maioria dos títulos de campo seja impressa com a primeira letra de cada
    /// palavra maiúscula, e o conteúdo dos campos seja sempre impresso em formato normal (2.4.3/2.4.4).
    /// </summary>
    internal class CampoDanfse : Campo
    {
        /// <summary>
        /// Quando verdadeiro, o título é impresso em caixa alta (usado apenas nos campos do item 2.1.2 -
        /// Dados de Identificação da NFS-e, conforme a exceção do item 2.4.2). A sigla "NFS-e" mantém o
        /// "e" minúsculo mesmo em caixa alta, conforme o próprio modelo do Anexo I.
        /// </summary>
        public bool CaixaAlta { get; set; }

        /// <summary>
        /// Quando verdadeiro, desenha um contorno ao redor do campo (usado apenas no bloco de canhoto).
        /// </summary>
        public bool ComBorda { get; set; }

        /// <summary>
        /// Quando verdadeiro, desenha um sombreamento em cinza claro atrás do campo (item 2.2.3 do manual,
        /// usado nos campos "Emitente da NFS-e" e "Valor Líquido da NFS-e + IBS/CBS").
        /// </summary>
        public bool ComSombreamento { get; set; }

        public CampoDanfse(string cabecalho, string conteudo, Estilo estilo, AlinhamentoHorizontal alinhamentoHorizontalConteudo = AlinhamentoHorizontal.Esquerda,
            bool caixaAlta = false, bool comBorda = false, bool comSombreamento = false)
            : base(cabecalho, conteudo, estilo, alinhamentoHorizontalConteudo)
        {
            CaixaAlta = caixaAlta;
            ComBorda = comBorda;
            ComSombreamento = comSombreamento;
            IsConteudoNegrito = false;
        }

        public override bool PossuiContono => ComBorda;

        public override void Draw(Gfx gfx)
        {
            if (ComSombreamento) Sombreamento.Desenhar(gfx, BoundingBox);
            base.Draw(gfx);
        }

        protected override void DesenharCabecalho(Gfx gfx)
        {
            if (string.IsNullOrWhiteSpace(Cabecalho)) return;

            var texto = CaixaAlta ? CasingHelper.CaixaAltaComNFSe(Cabecalho) : Cabecalho;

            // Item 2.4.2 do manual: título do campo em negrito. Constrói a fonte em negrito diretamente
            // (Estilo.CriarFonteNegrito já é pública) em vez de depender de alterações no Estilo do DANFE.
            var fonteNegrito = Estilo.CriarFonteNegrito(Estilo.FonteCampoCabecalho.Tamanho);

            gfx.DrawString(texto, RetanguloDesenhvael, fonteNegrito, AlinhamentoHorizontal.Esquerda, AlinhamentoVertical.Topo);
        }
    }
}
