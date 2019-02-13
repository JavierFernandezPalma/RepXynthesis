using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Modelo;
using Xynthesis.Utilidades;


namespace Xynthesis.AccesoDatos
{
    public class ADReporteGraficoLlamadasEntrantesSalientes
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
        LogXynthesis log = new LogXynthesis();


        public List<xyp_RepReceiveCallsLlamEntranSalien_Result> ObtenerListaLlamadasEntrantesSalientes(string FechaInicial, string FechaFinal, string usuario, string area)
        {
            
            try
            {
                return xyt.xyp_RepReceiveCallsLlamEntranSalien(FechaInicial, FechaFinal, usuario, area).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
