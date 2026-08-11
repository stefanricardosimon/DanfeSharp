using System.Collections.Generic;

namespace DanfeSharp.NFSe.Modelo
{
    /// <summary>
    /// Resolve a sigla da UF a partir dos dois primeiros dígitos do código do IBGE do município,
    /// usado quando o leiaute da NFS-e só fornece o código do município (sem a sigla da UF), como
    /// nos endereços de tomador/adquirente, destinatário, intermediário e prestador.
    /// </summary>
    public static class TabelaUF
    {
        private static readonly Dictionary<string, string> _UFPorPrefixo = new Dictionary<string, string>
        {
            ["11"] = "RO",
            ["12"] = "AC",
            ["13"] = "AM",
            ["14"] = "RR",
            ["15"] = "PA",
            ["16"] = "AP",
            ["17"] = "TO",
            ["21"] = "MA",
            ["22"] = "PI",
            ["23"] = "CE",
            ["24"] = "RN",
            ["25"] = "PB",
            ["26"] = "PE",
            ["27"] = "AL",
            ["28"] = "SE",
            ["29"] = "BA",
            ["31"] = "MG",
            ["32"] = "ES",
            ["33"] = "RJ",
            ["35"] = "SP",
            ["41"] = "PR",
            ["42"] = "SC",
            ["43"] = "RS",
            ["50"] = "MS",
            ["51"] = "MT",
            ["52"] = "GO",
            ["53"] = "DF",
        };

        /// <summary>
        /// Retorna a sigla da UF a partir do código do IBGE do município (7 dígitos), ou null se não reconhecido.
        /// </summary>
        public static string ObterUFPorCodigoIbge(string codigoIbgeMunicipio)
        {
            if (string.IsNullOrWhiteSpace(codigoIbgeMunicipio) || codigoIbgeMunicipio.Length < 2) return null;

            var prefixo = codigoIbgeMunicipio.Substring(0, 2);
            return _UFPorPrefixo.TryGetValue(prefixo, out var uf) ? uf : null;
        }
    }
}
