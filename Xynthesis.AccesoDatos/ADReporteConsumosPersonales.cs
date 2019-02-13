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
    public class ADReporteConsumosPersonales
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();
        public List<xyp_SelConsumeByExtensionAndUser_Result> ObtenerConsumosPersonales(string fecini, string fecfin, string usuario, string area, string extension, string cobertura, string destino)
        {
            //XYNP_SelConsumeByExtensionAndUser
            try
            {
                return xyt.xyp_SelConsumeByExtensionAndUser(fecini, fecfin, usuario, area, extension, cobertura, destino).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
