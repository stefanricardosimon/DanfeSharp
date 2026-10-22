using System;
using System.Linq;
using DanfeSharp.Graphics;
using DanfeSharp.Elementos;
using DanfeSharp.Modelo;

namespace DanfeSharp.Blocos
{
    /// <summary>
    /// Define um bloco básico do DANFE.
    /// </summary>
    internal abstract class BlocoBase : ElementoBase
    {
        /// <summary>
        /// Constante de proporção dos campos para o formato retrato A4, porcentagem dividida pela largura desenhável.
        /// </summary>
        public const float Proporcao = 100F / 200F;

        public DanfeViewModel ViewModel { get; private set; }

        public abstract PosicaoBloco Posicao { get; }

        /// <summary>
        /// Pilha principal.
        /// </summary>
        public VerticalStack MainVerticalStack { get; private set; }

        /// <summary>
        /// Quando verdadeiro, o bloco é mostrado apenas na primeira página, caso contário é mostrado em todas elas.
        /// </summary>
        public virtual bool VisivelSomentePrimeiraPagina => true;

        public virtual string Cabecalho => null;

        public BlocoBase(DanfeViewModel viewModel, Estilo estilo) : base(estilo)
        {
            MainVerticalStack = new VerticalStack();
            ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            if (!string.IsNullOrWhiteSpace(Cabecalho))
            {
                MainVerticalStack.Add(new CabecalhoBloco(estilo, Cabecalho));
            }
        }

        public LinhaCampos AdicionarLinhaCampos()
        {
            var l = new LinhaCampos(Estilo, Width);
            l.Width = Width;
            l.Height = Constantes.CampoAltura;
            MainVerticalStack.Add(l);
            return l;
        }

        /// <summary>
        /// Adiciona uma linha de campos numéricos facultativos: somente os campos com valor são impressos e a linha
        /// só é adicionada quando existe ao menos um deles. A NT 2026.010 não autoriza imprimir informação inexistente no XML.
        /// </summary>
        public void AdicionarLinhaCamposFacultativos(params (string Cabecalho, double? Valor, int CasasDecimais)[] campos)
        {
            var existentes = campos.Where(c => c.Valor.HasValue).ToList();
            if (existentes.Count == 0) return;

            var l = AdicionarLinhaCampos();

            foreach (var c in existentes)
                l.ComCampoNumerico(c.Cabecalho, c.Valor, c.CasasDecimais);

            l.ComLargurasIguais();
        }

        public override void Draw(Gfx gfx)
        {
            base.Draw(gfx);
            MainVerticalStack.SetPosition(X, Y);
            MainVerticalStack.Width = Width;
            MainVerticalStack.Draw(gfx);
        }

        public override float Height { get => MainVerticalStack.Height; set => throw new NotSupportedException(); }
        public override bool PossuiContono => false;            
    }
}
