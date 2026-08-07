using DanfeSharp;

namespace DanfeSharp.NFSe.Elementos
{
    /// <summary>
    /// <see cref="TextoSimples"/> sem contorno (o modelo real do DANFSe não usa grade interna).
    /// </summary>
    internal class TextoSimplesDanfse : TextoSimples
    {
        public TextoSimplesDanfse(Estilo estilo, string texto) : base(estilo, texto)
        {
        }

        public override bool PossuiContono => false;
    }
}
