using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Modelo;
using System.Data.Entity;
using Xynthesis.Utilidades;

namespace Xynthesis.AccesoDatos
{
    public class ADReporteCoberturaLlamadas
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();
        public List<xyp_ReceiveCalls_Result> ObtenerCoberturaLlamadas(string fecini, string fecfin, string user, string area)
        {
            try
            {
                return xyt.xyp_ReceiveCalls(fecini, fecfin, user, area).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
