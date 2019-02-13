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
   public class ADReporteTiempoDedicado
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
        LogXynthesis log = new LogXynthesis();

        public List<xyp_CallAmountByContraparte_Result> ObtenerListaTiempoDedicado(string FechaInicial, string FechaFinal, string user)
        {
            try
            {
                return xyt.xyp_CallAmountByContraparte(FechaInicial, FechaFinal, user).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
