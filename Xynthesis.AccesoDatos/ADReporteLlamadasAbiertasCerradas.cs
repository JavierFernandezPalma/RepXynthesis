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
    public class ADReporteLlamadasAbiertasCerradas
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
        LogXynthesis log = new LogXynthesis();

        public List<xyp_RepCallOpenAndClosed_Result> ObtenerListaLlamadasAbiertasCerradas(string FechaInicial, string FechaFinal, string hora, string usuario, string llamada)
        {
            try
            {
                return xyt.xyp_RepCallOpenAndClosed(FechaInicial, FechaFinal, hora, usuario, llamada).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
