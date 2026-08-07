using DanfeSharp;
using DanfeSharp.NFSe.Elementos;
using DanfeSharp.NFSe.Modelo;

namespace DanfeSharp.NFSe.Blocos
{
    /// <summary>
    /// Bloco da Tributação IBS/CBS, item 2.1.10 do manual.
    /// </summary>
    internal class BlocoTributacaoIBSCBS : DanfseBlocoBase
    {
        public BlocoTributacaoIBSCBS(DanfseViewModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {
            var i = viewModel.TributacaoIBSCBS;

            AdicionarLinhaComTitulo("Tributação IBS/CBS")
                .ComCampoDanfse("CST / cClassTrib", i.CstCClassTrib)
                .ComCampoDanfse("Indicador de Operação / Código IBGE Incidência / Município Incidência / Sigla UF", i.IndicadorOperacaoMunicipioIncidencia)
                .ComLarguras(25, 25, 50);

            AdicionarLinhaCampos()
                .ComCampoDanfse("Exclusões e Reduções da Base de Cálculo", i.ExclusoesReducoesBaseCalculo)
                .ComCampoDanfse("Base de Cálculo Após Exclusões e Reduções", i.BaseCalculoAposExclusoesReducoes)
                .ComCampoDanfse("Red. Alíquota IBS / Red. Alíquota CBS", i.ReducaoAliquotaIbsCbs)
                .ComCampoDanfse("Alíquota - IBS UF / IBS Mun", i.AliquotaIbsUfMun)
                .ComLargurasIguais();

            AdicionarLinhaCampos()
                .ComCampoDanfse("Alíq. Efetiva Municipal - IBS", i.AliquotaEfetivaMunicipalIbs)
                .ComCampoDanfse("Valor Apurado Municipal - IBS", i.ValorApuradoMunicipalIbs)
                .ComCampoDanfse("Alíq. Efetiva Estadual - IBS", i.AliquotaEfetivaEstadualIbs)
                .ComCampoDanfse("Valor Apurado Estadual - IBS", i.ValorApuradoEstadualIbs)
                .ComLargurasIguais();

            AdicionarLinhaCampos()
                .ComCampoDanfse("Valor Total Apurado - IBS", i.ValorTotalApuradoIbs)
                .ComCampoDanfse("Alíquota - CBS", i.AliquotaCbs)
                .ComCampoDanfse("Alíquota Efetiva - CBS", i.AliquotaEfetivaCbs)
                .ComCampoDanfse("Valor Total Apurado - CBS", i.ValorTotalApuradoCbs)
                .ComLargurasIguais();
        }
    }
}
