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
    public class ADRepPromedioLlamadasHora
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
        LogXynthesis log = new LogXynthesis();
        public List<xyp_RepPromedioLlamadasHora_Result> ObtenerPromedioLlamadasHora(string FechaInicial, string FechaFinal, string usuario, string area, string rango)
        {
            try
            {
                return xyt.xyp_RepPromedioLlamadasHora(FechaInicial, FechaFinal, usuario, area, rango).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
