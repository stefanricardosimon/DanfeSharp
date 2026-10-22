using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DanfeSharp.Graphics;
using DanfeSharp.Modelo;

namespace DanfeSharp.Blocos
{
    /// <summary>
    /// Quadro "Dados dos Produtos / Serviços" no leiaute da NT 2026.010: o código e a descrição dividem a mesma
    /// coluna e os tributos (ICMS, IPI, IBS, CBS e IS) são agrupados nas colunas bases de cálculo, alíquotas e valores.
    /// </summary>
    internal class TabelaProdutosServicos : ElementoBase
    {
        public CabecalhoBloco CabecalhoBloco { get; private set; }
        public Tabela Tabela { get; private set; }
        public DanfeViewModel ViewModel { get; private set; }

        public TabelaProdutosServicos(DanfeViewModel viewModel, Estilo estilo) : base(estilo)
        {
            ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            CabecalhoBloco = new CabecalhoBloco(estilo, "DADOS DOS PRODUTOS / SERVIÇOS");

            var ac = AlinhamentoHorizontal.Centro;
            var ae = AlinhamentoHorizontal.Esquerda;

            Tabela = new Tabela(Estilo);
            string rotuloCst = ViewModel.Emitente.CRT == "3" ? "CST" : "CSOSN";

            // A coluna da descrição ocupa o espaço restante.
            if (ViewModel.IsRetrato)
            {
                Tabela
                .ComColuna(0, ae, "DESCRIÇÃO DO PRODUTO / SERVIÇO")
                .ComColuna(6.5F, ac, rotuloCst + " /", "CFOP")
                .ComColuna(6.5F, ac, "QTD / UN")
                .ComColuna(6.5F, ac, "VLR UNIT")
                .ComColuna(7F, ac, "VLR TOTAL")
                .ComColuna(12F, ac, "BASES DE CÁLCULO")
                .ComColuna(17.5F, ac, "ALÍQUOTAS")
                .ComColuna(18.5F, ac, "VALOR DOS TRIBUTOS");
            }
            else
            {
                Tabela
                .ComColuna(0, ae, "DESCRIÇÃO DO PRODUTO / SERVIÇO")
                .ComColuna(5.5F, ac, rotuloCst + " /", "CFOP")
                .ComColuna(5.5F, ac, "QTD / UN")
                .ComColuna(5.5F, ac, "VLR UNIT")
                .ComColuna(6F, ac, "VLR TOTAL")
                .ComColuna(10.5F, ac, "BASES DE CÁLCULO")
                .ComColuna(14.5F, ac, "ALÍQUOTAS")
                .ComColuna(17.5F, ac, "VALOR DOS TRIBUTOS");
            }

            Tabela.AjustarLarguraColunas();

            foreach (var p in ViewModel.Produtos)
            {
                Tabela.AdicionarLinha(new List<CelulaTabela>
                {
                    CriarCelulaDescricao(p),
                    new CelulaPares()
                        .AdicionarLinha(rotuloCst, string.IsNullOrWhiteSpace(p.OCst) ? null : p.OCst)
                        .AdicionarLinha("CFOP", p.Cfop.ToString()),
                    CriarCelulaQuantidade(p),
                    //Comércio Exterior - OMA - NT 2016.001
                    CriarCelulaValor(p.ValorUnitario, p.ValorUnitarioTrib),
                    CriarCelulaValor(p.ValorTotal, p.ValorTotal),
                    new CelulaPares()
                        .AdicionarLinha("ICMS", Numero(p.BaseIcms))
                        .AdicionarLinha("IBS / CBS", Numero(p.BaseIbsCbs))
                        .AdicionarLinha("IS", Numero(p.BaseIs))
                        .AdicionarLinha("IPI", Numero(p.BaseIpi)),
                    new CelulaPares()
                        .AdicionarLinha("ICMS", Percentual(p.AliquotaIcms), "CBS", Percentual(p.AliquotaCbs))
                        .AdicionarLinha("IBS UF", Percentual(p.AliquotaIbsUf), "IPI", Percentual(p.AliquotaIpi))
                        .AdicionarLinha("IBS MUN", Percentual(p.AliquotaIbsMun), "IS", Percentual(p.AliquotaIs)),
                    new CelulaPares()
                        .AdicionarLinha("ICMS", Numero(p.ValorIcms), "CBS", Numero(p.ValorCbs))
                        .AdicionarLinha("IBS UF", Numero(p.ValorIbsUf), "IPI", Numero(p.ValorIpi))
                        .AdicionarLinha("IBS MUN", Numero(p.ValorIbsMun), "IS", Numero(p.ValorIs))
                });
            }
        }

