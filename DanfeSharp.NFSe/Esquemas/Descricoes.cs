using System.Collections.Generic;

namespace DanfeSharp.NFSe.Esquemas
{
    /// <summary>
    /// Descrições textuais dos campos codificados do leiaute da NFS-e/DPS, conforme a documentação dos
    /// tipos simples do XSD (tiposSimples_v1.01.xsd), usadas para exibição no DANFSe (item 2.4.5 do manual).
    /// </summary>
    public static class Descricoes
    {
        private static string Buscar(Dictionary<string, string> mapa, string codigo, string valorPadrao = "")
        {
            if (string.IsNullOrWhiteSpace(codigo)) return valorPadrao;
            return mapa.TryGetValue(codigo.Trim(), out var descricao) ? descricao : codigo;
        }

        #region Emitente da NFS-e (tpEmit)

        private static readonly Dictionary<string, string> _TpEmit = new Dictionary<string, string>
        {
            ["1"] = "Prestador",
            ["2"] = "Tomador",
            ["3"] = "Intermediário",
        };

        public static string TpEmit(string codigo) => Buscar(_TpEmit, codigo);

        #endregion

        #region Situação da NFS-e (cStat)

        private static readonly Dictionary<string, string> _CStat = new Dictionary<string, string>
        {
            ["100"] = "NFS-e Gerada",
            ["102"] = "NFS-e de Decisão Judicial ou Administrativa",
            ["103"] = "NFS-e Avulsa",
            ["107"] = "NFS-e MEI",
        };

        public static string CStat(string codigo) => Buscar(_CStat, codigo);

        #endregion

        #region Finalidade (finNFSe)

        private static readonly Dictionary<string, string> _FinNFSe = new Dictionary<string, string>
        {
            ["0"] = "NFS-e regular",
        };

        public static string FinNFSe(string codigo) => Buscar(_FinNFSe, codigo);

        #endregion

        #region Simples Nacional na Data de Competência (opSimpNac)

        private static readonly Dictionary<string, string> _OpSimpNac = new Dictionary<string, string>
        {
            ["1"] = "Não Optante",
            ["2"] = "Optante - Microempreendedor Individual (MEI)",
            ["3"] = "Optante - Microempresa ou Empresa de Pequeno Porte (ME/EPP)",
        };

        public static string OpSimpNac(string codigo) => Buscar(_OpSimpNac, codigo);

        #endregion

        #region Regime de Apuração Tributária pelo SN (regApTribSN)

        private static readonly Dictionary<string, string> _RegApTribSN = new Dictionary<string, string>
        {
            ["1"] = "Regime de apuração dos tributos federais e municipal pelo Simples Nacional",
            ["2"] = "Regime de apuração dos tributos federais pelo Simples Nacional e ISSQN por fora do Simples Nacional conforme respectiva legislação municipal do tributo",
            ["3"] = "Regime de apuração dos tributos federais e municipal por fora do Simples Nacional conforme respectivas legislações federal e municipal de cada tributo",
        };

        public static string RegApTribSN(string codigo) => Buscar(_RegApTribSN, codigo);

        #endregion

        #region Regime Especial de Tributação do ISSQN (regEspTrib)

        private static readonly Dictionary<string, string> _RegEspTrib = new Dictionary<string, string>
        {
            ["0"] = "Nenhum",
            ["1"] = "Ato Cooperado (Cooperativa)",
            ["2"] = "Estimativa",
            ["3"] = "Microempresa Municipal",
            ["4"] = "Notário ou Registrador",
            ["5"] = "Profissional Autônomo",
            ["6"] = "Sociedade de Profissionais",
            ["9"] = "Outros",
        };

        public static string RegEspTrib(string codigo) => Buscar(_RegEspTrib, codigo);

        #endregion

        #region Tipo de Tributação do ISSQN (tribISSQN)

        private static readonly Dictionary<string, string> _TribISSQN = new Dictionary<string, string>
        {
            ["1"] = "Operação Tributável",
            ["2"] = "Imunidade",
            ["3"] = "Exportação de Serviço",
            ["4"] = "Não Incidência",
        };

