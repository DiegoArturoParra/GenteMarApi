using GenteMarCore.Entities;
using System;

namespace GenteMarCore.Tests.Global
{

    public class TestSetup : IDisposable
    {
        private GenteDeMarCoreContext _context;
        public const string RutaInicial = "Recursos";
        public const string RutaModuloTipoDocumento = "DATOS_BASICOS/IMAGENES_GENTE_MAR";
        public const string NombreArchivo = "imagen-test.png";
        public TestSetup()
        {
            _context = new GenteDeMarCoreContext();
            Context = _context;
        }
        public GenteDeMarCoreContext Context { get; private set; }
        public void Dispose()
        {
            _context?.Dispose(); // Asegúrate de que el Dispose se llame correctamente
        }
    }
}
