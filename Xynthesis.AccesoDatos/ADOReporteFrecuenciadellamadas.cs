using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Modelo;
using System.Data.Entity;
using Xynthesis.Utilidades;
using Xynthesis.Utilidades.Mensajes;

namespace Xynthesis.AccesoDatos
{
    public class ADOReporteFrecuenciadellamadas
    {

        xynthesisEntities xyt = new xynthesisEntities();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();

        public List<xyp_SelFrequentExtensionNumber_Result> ObtenerFrecuenciaDeLlamadas(string fecini, string fecfin)
        {
            try
            {
                return xyt.xyp_SelFrequentExtensionNumber(fecini, fecfin).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
