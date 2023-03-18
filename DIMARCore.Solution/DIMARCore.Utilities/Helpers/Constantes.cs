using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public const string FOOTER_EMAIL = @"AVISO LEGAL: Este mensaje es solamente para la persona a la que va dirigido. 
                                             Puede contener información confidencial o legalmente protegida. Ni la confidencialidad o privilegio alguno se pierde por cualquier transmisión errónea.
                                             Si usted ha recibido este mensaje por error, por favor elimine de su sistema inmediatamente este mensaje y sus copias, 
                                             así como todas las copias físicas e informe de esto al remitente. No debe, directa o indirectamente, usar, revelar, distribuir,
                                             imprimir o copiar ninguna de las partes de este mensaje si usted no es el destinatario final.";

        public const string TRAMITE_TITULOS = "TITULOS";
        public const string TRAMITE_LICENCIA = "LICENCIA";
        public const string TIPO_TRAMITE = "Tramite";

        public const bool ACTIVO = true;
        public const bool INACTIVO = false;
        public const string REGEXPUNTOSMIL = @"\B(?=(\d{3})+(?!\d))"; 
        public const string SEPARADORDOCUMENTO = ".";
    }
}
