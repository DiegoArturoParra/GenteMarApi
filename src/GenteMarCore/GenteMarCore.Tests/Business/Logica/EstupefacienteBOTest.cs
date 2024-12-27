using DIMARCore.Business.Logica;
using DIMARCore.Repositories.Repository;
using DIMARCore.Utilities.Enums;
using GenteMarCore.Tests.Global;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenteMarCore.Tests.Business.Logica
{
    [TestClass]
    public class EstupefacienteBOTest
    {
        private TestSetup _setup;
        private EstupefacienteRepository _estupefacienteRepository;
        private EstupefacienteDatosBasicosRepository _estupefacienteDatosBasicosRepository;
        private DatosBasicosRepository _datosBasicosRepository;
        private TramiteEstupefacienteRepository _tramiteEstupefacienteRepository;
        private EstadoEstupefacienteRepository _estadoEstupefacienteRepository;
        private ObservacionesBO _observacionesBO;
        private EstupefacienteBO _estupefacienteBO;

        [TestInitialize]
        public void Setup()
        {
            _setup = new TestSetup();
            _datosBasicosRepository = new DatosBasicosRepository(_setup.Context);
            _estupefacienteDatosBasicosRepository = new EstupefacienteDatosBasicosRepository(_setup.Context);
            _estupefacienteRepository = new EstupefacienteRepository(_setup.Context);
            _tramiteEstupefacienteRepository = new TramiteEstupefacienteRepository(_setup.Context);
            _estadoEstupefacienteRepository = new EstadoEstupefacienteRepository(_setup.Context);
            _observacionesBO = new ObservacionesBO();
            _estupefacienteBO = new EstupefacienteBO(_estupefacienteRepository, _estupefacienteDatosBasicosRepository,
                            _datosBasicosRepository, _tramiteEstupefacienteRepository, _estadoEstupefacienteRepository, _observacionesBO);
        }

        [TestMethod]
        [DataRow((int)TipoTramiteEstupefacienteEnum.CONCESION)]
        [DataRow((int)TipoTramiteEstupefacienteEnum.TRANSPORTE)]
        public void ValidarRadicadoSiEsIlimitadoPorTramite_TipoTramiteValido_DeberiaRetornarVerdadero(int tipoTramiteId)
        {
            // Act
            var (tramites, existe) = _estupefacienteBO.ValidarRadicadoSiEsIlimitadoPorTramite(tipoTramiteId);

            // Assert
            Assert.IsTrue(existe);
            Assert.IsTrue(tramites.Contains(tipoTramiteId));
        }

        [TestMethod]
        [DataRow((int)TipoTramiteEstupefacienteEnum.NAVES)]
        [DataRow((int)TipoTramiteEstupefacienteEnum.GENTES)]
        public void ValidarRadicadoSiEsIlimitadoPorTramite_TipoTramiteInvalido_DeberiaRetornarFalso(int tipoTramiteId)
        {
            // Act
            var (tramites, existe) = _estupefacienteBO.ValidarRadicadoSiEsIlimitadoPorTramite(tipoTramiteId);

            // Assert
            Assert.IsFalse(existe);
            Assert.IsFalse(tramites.Contains(tipoTramiteId));
        }
    }
}
