using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Modelo;
using Xynthesis.Utilidades;

namespace Xynthesis.AccesoDatos
{
    public class ADBase
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
        LogXynthesis log = new LogXynthesis();

        public List<xyp_SelReports_Result> ObtenerProgramacionReportes(Int32 IdConfiguracion)
        {
            try
            {
                return xyt.xyp_SelReports(IdConfiguracion).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
