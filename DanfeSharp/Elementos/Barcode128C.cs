using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DanfeSharp
{
    /// <summary>
    /// Desenha o Código de Barras Code 128C
    /// </summary>
    internal class Barcode128C : Barcode128Base
    {
        public Barcode128C(string code, Estilo estilo, float largura = 75F) : base(estilo, largura)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("O código não pode ser vazio.", "code");
            }

            if (!Regex.IsMatch(code, @"^\d+$"))
            {
                throw new ArgumentException("O código deve apenas conter digítos numéricos.", "code");
            }

            if (code.Length % 2 != 0)
            {
                Code = "0" + code;
            }
            else
            {
                Code = code;
            }
        }

        protected override List<byte> MontarCodeBytes()
        {
            List<byte> codeBytes = new List<byte>();

            codeBytes.Add(105);

            for (int i = 0; i < Code.Length; i += 2)
            {
                byte b = byte.Parse(Code.Substring(i, 2));
                codeBytes.Add(b);
            }

            // Calcular dígito verificador
            int cd = 105;

            for (int i = 1; i < codeBytes.Count; i++)
            {
                cd += i * codeBytes[i];
                cd %= 103;
            }

            codeBytes.Add((byte)cd);
            codeBytes.Add(106);

            return codeBytes;
        }
    }
}
