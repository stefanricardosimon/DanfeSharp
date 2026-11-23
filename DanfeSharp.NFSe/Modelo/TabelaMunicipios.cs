using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DanfeSharp.NFSe.Modelo
{
    /// <summary>
    /// Resolve o nome de um município a partir do seu código do IBGE (7 dígitos), usando a tabela
    /// de municípios do IBGE embutida na DLL (Resources\MunicipiosIbge.txt).
    /// O leiaute da NFS-e só traz o código do município nos endereços nacionais de prestador,
    /// tomador/adquirente, destinatário e intermediário; esta tabela permite exibir o nome no DANFSe.
    /// A tabela foi gerada a partir da API de Localidades do IBGE
    /// (https://servicodados.ibge.gov.br/api/v1/localidades/municipios?view=nivelado): uma linha
    /// "codigo;nome" por município, em UTF-8 sem BOM, ordenada pelo código.
    /// </summary>
    public static class TabelaMunicipios
    {
        private const string NomeRecurso = "DanfeSharp.NFSe.Resources.MunicipiosIbge.txt";

        private static readonly Lazy<Dictionary<string, string>> _NomePorCodigo =
            new Lazy<Dictionary<string, string>>(Carregar);

        /// <summary>
        /// Retorna o nome do município a partir do código do IBGE, ou null se o código não constar da tabela.
        /// </summary>
        public static string ObterNome(string codigoIbgeMunicipio)
        {
            if (string.IsNullOrWhiteSpace(codigoIbgeMunicipio)) return null;
            return _NomePorCodigo.Value.TryGetValue(codigoIbgeMunicipio.Trim(), out var nome) ? nome : null;
        }

        /// <summary>
        /// Resolvedor padrão do DANFSe: retorna o nome do município ou, se o código não constar da tabela
        /// (por exemplo, um município criado depois da versão embutida), o próprio código.
        /// </summary>
        public static string ResolverNomeOuCodigo(string codigoIbgeMunicipio)
        {
            return ObterNome(codigoIbgeMunicipio) ?? codigoIbgeMunicipio;
        }

        private static Dictionary<string, string> Carregar()
        {
            var tabela = new Dictionary<string, string>(StringComparer.Ordinal);

            using (var stream = typeof(TabelaMunicipios).Assembly.GetManifestResourceStream(NomeRecurso))
            {
                if (stream == null)
                    throw new InvalidOperationException($"Recurso '{NomeRecurso}' não encontrado no assembly.");

                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string linha;
                    while ((linha = reader.ReadLine()) != null)
                    {
                        var separador = linha.IndexOf(';');
                        if (separador <= 0) continue;

                        tabela[linha.Substring(0, separador)] = linha.Substring(separador + 1);
                    }
                }
            }

            return tabela;
        }
    }
}
