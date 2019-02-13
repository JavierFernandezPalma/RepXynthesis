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
  public class ADReporteTiempoPromedioAntesContestar
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
        LogXynthesis log = new LogXynthesis();

        public List<xyp_ReceiveCallsTiempoPromedio_Result> ObtenerListaTiempoPromedioAntesContestar(string FechaInicial, string FechaFinal, string usuario, string area)
        {
            try
            {
                return xyt.xyp_ReceiveCallsTiempoPromedio(FechaInicial, FechaFinal, usuario, area).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
