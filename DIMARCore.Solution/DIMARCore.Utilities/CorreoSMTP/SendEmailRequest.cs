using System;

namespace DIMARCore.Utilities.CorreoSMTP
{
    public class SendEmailRequest
    {
        public String[] CorreosAEnviar { get; set; }
        public string Asunto { get; set; }
        public string CuerpoDelMensaje { get; set; }
        public string Footer { get; set; }
    }
}
