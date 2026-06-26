using System;
using System.Collections.Generic;

namespace DanfeSharp
{
    /// <summary>
    /// Desenha o Código de Barras Code 128, alternando entre os subconjuntos A e C
    /// para representar de forma compacta tanto letras quanto sequências de dígitos
    /// (usado para a Chave de Acesso quando o CNPJ alfanumérico é utilizado).
    /// </summary>
    internal class Barcode128A : Barcode128Base
    {
        private const byte CodeA = 101;
        private const byte CodeC = 99;
        private const byte StartA = 103;
        private const byte StartC = 105;
        private const byte Stop = 106;

        /// <summary>
        /// Tamanho mínimo de uma sequência de dígitos consecutivos para que seja vantajoso
        /// alternar para o subconjunto C, que representa pares de dígitos com um único símbolo.
        /// </summary>
        private const int TamanhoMinimoSequenciaDigitos = 4;

        public Barcode128A(string code, Estilo estilo, float largura = Barcode128Base.LarguraAutomatica) : base(estilo, largura)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("O código não pode ser vazio.", "code");
            }

            foreach (char c in code)
            {
                if (!PodeCodificar(c))
                {
                    throw new ArgumentException("O código contém um caracter que não pode ser representado pelo Code 128A/C.", "code");
                }
            }

            Code = code;
        }

        private static bool PodeCodificar(char c) => c <= 95;

        private static byte ValorSimboloA(char c) => c < 32 ? (byte)(c + 64) : (byte)(c - 32);

        private static int TamanhoSequenciaDigitos(string s, int inicio)
        {
            int i = inicio;
            while (i < s.Length && char.IsDigit(s[i])) i++;
            return i - inicio;
        }

        protected override List<byte> MontarCodeBytes()
        {
            List<byte> codeBytes = new List<byte>();

            bool emC = TamanhoSequenciaDigitos(Code, 0) >= TamanhoMinimoSequenciaDigitos;
            codeBytes.Add(emC ? StartC : StartA);

            int i = 0;

            while (i < Code.Length)
            {
                if (emC)
                {
                    int sequencia = TamanhoSequenciaDigitos(Code, i);
                    int tamanhoPares = sequencia - (sequencia % 2);

                    for (int k = 0; k < tamanhoPares; k += 2)
                    {
                        codeBytes.Add(byte.Parse(Code.Substring(i + k, 2)));
                    }

                    i += tamanhoPares;

                    if (i < Code.Length && TamanhoSequenciaDigitos(Code, i) < TamanhoMinimoSequenciaDigitos)
                    {
                        codeBytes.Add(CodeA);
                        emC = false;
                    }
                }
                else
                {
                    int sequencia = TamanhoSequenciaDigitos(Code, i);

                    if (sequencia >= TamanhoMinimoSequenciaDigitos)
                    {
                        codeBytes.Add(CodeC);
                        emC = true;
                    }
                    else
                    {
                        codeBytes.Add(ValorSimboloA(Code[i]));
                        i++;
                    }
                }
            }

            // Calcular dígito verificador
            int cd = codeBytes[0];

            for (int j = 1; j < codeBytes.Count; j++)
            {
                cd += j * codeBytes[j];
                cd %= 103;
            }

            codeBytes.Add((byte)cd);
            codeBytes.Add(Stop);

            return codeBytes;
        }
    }
}
