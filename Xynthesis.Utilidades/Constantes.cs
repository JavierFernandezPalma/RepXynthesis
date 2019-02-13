using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynthesis.Utilidades
{
    public class Constantes
    {
        //public static Int32 MAXPAGEROWS = Int32.Parse(ConfigurationSettings.AppSettings["MAXPAGEROWS"]);
        //public static Int32 MAXVISIBLE = Int32.Parse(ConfigurationSettings.AppSettings["MAXVISIBLE"]);
        public string MaxRegGrilla = System.Configuration.ConfigurationSettings.AppSettings["MaxRegGrilla"];

        public const string SI = "SI";
        public const string NO = "NO";
        public const string RutaRpt = "RutaRpt";
        public const string RutaLogServiceRpt = "RutaLogServiceRpt";
    }

    public class tipoExportacion
    {
        public const string pdf = "pdf";
        public const string xls = "xls";
        public const string doc = "doc";
    }

    public class ParametrosEmail
    {
        public const string MailSenderHost = "MailSender.Host";
        public const string MailSenderPor = "MailSender.Port";
        public const string MailSenderFrom = "MailSender.From";
        public const string MailSenderCredential = "MailSender.Credential";
        public const string MailSenderEsRequeridoCredential = "MailSender.EsRequeridoCredential";

    }

    public class ReportesXyn
    {
        public const string RepPromedioLlamadasHora = "RepPromedioLlamadasHora";
    }

  

    public class Frecuencia
    {
        public const string Mensual = "Mensual";
    }
}
