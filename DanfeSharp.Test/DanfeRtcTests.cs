using DanfeSharp.Modelo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DanfeSharp.Test
{
    /// <summary>
    /// Testes dos campos da Reforma Tributária (IBS, CBS e IS) lidos do XML, conforme NT 2025.002 e NT 2026.010.
    /// </summary>
    [TestClass]
    public class DanfeRtcTests
    {
        private static readonly string PastaXml = Path.Combine("Xml", "NFe", "v4.00");

        private static DanfeViewModel Carregar(string arquivo) => DanfeViewModelCreator.CriarDeArquivoXml(Path.Combine(PastaXml, arquivo));

        [TestMethod]
        public void Item_ComIbsCbs_UsaAliquotasVigentes()
        {
            var p = Carregar("v4_IbsCbs.xml").Produtos[0];

            Assert.AreEqual("000001", p.ClassificacaoTributariaIbsCbs);
            Assert.AreEqual(100.00, p.BaseIbsCbs);
            Assert.AreEqual(0.10, p.AliquotaIbsUf);
            Assert.AreEqual(0.10, p.ValorIbsUf);
            Assert.AreEqual(0.00, p.AliquotaIbsMun);
            Assert.AreEqual(0.00, p.ValorIbsMun);
            Assert.AreEqual(0.90, p.AliquotaCbs);
            Assert.AreEqual(0.90, p.ValorCbs);
            Assert.AreEqual(100.00, p.BaseIpi);
        }

        [TestMethod]
        public void Item_ComReducaoDeAliquota_UsaAliquotaEfetiva()
        {
            var p = Carregar("v4_IbsCbs.xml").Produtos[1];

            // pIBSUF = 0,10 e pAliqEfet = 0,04 (NT 2026.010, item 4.3)
            Assert.AreEqual(0.04, p.AliquotaIbsUf);
            Assert.AreEqual(0.00, p.AliquotaIbsMun);
            Assert.AreEqual(0.36, p.AliquotaCbs);
        }

        [TestMethod]
        public void Item_ComImpostoSeletivo()
        {
            var p = Carregar("v4_IbsCbs.xml").Produtos[2];

            Assert.AreEqual(100.00, p.BaseIs);
            Assert.AreEqual(2.50, p.AliquotaIs);
            Assert.AreEqual(2.50, p.ValorIs);
        }

        [TestMethod]
        public void Item_SemGruposDaReformaTributaria_PermaneceNulo()
        {
            var p = Carregar("v4_IbsCbs.xml").Produtos[3];

            Assert.IsNull(p.ClassificacaoTributariaIbsCbs);
            Assert.IsNull(p.BaseIbsCbs);
            Assert.IsNull(p.AliquotaIbsUf);
            Assert.IsNull(p.ValorCbs);
            Assert.IsNull(p.BaseIs);
            Assert.IsNull(p.ValorIs);
            Assert.IsNull(p.BaseIpi);
        }

        [TestMethod]
        public void Totais_ComIbsCbsIs()
        {
            var c = Carregar("v4_IbsCbs.xml").CalculoImposto;

            Assert.AreEqual(2.16, c.ValorCbs);
            Assert.AreEqual(0.24, c.ValorIbsUf);
            Assert.AreEqual(0.00, c.ValorIbsMun);
            Assert.AreEqual(2.50, c.ValorIs);
            Assert.AreEqual(0.50, c.ValorIbsMonofasico);
            Assert.AreEqual(0.60, c.ValorCbsMonofasica);
            Assert.IsNull(c.ValorIbsMonofasicoRetencao);
            Assert.IsNull(c.ValorCbsMonofasicaRetencao);
        }

        [TestMethod]
        public void Totais_IcmsFacultativos()
        {
            var c = Carregar("v4_IbsCbs.xml").CalculoImposto;

            Assert.AreEqual(1.00, c.vFCP);
            Assert.AreEqual(0.00, c.vFCPST);
            Assert.AreEqual(10.00, c.QuantidadeIcmsMonofasico);
            Assert.AreEqual(2.00, c.ValorIcmsMonofasico);
            Assert.AreEqual(5.00, c.QuantidadeIcmsMonofasicoRetencao);
            Assert.AreEqual(1.00, c.ValorIcmsMonofasicoRetencao);
        }

        [TestMethod]
        public void NotaSemGruposDaReformaTributaria_NaoInventaValores()
        {
            // Nota emitida antes da obrigatoriedade do IBS/CBS.
            var m = Carregar("v4_ComLocalEntrega.xml");

            Assert.IsNull(m.CalculoImposto.ValorCbs);
            Assert.IsNull(m.CalculoImposto.ValorIbsUf);
            Assert.IsNull(m.CalculoImposto.ValorIbsMun);
            Assert.IsNull(m.CalculoImposto.ValorIs);
            Assert.IsNull(m.CalculoImposto.ValorIbsMonofasico);
            Assert.IsNull(m.CalculoImposto.QuantidadeIcmsMonofasico);
            Assert.IsNull(m.Produtos[0].BaseIbsCbs);
            Assert.IsNull(m.Produtos[0].ValorCbs);
        }

        [TestMethod]
        public void Emitente_DescricaoDoRegimeTributario()
        {
            Assert.AreEqual("3 - Regime Normal", new EmpresaViewModel { CRT = "3" }.CRTDescricao);
            Assert.AreEqual("1 - Simples Nacional", new EmpresaViewModel { CRT = "1" }.CRTDescricao);
            Assert.AreEqual("4 - Simples Nacional - MEI", new EmpresaViewModel { CRT = "4" }.CRTDescricao);
            Assert.IsNull(new EmpresaViewModel().CRTDescricao);
        }
    }
}
