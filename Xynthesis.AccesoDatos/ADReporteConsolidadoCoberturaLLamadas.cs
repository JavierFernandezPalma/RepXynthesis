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
    public class ADReporteConsolidadoCoberturaLLamadas
    {

        xynthesisEntities xyt = new xynthesisEntities();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();
        public List<xyp_RepConsolidadoCoberturaLLamadas_Result> ObtenerConsolidadoCoberturaLlamadas(string fecini, string fecfin, string usuario, string area)
        {
            try
            {
                return xyt.xyp_RepConsolidadoCoberturaLLamadas(fecini, fecfin, usuario, area).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
