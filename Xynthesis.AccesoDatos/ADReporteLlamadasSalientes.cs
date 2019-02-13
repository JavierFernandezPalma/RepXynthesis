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
   public class ADReporteLlamadasSalientes
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
        LogXynthesis log = new LogXynthesis();

        public List<xyp_NumberAmountsByOutSubscriber_Result> ObtenerListaLlamadasSalientes(string FechaInicial, string FechaFinal, string usuario, string llamada)
        {
            try
            {
                return xyt.xyp_NumberAmountsByOutSubscriber(FechaInicial, FechaFinal, usuario, llamada).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
