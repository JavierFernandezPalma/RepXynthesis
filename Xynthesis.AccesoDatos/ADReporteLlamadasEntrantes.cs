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
    public class ADReporteLlamadasEntrantes
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
        LogXynthesis log = new LogXynthesis();

        public List<xyp_NumberAmountsByInSubscriber_Result> ObtenerListaLlamadasEntrantes(string FechaInicial, string FechaFinal, string usuario, string llamadaEntrante, string extension)
        {
            try
            {
                return xyt.xyp_NumberAmountsByInSubscriber(FechaInicial, FechaFinal, usuario, llamadaEntrante, extension).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
