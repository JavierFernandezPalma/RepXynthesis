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
    public class ADReporteNumeroMasMarcado
    {

        xynthesisEntities xyt = new xynthesisEntities();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();

        public List<xyp_SelDialedNumber_Result> ObtenerNumeroMasMarcado(string fecini, string fecfin, string origen, string destino)
        {
            try
            {
                return xyt.xyp_SelDialedNumber(fecini, fecfin, origen, destino).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
