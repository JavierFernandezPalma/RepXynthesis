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
    public  class ADReporteConsumosPorCentroCost
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();

        public List<xyp_SelConsumeByCostCenter_Result> ObtenerConsumosPorcentroCostos(string fecini, string fecfin, string area, string cobertura)
        {
            try
            {
                return xyt.xyp_SelConsumeByCostCenter(fecini, fecfin, area, cobertura).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
