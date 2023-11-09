using System.CodeDom;

namespace DIMARCore.Utilities.Helpers
{
    public class Constantes
    {
        public const string COLOMBIA_CODIGO = "160";
        public const int ID_APLICACION = 10;
        public const string PATH_REPOSITORIO_ARCHIVOS = "~/RepositorioArchivos";
        public const string CARPETA_MODULO_DATOSBASICOS = "DATOS_BASICOS";
        public const string CARPETA_MODULO_TITULOS = "TITULOS";
        public const string CARPETA_MODULO_LICENCIAS = "LICENCIAS";
        public const string CARPETA_MODULO_ESTUPEFACIENTES = "ESTUPEFACIENTES";
        public const string CARPETA_OBSERVACIONES = "OBSERVACIONES";
        public const string CARPETA_IMAGENES = "IMAGENES_GENTE_MAR";
        public const string CARPETA_ACLARACION_EXPEDIENTE = "HISTORICO_ACLARACION_EXPEDIENTE";
        public const string FOOTER_EMAIL = @"AVISO LEGAL: Este mensaje es solamente para la persona a la que va dirigido. 
                                             Puede contener información confidencial o legalmente protegida. Ni la confidencialidad o privilegio alguno se pierde por cualquier transmisión errónea.
                                             Si usted ha recibido este mensaje por error, por favor elimine de su sistema inmediatamente este mensaje y sus copias, 
                                             así como todas las copias físicas e informe de esto al remitente. No debe, directa o indirectamente, usar, revelar, distribuir,
                                             imprimir o copiar ninguna de las partes de este mensaje si usted no es el destinatario final.";

        public const string TRAMITE_TITULOS = "TITULOS";
        public const string PASS_DEFAULT = "triton12345";
        public const string TRAMITE_LICENCIA = "LICENCIA";
        public const string PREVISTATRAMITE = "Tramite";
        public const string PREVISTAGENERADA = "Prevista Generada";
        public const string PREVISTAIMPRESA = "Documento Impreso";
        public const bool ACTIVO = true;
        public const bool INACTIVO = false;
        public const string REGEXPUNTOSMIL = @"\B(?=(\d{3})+(?!\d))";
        public const string SEPARADORDOCUMENTO = ".";
        public const string NAVEACTIVA = "A";
        public const string ESTADOTRAMITESGDA = "Tramite";
        public const string NAME_KEY_ENCRYPTION = "NameHash";
        public const string EXTENSION_EXCEL = ".xlsx";
        public const string PATH_PLANTILLA_LICENCIA_NAVEGACION = "Templates/Licencias/PLANTILLALICENCIADENAVEGACION.html";
        public const string PLANTILLALICENCIAPERITO = "Templates/Licencias/PLANTILLALICENCIAPERITO.html";
        public const string PLANTILLALICENCIAPILOTO = "Templates/Licencias/PLANTILLALICENCIAPILOTO.html";
        public const string PLANTILLALICENCIAPEP = "Templates/Licencias/PLANTILLALICENCIAPEP.html";
        public const string PLANTILLATITULO = "Templates/Titulos/TITULO.html";
        public const string PLANTILLAREFRENDO = "Templates/Titulos/REFRENDO.html";
        public const string KEY_ENVIRONMENT = "Environment";
        public const string KEY_EXPIRATION_CACHE = "ExpirationCache";
        public const string SIN_OBSERVACION = "SIN OBSERVACIÓN";
        public const string SIN_ACLARACION = "SIN ACLARACIÓN";
        public const string OBSERVACION_PENDIENTE = "Observación Pendiente";
        public const string OBSERVACION_REGISTRADA = "Observación Registrada";
        public const string SIGLA_CONSOLIDADO = "C-";
        public const string NUMERO_DE_LOTES = "NumeroDeLotes";
    }
}
