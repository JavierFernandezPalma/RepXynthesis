using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Xynthesis.Modelo;
using Xynthesis.Utilidades;

namespace Xynthesis.AccesoDatos
{
   public class ADReporteLlamadasEntrantesSalientes
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
        LogXynthesis log = new LogXynthesis();

        public List<xyp_RepReceiveCallsLlamEntranSalien_Result> ObtenerListaLlamadasEntrantesSalientes(string FechaInicial, string FechaFinal, string user, string area)
        {
            try
            {
                return xyt.xyp_RepReceiveCallsLlamEntranSalien(FechaInicial, FechaFinal, user, area).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
