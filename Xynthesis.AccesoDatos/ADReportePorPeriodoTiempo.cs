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
   public class ADReportePorPeriodoTiempo
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
        LogXynthesis log = new LogXynthesis();

        public List<xyp_SelCallAmountsBySubscriber_Result> ObtenerListaPorPeriodoTiempo(string FechaInicial, string FechaFinal, string user)
        {
            try
            {
                return xyt.xyp_SelCallAmountsBySubscriber(FechaInicial, FechaFinal, user).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
