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
   public class ADReporteInicioFinActividades
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
        LogXynthesis log = new LogXynthesis();

        public List<xyp_SelActivityFirstAndLast_Result> ObtenerListaInicioFin(string FechaInicial, string FechaFinal, string usuario)
        {           

            try
            {
                return xyt.xyp_SelActivityFirstAndLast(FechaInicial, FechaFinal, usuario).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}
