using DIMARCore.Utilities.Enums;
using System.Collections.Generic;
using System.Web.Http;

namespace DIMARCore.Api.Core.Atributos
{
    /// <summary>
    /// Clase para autorizar varios roles en los controladores
    /// </summary>
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// se concatena los roles por comas
        /// </summary>
        /// <param name="rolesEnum"></param>
        public AuthorizeRolesAttribute(params RolesEnum[] rolesEnum) : base()
        {
            List<string> roles = new List<string>();
            foreach (var item in rolesEnum)
            {
                string rol = EnumConfig.GetDescription(item);
                roles.Add(rol);
            }
            Roles = string.Join(",", roles);
        }
    }
}