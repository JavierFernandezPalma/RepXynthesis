using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Modelo;
using Xynthesis.Utilidades;

namespace Xynthesis.AccesoDatos
{
   public class ADReporteGraficoTiempoPromedioAntesDeContestar
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
        LogXynthesis log = new LogXynthesis();


        public List<xyp_ReceiveCallsTiempoPromedio_repo_Result> ObtenerListaTiempoPromedioAntesContestar(string FechaInicial, string FechaFinal)
        {
            try
            {
                return xyt.xyp_ReceiveCallsTiempoPromedio_repo(FechaInicial, FechaFinal).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<xyp_ReceiveCallsTiempoPromedio_Result> ObtenerListaTiempoPromedioAntesContestarResumido(string FechaInicial, string FechaFinal, string usuario, string area)
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
