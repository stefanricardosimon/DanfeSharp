using System;
using DanfeSharp;
using DanfeSharp.Elementos;
using DanfeSharp.Graphics;
using DanfeSharp.NFSe.Elementos;
using DanfeSharp.NFSe.Modelo;

namespace DanfeSharp.NFSe.Blocos
{
    /// <summary>
    /// Bloco básico do DANFSe. Réplica de <see cref="DanfeSharp.Blocos.BlocoBase"/> parametrizada para o
    /// <see cref="DanfseViewModel"/> (a classe original é acoplada ao ViewModel do DANFE), mas sem grade
    /// interna: o modelo real do DANFSe separa os blocos apenas com uma linha horizontal (item 2.2.3 e
    /// Anexo I do manual), e o título do bloco ocupa a primeira coluna da primeira linha de campos.
    /// </summary>
    internal abstract class DanfseBlocoBase : ElementoBase
    {
        public DanfseViewModel ViewModel { get; }

        public VerticalStack MainVerticalStack { get; }

        /// <summary>
        /// Quando verdadeiro (padrão), desenha uma linha separadora acima do bloco. Deve ser falso quando
        /// o bloco é visualmente contínuo com o anterior (ex.: Intermediário após Destinatário) ou quando
        /// o bloco anterior já termina com uma linha própria (ex.: logo após o cabeçalho).
        /// </summary>
        protected virtual bool LinhaSuperior => true;

        protected DanfseBlocoBase(DanfseViewModel viewModel, Estilo estilo) : base(estilo)
        {
            ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            MainVerticalStack = new VerticalStack();

            if (LinhaSuperior)
            {
                MainVerticalStack.Add(new LinhaSeparadora());
            }
        }

        public LinhaCampos AdicionarLinhaCampos(float altura = Constantes.CampoAltura)
        {
            var l = new LinhaCampos(Estilo, Width, altura);
            MainVerticalStack.Add(l);
            return l;
        }

        /// <summary>
        /// Adiciona a primeira linha de campos do bloco, com o título do bloco ocupando a primeira coluna
        /// (sombreada), conforme o modelo real do Anexo I.
        /// </summary>
        public LinhaCampos AdicionarLinhaComTitulo(string titulo, float altura = Constantes.CampoAltura)
        {
            var l = AdicionarLinhaCampos(altura);
            l.Elementos.Add(new TituloBlocoCampo(titulo, Estilo));
            return l;
        }

        public void Adicionar(DrawableBase elemento)
        {
            MainVerticalStack.Add(elemento);
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
