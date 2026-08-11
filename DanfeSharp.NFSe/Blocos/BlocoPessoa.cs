using DanfeSharp.NFSe.Elementos;
using DanfeSharp.NFSe.Interno;
using DanfeSharp.NFSe.Modelo;

namespace DanfeSharp.NFSe.Blocos
{
    /// <summary>
    /// Bloco reutilizável para Prestador/Fornecedor, Tomador/Adquirente e Intermediário da Operação
    /// (itens 2.1.3, 2.1.4 e 2.1.6 do manual).
    /// </summary>
    internal class BlocoPessoa : DanfseBlocoBase
    {
        private readonly bool _linhaSuperior;

        public BlocoPessoa(string titulo, PessoaViewModel pessoa, string mensagemNaoIdentificado, bool mostrarSimplesNacional,
            DanfseViewModel viewModel, Estilo estilo, bool linhaSuperior = true) : base(viewModel, estilo)
        {
            _linhaSuperior = linhaSuperior;

            if (!pessoa.Identificado)
            {
                AdicionarLinhaCampos().ComCampoDanfse(null, mensagemNaoIdentificado, AlinhamentoHorizontal.Centro).ComLarguras(0);
                return;
            }

            AdicionarLinhaComTitulo(titulo)
                .ComCampoDanfse("CNPJ / CPF / NIF", pessoa.CnpjCpfNif)
                .ComCampoDanfse("Indicador Municipal (Inscrição)", pessoa.IndicadorMunicipal)
                .ComCampoDanfse("Telefone", pessoa.Telefone)
                .ComLargurasIguais();

            AdicionarLinhaCampos()
                .ComCampoDanfse("Nome / Nome Empresarial", pessoa.NomeRazaoSocial)
                .ComCampoDanfse("Município / Sigla UF", pessoa.MunicipioUf)
                .ComCampoDanfse("Código IBGE / CEP", pessoa.CodigoIbgeCep)
                .ComLarguras(50, 25, 25);

            AdicionarLinhaCampos()
                .ComCampoDanfse("Endereço", pessoa.Endereco)
                .ComCampoDanfse("E-mail", pessoa.Email)
                .ComLarguras(50, 50);

            if (mostrarSimplesNacional)
            {
                AdicionarLinhaCampos()
                    .ComCampoDanfse("Simples Nacional na Data de Competência", pessoa.SimplesNacional)
                    .ComCampoDanfse("Regime de Apuração Tributária pelo SN", pessoa.RegimeApuracaoSN)
                    .ComLarguras(33, 67);
            }
        }

        protected override bool LinhaSuperior => _linhaSuperior;
    }
}
