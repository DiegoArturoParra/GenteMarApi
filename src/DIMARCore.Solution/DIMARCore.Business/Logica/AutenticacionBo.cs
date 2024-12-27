using DIMARCore.Repositories.Repository;
using DIMARCore.UIEntities.DTOs;
using DIMARCore.UIEntities.Requests;
using DIMARCore.Utilities.Config;
using DIMARCore.Utilities.Enums;
using DIMARCore.Utilities.Helpers;
using DIMARCore.Utilities.Seguridad;
using GenteMarCore.Entities.Models;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace DIMARCore.Business.Logica
{
    public class AutenticacionBO
    {
        private readonly int _expireTimeToken;
        private readonly SeguridadBO _seguridadBO;
        public AutenticacionBO()
        {
            _seguridadBO = new SeguridadBO();
            _expireTimeToken = Convert.ToInt32(ConfigurationManager.AppSettings[AutenticacionConfig.TOKEN_JWT_EXPIRE_MINUTES]);
        }

        public async Task<UserSesionDTO> Login(LoginRequest login)
        {
            // Se valida que el id de la aplicación sea valido
            await new AplicacionBO().GetAplicacion(login.Aplicacion);
            // Se busca y valida el usuario
            UserSesionDTO user = await _seguridadBO.ValidarUsuario(login.UserName, login.Password);
            user.NombresUsuario = string.IsNullOrWhiteSpace(user.NombresUsuario) ? user.NombresUsuario.Trim() : user.NombresUsuario;
            user.ApellidosUsuario = string.IsNullOrWhiteSpace(user.ApellidosUsuario) ? user.ApellidosUsuario.Trim() : user.ApellidosUsuario;
            return user;
        }

        public async Task<UserSesionDTO> LoginTest(LoginRequest login)
        {
            // Se valida que el id de la aplicación sea valido
            await new AplicacionBO().GetAplicacion(login.Aplicacion);
            // Se busca y valida el usuario
            UserSesionDTO user = await _seguridadBO.ValidarUsuarioTest(login.UserName, login.Password);
            return user;
        }

        public async Task<Respuesta> RegistroDeAutenticacion(UserSesionDTO user, string token, string ipAddress)
        {
            await CrearAsync(token, ipAddress, user.LoginId);
            Respuesta respuesta = _seguridadBO.ResultadoAutenticacion(user, token);
            return respuesta;
        }


        public async Task<Respuesta> CrearAsync(string token, string ipAddress, int loginId)
        {

            DateTime fechaActual = DateTime.Now;
            GENTEMAR_AUTENTICACION gentemarAutenticacion = new GENTEMAR_AUTENTICACION
            {
                IdUsuario = loginId,
                Token = EncryptDecryptService.GenerateEncrypt(token),
                Ip = ipAddress,
                FechaHoraInicioSesion = fechaActual,
                FechaHoraFinSesion = null,
                Estado = (int)EstadoTokenEnum.Activo,
                FechaExpiracion = fechaActual.AddMinutes(_expireTimeToken),
                FechaModificacion = fechaActual,
                Comentario = "New Token",
                IdAplicacion = (int)TipoAplicacionEnum.GenteDeMar
            };
            using (var repo = new AutenticacionRepository())
            {
                await repo.Create(gentemarAutenticacion);
                return Responses.SetCreatedResponse(gentemarAutenticacion);
            }
        }
        public GENTEMAR_AUTENTICACION GetToken(string token)
        {
            using (var repo = new AutenticacionRepository())
            {
                string tokenEncrypt = EncryptDecryptService.GenerateEncrypt(token);
                var query = repo.GetWithCondition(x => x.Token == tokenEncrypt && x.IdAplicacion == (int)TipoAplicacionEnum.GenteDeMar);
                return query;
            }
        }

        public async Task<Respuesta> DesactivarToken(string token, string comentario)
        {
            using (var repo = new AutenticacionRepository())
            {
                var loginId = ClaimsHelper.GetLoginId();
                string tokenEncrypt = EncryptDecryptService.GenerateEncrypt(token);
                var tokenActivo = await repo.GetWithConditionAsync(x => x.Token.Equals(tokenEncrypt)
                                                                    && x.IdUsuario == loginId
                                                                    && x.IdAplicacion == (int)TipoAplicacionEnum.GenteDeMar);
                if (tokenActivo == null)
                    return Responses.SetOkResponse("The token does not exist");

                if (tokenActivo.Estado == (int)EstadoTokenEnum.Inactivo)
                    return Responses.SetOkResponse("It was already inactive");

                tokenActivo.Estado = (int)EstadoTokenEnum.Inactivo;
                tokenActivo.Comentario = comentario;
                tokenActivo.FechaHoraFinSesion = DateTime.Now;
                tokenActivo.FechaModificacion = DateTime.Now;
                await repo.Update(tokenActivo);
                return Responses.SetOkResponse("Se cierra la sesión satisfactoriamente.");
            }
        }

        public async Task RemoverTokensPorUsuarioId(int loginId)
        {
            using (var repo = new AutenticacionRepository())
            {
                await repo.RemoverTokensPorUsuarioId(loginId);
            }
        }

        public async Task<Respuesta> LogOut(string token)
        {
            using (var repo = new AutenticacionRepository())
            {
                return await DesactivarToken(token, AutenticacionConfig.COMENTARIO_FINALIZACION_SESION);
            }
        }
    }
}
