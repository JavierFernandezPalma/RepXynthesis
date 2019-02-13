using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Utilidades.Mensajes;

namespace Xynthesis.Utilidades
{
    public class Mensaje
    {
        public int codigo { get; set; }
        public string mensaje { get; set; }
    }

    public class FuncionesComunes
    {
        public static ResourceManager recurso = new ResourceManager("Xynthesis.Utilidades.Mensajes.MensajesXynthesis", typeof(MensajesXynthesis).Assembly);

        public static string ObtenerMensaje(string MensajeID)
        {
            return recurso.GetString(MensajeID);
        }

        public static string ObtenerValorConfiguracion(string clave)
        {
            var cla = ConfigurationManager.AppSettings[clave] as string ?? string.Empty;
            return cla;
        }

        public static void EnviarCorreo(List<string> correos, string asunto, string cuerpoMensaje, List<Attachment> files)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(ObtenerValorConfiguracion(ParametrosEmail.MailSenderHost));
                foreach (var fl in files)
                {
                    mail.Attachments.Add(fl);
                }
                mail.From = new MailAddress(ObtenerValorConfiguracion(ParametrosEmail.MailSenderFrom));
                foreach (var email in correos)
                {
                    mail.To.Add(new MailAddress(email));
                }
                mail.Subject = asunto;
                mail.IsBodyHtml = true;
                mail.Body = cuerpoMensaje;

                SmtpServer.Port = Convert.ToInt32(ObtenerValorConfiguracion(ParametrosEmail.MailSenderPor));

                if (ObtenerValorConfiguracion(ParametrosEmail.MailSenderEsRequeridoCredential).Equals(Constantes.SI))
                {
                    string credencial = ObtenerValorConfiguracion(ParametrosEmail.MailSenderCredential);

                    var valoresCredenciales = credencial.Split(';').ToArray();
                    //SmtpServer.UseDefaultCredentials = false;
                    SmtpServer.UseDefaultCredentials = true;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(valoresCredenciales[0].ToString(), valoresCredenciales[1].ToString());
                }

                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
