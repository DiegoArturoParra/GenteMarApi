using System;

namespace DIMARCore.Utilities.CorreoSMTP
{
    public class SendEmailRequest
    {
        public SendEmailRequest()
        {

        }
        public SendEmailRequest(string[] correosDestino, string mensaje, string body, string title, string footer)
        {
            this.CorreosDestino = correosDestino;
            this.Asunto = mensaje;
            this.CuerpoDelMensaje = body;
            this.Titulo = title;
            this.Footer = footer;
        }

        public string[] CorreosDestino { get; set; }
        public string Asunto { get; set; }
        public string CuerpoDelMensaje { get; set; }
        public string CC { get; set; }
        public string Titulo { get; set; }
        public string BCC { get; set; }
        public string Footer { get; set; } = "por favor comuniquese con el administrador.";
    }
}
