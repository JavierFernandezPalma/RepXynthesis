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
    public class ADRepTarificacionEntraSalieTrans
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
        LogXynthesis log = new LogXynthesis();
        public List<xyp_RepTarificacionEntraSalieTrans_Result> ObtenerTarificacion(string FechaInicial, string FechaFinal, string usuario, string area, string sucursal)
        {
            try
            {
                //return xyt.xyp_CallTypesAmountReport(FechaInicial, FechaFinal).ToList();
                return xyt.xyp_RepTarificacionEntraSalieTrans(FechaInicial, FechaFinal, usuario, area, sucursal).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
