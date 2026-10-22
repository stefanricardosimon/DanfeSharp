using DanfeSharp.Graphics;
using org.pdfclown.documents.contents.colorSpaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DanfeSharp
{
    /// <summary>
    /// Conteúdo de uma célula da <see cref="Tabela"/>.
    /// </summary>
    internal abstract class CelulaTabela
    {
        /// <summary>
        /// Altura necessária, em mm, para desenhar o conteúdo na largura informada.
        /// </summary>
        public abstract float CalcularAltura(float largura, Fonte fonte);

        /// <summary>
        /// Desenha o conteúdo na área informada (mm).
        /// </summary>
        public abstract void Desenhar(Gfx gfx, RectangleF area, Fonte fonte);

        /// <summary>
        /// Executa o desenho com a cor cinza usada nos rótulos.
        /// </summary>
        protected static void ComCorRotulo(Gfx gfx, Action desenho)
        {
            gfx.PrimitiveComposer.BeginLocalState();
            gfx.PrimitiveComposer.SetFillColor(new DeviceRGBColor(0.4, 0.4, 0.4));
            desenho();
            gfx.PrimitiveComposer.End();
        }
    }

    /// <summary>
    /// Célula de texto, com quebra de linha automática e um rodapé opcional alinhado na base da célula.
    /// </summary>
    internal class CelulaTexto : CelulaTabela
    {
        public string Texto { get; private set; }
        public AlinhamentoHorizontal Alinhamento { get; private set; }

        /// <summary>
        /// Texto desenhado em cinza na base da célula.
        /// </summary>
        public string Rodape { get; set; }

        public AlinhamentoHorizontal AlinhamentoRodape { get; set; }

        public CelulaTexto(string texto, AlinhamentoHorizontal alinhamento = AlinhamentoHorizontal.Esquerda)
        {
            Texto = texto;
            Alinhamento = alinhamento;
            AlinhamentoRodape = AlinhamentoHorizontal.Esquerda;
        }

        private static TextBlock CriarBloco(string texto, float largura, Fonte fonte, AlinhamentoHorizontal alinhamento)
        {
            return new TextBlock(texto, fonte) { Width = largura, AlinhamentoHorizontal = alinhamento };
        }

        public override float CalcularAltura(float largura, Fonte fonte)
        {
            float altura = 0;

            if (!string.IsNullOrWhiteSpace(Texto))
                altura += CriarBloco(Texto, largura, fonte, Alinhamento).Height;

            if (!string.IsNullOrWhiteSpace(Rodape))
                altura += CriarBloco(Rodape, largura, fonte, AlinhamentoRodape).Height;

            return altura;
        }

        public override void Desenhar(Gfx gfx, RectangleF area, Fonte fonte)
        {
            if (!string.IsNullOrWhiteSpace(Texto))
            {
                var bloco = CriarBloco(Texto, area.Width, fonte, Alinhamento);
                bloco.SetPosition(area.X, area.Y);
                bloco.Draw(gfx);
            }

            if (!string.IsNullOrWhiteSpace(Rodape))
            {
                var bloco = CriarBloco(Rodape, area.Width, fonte, AlinhamentoRodape);
                bloco.SetPosition(area.X, area.Bottom - bloco.Height);
                ComCorRotulo(gfx, () => bloco.Draw(gfx));
            }
        }
    }

    /// <summary>
    /// Célula com uma lista de pares rótulo/valor, no formato "ICMS.........1.234,56". Cada linha possui um
    /// ou mais pares, que dividem igualmente a largura da célula. O rótulo e os pontos são desenhados em cinza
    /// e o valor é alinhado à direita do seu espaço.
    /// </summary>
    internal class CelulaPares : CelulaTabela
    {
        private const float EspacoEntrePares = 1.2F;
        private const float FonteTamanhoMinimo = 5F;

        private readonly List<string[]> _Linhas = new List<string[]>();

        /// <summary>
        /// Adiciona uma linha com um par rótulo/valor. A linha é ignorada quando o valor é nulo.
        /// </summary>
        public CelulaPares AdicionarLinha(string rotulo, string valor)
        {
            return AdicionarLinha(rotulo, valor, null, null);
        }

        /// <summary>
        /// Adiciona uma linha com dois pares rótulo/valor. Um par sem valor (nulo) deixa seu espaço vazio, e a linha
        /// é ignorada quando nenhum dos pares possui valor.
        /// </summary>
        public CelulaPares AdicionarLinha(string rotulo1, string valor1, string rotulo2, string valor2)
        {
            bool tem1 = valor1 != null, tem2 = valor2 != null;

            if (!tem1 && !tem2) return this;

            // Uma linha de um único par ocupa toda a largura da célula.
            if (rotulo2 == null && valor2 == null)
                _Linhas.Add(new[] { rotulo1, valor1 });
            else
                _Linhas.Add(new[] { tem1 ? rotulo1 : null, valor1, tem2 ? rotulo2 : null, valor2 });

            return this;
        }

        public override float CalcularAltura(float largura, Fonte fonte) => _Linhas.Count * fonte.AlturaLinha;

        public override void Desenhar(Gfx gfx, RectangleF area, Fonte fonte)
        {
            float y = area.Y;

            foreach (var linha in _Linhas)
            {
                int pares = linha.Length / 2;
                float larguraPar = (area.Width - EspacoEntrePares * (pares - 1)) / pares;

                for (int i = 0; i < pares; i++)
                {
                    var valor = linha[i * 2 + 1];
                    if (valor == null) continue;

                    DesenharPar(gfx, fonte, area.X + i * (larguraPar + EspacoEntrePares), y, larguraPar, linha[i * 2], valor);
                }

                y += fonte.AlturaLinha;
            }
        }

        private static void DesenharPar(Gfx gfx, Fonte fonte, float x, float y, float largura, string rotulo, string valor)
        {
            float larguraRotulo = fonte.MedirLarguraTexto(rotulo);
            float larguraValor = fonte.MedirLarguraTexto(valor);
            float larguraPonto = fonte.MedirLarguraChar('.');
            float necessario = larguraRotulo + larguraValor + larguraPonto;

            // Reduz a fonte, até um limite, quando o par não cabe no espaço disponível.
            if (necessario > largura)
            {
                fonte = new Fonte(fonte.FonteInterna, Math.Max(FonteTamanhoMinimo, fonte.Tamanho * largura / necessario));
                larguraRotulo = fonte.MedirLarguraTexto(rotulo);
                larguraValor = fonte.MedirLarguraTexto(valor);
                larguraPonto = fonte.MedirLarguraChar('.');
            }

            int pontos = Math.Max(0, (int)Math.Floor((largura - larguraRotulo - larguraValor) / larguraPonto));
            var retangulo = new RectangleF(x, y, largura, fonte.AlturaLinha);

            ComCorRotulo(gfx, () => gfx.DrawString(rotulo + new string('.', pontos), retangulo, fonte, AlinhamentoHorizontal.Esquerda));
            gfx.DrawString(valor, retangulo, fonte, AlinhamentoHorizontal.Direita);
        }
    }
}