        public static string TribISSQN(string codigo) => Buscar(_TribISSQN, codigo);

        #endregion

        #region Tipo de Imunidade do ISSQN (tpImunidade)

        private static readonly Dictionary<string, string> _TpImunidade = new Dictionary<string, string>
        {
            ["0"] = "Imunidade (tipo não informado na nota de origem)",
            ["1"] = "Patrimônio, renda ou serviços, uns dos outros",
            ["2"] = "Templos de qualquer culto",
            ["3"] = "Patrimônio, renda ou serviços dos partidos políticos, entidades sindicais dos trabalhadores e instituições de educação e assistência social",
            ["4"] = "Livros, jornais, periódicos e o papel destinado a sua impressão",
            ["5"] = "Fonogramas e videofonogramas musicais produzidos no Brasil",
        };

        public static string TpImunidade(string codigo) => Buscar(_TpImunidade, codigo);

        #endregion

        #region Suspensão da Exigibilidade do ISSQN (tpSusp)

        private static readonly Dictionary<string, string> _TpSusp = new Dictionary<string, string>
        {
            ["1"] = "Exigibilidade Suspensa por Decisão Judicial",
            ["2"] = "Exigibilidade Suspensa por Processo Administrativo",
        };

        public static string TpSusp(string codigo) => Buscar(_TpSusp, codigo);

        #endregion

        #region Benefício Municipal (tpBM)

        private static readonly Dictionary<string, string> _TpBM = new Dictionary<string, string>
        {
            ["1"] = "Isenção",
            ["2"] = "Redução da Base de Cálculo",
            ["3"] = "Redução da Base de Cálculo",
            ["4"] = "Alíquota Diferenciada",
        };

        public static string TpBM(string codigo) => Buscar(_TpBM, codigo);

        #endregion

        #region Retenção do ISSQN (tpRetISSQN)

        private static readonly Dictionary<string, string> _TpRetISSQN = new Dictionary<string, string>
        {
            ["1"] = "Não Retido",
            ["2"] = "Retido pelo Tomador",
            ["3"] = "Retido pelo Intermediário",
        };

        public static string TpRetISSQN(string codigo) => Buscar(_TpRetISSQN, codigo);

        #endregion

        #region Descrição Contribuições Sociais - Retidas (tpRetPisCofins)

        private static readonly Dictionary<string, string> _TpRetPisCofins = new Dictionary<string, string>
        {
            ["0"] = "PIS/COFINS/CSLL Não Retidos",
            ["1"] = "PIS/COFINS Retidos",
            ["2"] = "PIS/COFINS Não Retidos",
            ["3"] = "PIS/COFINS/CSLL Retidos",
            ["4"] = "PIS/COFINS Retidos, CSLL Não Retido",
            ["5"] = "PIS Retido, COFINS/CSLL Não Retido",
            ["6"] = "COFINS Retido, PIS/CSLL Não Retido",
            ["7"] = "PIS Não Retido, COFINS/CSLL Retidos",
            ["8"] = "PIS/COFINS Não Retidos, CSLL Retido",
            ["9"] = "COFINS Não Retido, PIS/CSLL Retidos",
        };

        public static string TpRetPisCofins(string codigo) => Buscar(_TpRetPisCofins, codigo);

        #endregion

        #region Ambiente Gerador da NFS-e (ambGer)

        private static readonly Dictionary<string, string> _AmbGer = new Dictionary<string, string>
        {
            ["1"] = "Prefeitura",
            ["2"] = "Sistema Nacional da NFS-e",
        };

        public static string AmbGer(string codigo) => Buscar(_AmbGer, codigo);

        #endregion

        #region Tipo de Ambiente (tpAmb)

        private static readonly Dictionary<string, string> _TpAmb = new Dictionary<string, string>
        {
            ["1"] = "Produção",
            ["2"] = "Homologação",
        };

        public static string TpAmb(string codigo) => Buscar(_TpAmb, codigo);

        #endregion
    }
}
