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
    public class ADReporteClaseDeLlamadas
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
        LogXynthesis log = new LogXynthesis();

        public List<xyp_CallTypesAmountReport_Result> ObtenerListaClaseDeLlamadas(string FechaInicial, string FechaFinal)
        {
            try
            {
                return xyt.xyp_CallTypesAmountReport(FechaInicial, FechaFinal).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
