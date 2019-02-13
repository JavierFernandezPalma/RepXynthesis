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
    public class ADLlamadaCampeonaDuracion
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();
        public List<xyp_SelDetailChampCallDuration_Result> ObtenerLlamadaCampeonaDuracion(string fecini, string fecfin, string area, string llamada)
        {
            try
            {
                return xyt.xyp_SelDetailChampCallDuration(fecini, fecfin, area, llamada).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
