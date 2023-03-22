using System;
using DIMARCore.Utilities.Helpers;
using System.DirectoryServices;
using System.Net;
using DIMARCore.Utilities.Enums;

namespace DIMARCore.Utilities.Seguridad
{
    public class ActiveDirectory
    {
        private static string GENERIC_DIRECTORY = @"LDAP://RootDSE";

        private static string GetCurrentDomainPath()
        {
            DirectoryEntry directorio = new DirectoryEntry(GENERIC_DIRECTORY);
            return "LDAP://" + directorio.Properties["defaultNamingContext"][0];
        }

        private static DirectoryEntry CreateDirectoryEntry(string userName, string password)
        {
            DirectoryEntry ldapConnection = new DirectoryEntry
            {
                Path = GetCurrentDomainPath(),
                Username = userName,
                Password = password,
                AuthenticationType = AuthenticationTypes.Secure
            };
            return ldapConnection;
        }

        public Respuesta GetUserActiveDirectory(string userName, string password)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                DirectoryEntry ldapConnection = CreateDirectoryEntry(userName, password);
                DirectorySearcher searcher = new DirectorySearcher(ldapConnection);
                SearchResult resultadoBusqueda = null;
                searcher.SearchRoot = ldapConnection;
                searcher.SearchScope = SearchScope.Subtree;
                searcher.Filter = $"(&(objectCategory=person)(objectClass=user)(sAMAccountName={userName}))";
                resultadoBusqueda = searcher.FindOne();
                DirectoryEntry usuario = resultadoBusqueda?.GetDirectoryEntry();
                if (usuario == null)

                    return new Respuesta
                    {
                        Data = null,
                        Mensaje = "La combinación usuario/contraseña es incorrecta.",
                        Estado = false,
                        StatusCode = HttpStatusCode.Unauthorized
                    };
                {
                    UserDirectory userDirectory = new UserDirectory();
                    userDirectory.Identificacion = usuario.Properties["ipPhone"].Value != null
                        ? usuario.Properties["ipPhone"][0].ToString()
                        : "N/A";
                    userDirectory.Email = usuario.Properties["userPrincipalName"].Value == null
                        ? "N/A"
                        : usuario.Properties["userPrincipalName"][0].ToString();
                    userDirectory.LoginName = usuario.Properties["sAMAccountName"][0].ToString();
                    userDirectory.Estado = usuario.Properties["userAccountControl"].Value == null
                        ? 0
                        : Convert.ToInt32(usuario.Properties["userAccountControl"][0]);

                    respuesta = validarEstados(userDirectory.Estado);
                    if (respuesta.Estado)
                    {
                        respuesta.Data = userDirectory;
                        respuesta.Mensaje = "ok";
                        respuesta.StatusCode = HttpStatusCode.OK;
                        respuesta.Estado = true;
                    }
                }
                searcher.Dispose();
                ldapConnection.Dispose();
            }
            catch (DirectoryServicesCOMException ex)
            {
                respuesta.Data = null;
                respuesta.MensajeExcepcion = ex.Message;
                respuesta.Mensaje = "La combinación usuario/contraseña es incorrecta.";
                respuesta.Estado = false;
                respuesta.StatusCode = HttpStatusCode.Unauthorized;
            }

            return respuesta;
        }

        private Respuesta validarEstados(int userDirectoryEstado)
        {
            Respuesta respuesta = new Respuesta();
            if (userDirectoryEstado == (int)StatusDirectoryEnum.NORMAL_ACCOUNT ||
                userDirectoryEstado == (int)StatusDirectoryEnum.NORMAL_ACCOUNT_DONT_EXPIRE_PASS)
            {
                respuesta = Responses.SetOkResponse();
            }
            else if (userDirectoryEstado == (int)StatusDirectoryEnum.LOCKOUT)
            {
                respuesta = Responses.SetUnathorizedResponse("Su cuenta esta deshabilitada.");
            }
            else if (userDirectoryEstado == (int)StatusDirectoryEnum.NORMAL_ACCOUNT_BLOCKED)
            {
                respuesta = Responses.SetUnathorizedResponse("Su cuenta esta bloqueada.");
            }

            return respuesta;
        }
    }
}