using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Modelo;
using Xynthesis.Utilidades;

namespace Xynthesis.AccesoDatos
{
    public class ADReporteSabanaCalls
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
        LogXynthesis log = new LogXynthesis();

        public List<xyp_RepSabanaCalls_Result> ObtenerListaSabanaCalls(string fecini, string fecfin)
        {
            try
            {
                return xyt.xyp_RepSabanaCalls(fecini, fecfin).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
