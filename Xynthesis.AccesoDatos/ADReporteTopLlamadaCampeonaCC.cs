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
    public class ADReporteTopLlamadaCampeonaCC
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();


        public List<xyp_SelDetailChampCallCost_Result> ObtenerTopLlamadaCampeonaCC(string fecini, string fecfin, string are, string llamada, string origen)
        {
            try
            {
                return xyt.xyp_SelDetailChampCallCost(fecini, fecfin, are, llamada, origen).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
