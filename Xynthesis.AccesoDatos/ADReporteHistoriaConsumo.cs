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
    public class ADReporteHistoriaConsumo
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();
        public List<xyp_SelConsumeByHistory_Result> ObtenerHistoriaConsumos(string fecini, string fecfin, string area)
        {
            try
            {
                return xyt.xyp_SelConsumeByHistory(fecini, fecfin, area).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