        private static string Numero(double? valor) => valor.HasValue ? valor.Value.Formatar() : null;

        private static string Percentual(double? valor) => valor.HasValue ? valor.Value.Formatar() + "%" : null;

        /// <summary>
        /// Código e descrição na mesma célula. Na base, referências ao NCM e à classificação tributária do IBS/CBS.
        /// </summary>
        private static CelulaTexto CriarCelulaDescricao(ProdutoViewModel p)
        {
            string texto = string.IsNullOrWhiteSpace(p.Codigo) ? p.Descricao : $"{p.Codigo} - {p.Descricao}";

            if (!string.IsNullOrWhiteSpace(p.InformacoesAdicionais))
                texto += "\r\n" + p.InformacoesAdicionais;

            var referencias = new List<string>();

            if (!string.IsNullOrWhiteSpace(p.Ncm))
                referencias.Add($"[NCM {p.Ncm}]");

            if (!string.IsNullOrWhiteSpace(p.ClassificacaoTributariaIbsCbs))
                referencias.Add($"[cClassTrib {p.ClassificacaoTributariaIbsCbs}]");

            return new CelulaTexto(texto)
            {
                Rodape = string.Join(" ", referencias),
                AlinhamentoRodape = AlinhamentoHorizontal.Direita
            };
        }

        /// <summary>
        /// Quantidade e unidade, em linhas separadas. Quando a unidade tributável difere da comercial (Comércio Exterior - OMA - NT 2016.001),
        /// a quantidade e a unidade tributáveis seguem nas linhas seguintes.
        /// </summary>
        private static CelulaTexto CriarCelulaQuantidade(ProdutoViewModel p)
        {
            var linhas = new List<string> { p.Quantidade.Formatar(), p.Unidade };

            if (!string.Equals(p.Unidade, p.UnidadeTrib) || !Equals(p.Quantidade, p.QuantidadeTrib))
            {
                linhas.Add(p.QuantidadeTrib.Formatar());
                linhas.Add(p.UnidadeTrib);
            }

            return new CelulaTexto(string.Join("\r\n", linhas.Where(l => !string.IsNullOrWhiteSpace(l))), AlinhamentoHorizontal.Centro);
        }

        private static CelulaTexto CriarCelulaValor(double comercial, double tributavel)
        {
            string texto = Equals(comercial, tributavel) ? comercial.Formatar() : comercial.Formatar() + "\r\n" + tributavel.Formatar();
            return new CelulaTexto(texto, AlinhamentoHorizontal.Centro);
        }

        public override void Draw(Gfx gfx)
        {
            base.Draw(gfx);

            Tabela.SetPosition(RetanguloTabela.Location);
            Tabela.SetSize(RetanguloTabela.Size);
            Tabela.Draw(gfx);

            CabecalhoBloco.SetPosition(X, Y);
            CabecalhoBloco.Width = Width;
            CabecalhoBloco.Draw(gfx);
        }


        public RectangleF RetanguloTabela => BoundingBox.CutTop(CabecalhoBloco.Height);
        public bool CompletamenteDesenhada => Tabela.LinhaAtual == ViewModel.Produtos.Count;
        public override bool PossuiContono => false;
    }
}
