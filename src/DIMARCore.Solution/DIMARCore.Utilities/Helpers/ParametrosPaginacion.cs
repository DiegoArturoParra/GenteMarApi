namespace DIMARCore.Utilities.Helpers
{
    /// <summary>
    /// Clase para mandar parametros de paginación
    /// </summary>
    public class ParametrosPaginacion
    {
        /// <summary>
        /// propiedad para ingresar la pagina.
        /// </summary>
        /// <autor>diego parra</autor>

        public int Page { get; set; } = 1;
        /// <summary>
        /// propiedad para el tamaño del paginado.
        /// </summary>
        public int PageSize { get; set; } = 25;
    }
}
