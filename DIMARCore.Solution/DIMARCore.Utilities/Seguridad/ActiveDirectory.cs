using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.DirectoryServices;

namespace DIMARCore.Utilities.Seguridad
{
    public class ActiveDirectory
    {
        private static string GENERIC_DIRECTORY = @"LDAP://RootDSE";
        private static string DOMAIN_PATH_DIMAR = "dimar.mil.co";

        private static string GetCurrentDomainGenericPath()
        {
            DirectoryEntry directorio = new DirectoryEntry(GENERIC_DIRECTORY);
            return $"LDAP://{directorio.Properties["defaultNamingContext"][0]}";
        }
        private static string GetCurrentDomainDimarPath()
        {
            return $"LDAP://{DOMAIN_PATH_DIMAR}";
        }

        private static DirectoryEntry CreateDirectoryEntry(string userName, string password)
        {
            DirectoryEntry ldapConnection = new DirectoryEntry
            {
                Path = GetCurrentDomainGenericPath(),
                Username = userName,
                Password = password,
                AuthenticationType = AuthenticationTypes.Secure
            };
            return ldapConnection;
        }

        /// <summary>
        /// Obtiene los datos de todos los usuarios registrados en el Active Directory
        /// </summary>
        /// <param name="userName">variable para logeo al Active Directory</param>
        /// <param name="password">variable contraseña para logeo al Active Directory</param>
        /// <returns></returns>
        public Respuesta GetUsersByActiveDirectory(ActiveDirectoryFilter filtro)
        {
            List<UserDirectory> usersList = new List<UserDirectory>();
            try
            {
                using (DirectoryEntry ldapConnection = new DirectoryEntry(GetCurrentDomainDimarPath()))
                {
                    // Crear un objeto de búsqueda en el directorio
                    using (DirectorySearcher directorySearcher = BuildUserSearcher(ldapConnection))
                    {
                        string consulta = string.Empty;
                        if (!string.IsNullOrEmpty(filtro.Nombres))
                        {
                            consulta = $"({ActiveDirectoryConfig.name}=*{filtro.Nombres}*)";
                        }
                        if (!string.IsNullOrEmpty(filtro.LoginName))
                        {
                            var consultaNombreUsuario = $"(|({ActiveDirectoryConfig.sAMAccountName}=*{filtro.LoginName}*)({ActiveDirectoryConfig.userPrincipalName}=*{filtro.LoginName}*))";
                            consulta = string.IsNullOrEmpty(consulta) ? consultaNombreUsuario : $"{consulta}{consultaNombreUsuario}";

                        }
                        if (!string.IsNullOrEmpty(filtro.Identificacion))
                        {
                            var consultaIdentificacion = $"({ActiveDirectoryConfig.ipPhone}=*{filtro.Identificacion}*)";
                            consulta = string.IsNullOrEmpty(consulta) ? consultaIdentificacion : $"{consulta}{consultaIdentificacion}";
                        }

                        var consultaEstado = $"({ActiveDirectoryConfig.userAccountControl}={(int)StatusDirectoryEnum.NORMAL_ACCOUNT})";
                        consulta = string.IsNullOrEmpty(consulta) ? consultaEstado : $"{consulta}{consultaEstado}";

                        consulta = $"&({consulta})";
                        directorySearcher.Filter = $"(&(objectCategory=User)(objectClass=person)({consulta}))";
                        // Obtener los resultados de la búsqueda
                        SearchResultCollection searchResults = directorySearcher.FindAll();

                        if (searchResults != null)
                        {
                            foreach (SearchResult sr in searchResults)
                            {

                                UserDirectory usuarioDirectorioActivo = LoadUserDataActiveDirectory(sr, null);
                                if (usuarioDirectorioActivo != null)
                                {
                                    usersList.Add(usuarioDirectorioActivo);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Responses.SetInternalServerErrorResponse(ex, "Error autenticación con el directorio activo.");
            }
            return Responses.SetOkResponse(usersList);
        }
        /// <summary>
        /// Obtiene los datos de un usuario del directorio activo
        /// </summary>
        /// <param name="userName">variable para logeo al Active Directory desde el front-end</param>
        /// <param name="password">variable contraseña para logeo al Active Directory desde el front-end</param>
        /// <returns></returns>
        public Respuesta GetUserByActiveDirectory(string userName, string password)
        {
            try
            {
                using (DirectoryEntry ldapConnection = CreateDirectoryEntry(userName, password))
                {
                    // Crear un objeto de búsqueda en el directorio
                    using (DirectorySearcher directorySearcher = new DirectorySearcher(ldapConnection))
                    {
                        SearchResult resultadoBusqueda = null;
                        directorySearcher.SearchRoot = ldapConnection;
                        directorySearcher.SearchScope = SearchScope.Subtree;
                        directorySearcher.Filter = $"(&(objectCategory=person)(objectClass=user)(sAMAccountName={userName}))";
                        resultadoBusqueda = directorySearcher.FindOne();
                        DirectoryEntry userEntry = resultadoBusqueda?.GetDirectoryEntry();
                        if (userEntry == null)
                            return Responses.SetUnathorizedResponse("La combinación usuario/contraseña es incorrecta.");
                        // Crear un objeto UserInfo y almacenar las propiedades del usuario
                        UserDirectory userDirectory = LoadUserDataActiveDirectory(null, userEntry);

                        return ValidarEstados(userDirectory);
                    }
                }
            }
            catch (DirectoryServicesCOMException)
            {
                return Responses.SetUnathorizedResponse("La combinación usuario/contraseña es incorrecta.");

            }
            catch (Exception ex)
            {
                return Responses.SetInternalServerErrorResponse(ex, "Error autenticación con el directorio activo.");
            }
        }

        private Respuesta ValidarEstados(UserDirectory user)
        {
            Respuesta respuesta = new Respuesta();
            if (ValidarEstadoActivoUsuario(user.Estado))
                respuesta = Responses.SetOkResponse(user);

            else if (user.Estado == (int)StatusDirectoryEnum.LOCKOUT)
                respuesta = Responses.SetUnathorizedResponse("Su cuenta esta deshabilitada.");

            else if (user.Estado == (int)StatusDirectoryEnum.NORMAL_ACCOUNT_BLOCKED)
                respuesta = Responses.SetUnathorizedResponse("Su cuenta esta bloqueada.");


            return respuesta;
        }

        /// <summary>
        /// Valida si el usuario se encuentra activo
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        private bool ValidarEstadoActivoUsuario(int estado)
        {
            bool activo = false;

            if (estado == (int)StatusDirectoryEnum.NORMAL_ACCOUNT ||
                estado == (int)StatusDirectoryEnum.Enabled_Password_Not_Required_544 ||
                estado == (int)StatusDirectoryEnum.Enabled_Password_Doesnt_Expire_66048 ||
                estado == (int)StatusDirectoryEnum.Enabled_Smartcard_Required_262656)
            {
                activo = true;
            }

            return activo;
        }

        /// <summary>
        /// Define listado de propiedades del usuario, a buscar en el directorio activo
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        private DirectorySearcher BuildUserSearcher(DirectoryEntry de)
        {
            DirectorySearcher ds = null;
            ds = new DirectorySearcher(de)
            {
                PropertiesToLoad = {
                    ActiveDirectoryConfig.name, // Full Name
                    ActiveDirectoryConfig.ipPhone, // Identificación
                    ActiveDirectoryConfig.userPrincipalName, // Email Name
                    ActiveDirectoryConfig.mail, // Email Address
                    ActiveDirectoryConfig.sAMAccountName, // Login Name
                    ActiveDirectoryConfig.givenName, // First Name
                    ActiveDirectoryConfig.sn, // Last Name (Surname)
                    ActiveDirectoryConfig.displayName, // displayName
                    ActiveDirectoryConfig.userPrincipalName, // userPrincipalName
                    ActiveDirectoryConfig.l,
                    ActiveDirectoryConfig.st,
                    ActiveDirectoryConfig.cn,
                    ActiveDirectoryConfig.physicalDeliveryOfficeName,
                    ActiveDirectoryConfig.distinguishedName, // Distinguished Name
                    ActiveDirectoryConfig.userAccountControl // Estado
                }
            };

            return ds;
        }


        /// <summary>
        /// Carga los datos del usuario dada información del directorio activo
        /// </summary>
        /// <param name="sr">Datos del usuario</param>
        /// <returns></returns>
        private UserDirectory LoadUserDataActiveDirectory(SearchResult sr, DirectoryEntry directoryEntry)
        {
            UserDirectory userDirectory = new UserDirectory();
            if (directoryEntry != null)
            {
                userDirectory = new UserDirectory
                {
                    Identificacion = GetPropertyValue(directoryEntry, ActiveDirectoryConfig.ipPhone),
                    Email = GetPropertyValue(directoryEntry, ActiveDirectoryConfig.userPrincipalName),
                    Nombres = GetPropertyValue(directoryEntry, ActiveDirectoryConfig.name),
                    Capitania = GetPropertyValue(directoryEntry, ActiveDirectoryConfig.physicalDeliveryOfficeName),
                    LoginName = GetPropertyValue(directoryEntry, ActiveDirectoryConfig.sAMAccountName),
                    Estado = !string.IsNullOrWhiteSpace(GetPropertyValue(directoryEntry, ActiveDirectoryConfig.userAccountControl)) ?
                Convert.ToInt32(GetPropertyValue(directoryEntry, ActiveDirectoryConfig.userAccountControl)) : 0
                };
            }
            else if (sr != null)
            {
                userDirectory = new UserDirectory
                {
                    Identificacion = sr.GetPropertyValue(ActiveDirectoryConfig.ipPhone),
                    Email = sr.GetPropertyValue(ActiveDirectoryConfig.userPrincipalName),
                    Nombres = sr.GetPropertyValue(ActiveDirectoryConfig.name),
                    Capitania = sr.GetPropertyValue(ActiveDirectoryConfig.physicalDeliveryOfficeName),
                    LoginName = sr.GetPropertyValue(ActiveDirectoryConfig.sAMAccountName),
                    Estado = !string.IsNullOrWhiteSpace(sr.GetPropertyValue(ActiveDirectoryConfig.userAccountControl)) ?
                Convert.ToInt32(sr.GetPropertyValue(ActiveDirectoryConfig.userAccountControl)) : 0
                };
            }
            return userDirectory;
        }

        /// <summary>
        /// Obtiene el valor de una propiedad en el directorio
        /// </summary>
        /// <param name="sr">directorio</param>
        /// <param name="propertyName">Nombre de la propiedad</param>
        /// <returns>Valor de la propiedad. Si no se encuentra retorna una cadena vacia</returns>
        private string GetPropertyValue(DirectoryEntry sr, string propertyName)
        {
            string ret = string.Empty;

            if (sr.Properties[propertyName].Count > 0)
            {
                ret = sr.Properties[propertyName][0].ToString();
            }

            return ret;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class ADExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetPropertyValue(this SearchResult sr, string propertyName)
        {
            string ret = string.Empty;

            if (sr.Properties[propertyName].Count > 0)
            {
                ret = sr.Properties[propertyName][0].ToString();
            }

            return string.IsNullOrEmpty(ret) ? string.Empty : ret;
        }
    }
}