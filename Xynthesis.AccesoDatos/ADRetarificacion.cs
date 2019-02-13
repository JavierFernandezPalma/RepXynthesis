using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Modelo;
using System.Data.Entity;
using Xynthesis.Utilidades;
using Xynthesis.Utilidades.Mensajes;

namespace Xynthesis.AccesoDatos
{
    public class ADRetarificacion
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();

        public Xynthesis.Utilidades.Mensaje retarificar(string FechaInicial, string FechaFinal)
        {

            msg = new Mensaje();
            try
            {
                if (!FechaInicial.Equals("") && !FechaFinal.Equals(""))
                {
                    if (Convert.ToDateTime(FechaInicial) <= Convert.ToDateTime(FechaFinal))
                    {
                        xyt.xyp_RECalCostTicket(FechaInicial, FechaFinal);
                        msg.codigo = 1;
                        msg.mensaje =MensajesXynthesis.proRetar;
                    }
                    else
                    {
                        msg.codigo = 0;
                        msg.mensaje = MensajesXynthesis.valFechas;
                    }
                }
                else
                {
                    msg.codigo = 0;
                    msg.mensaje = MensajesXynthesis.sinfechas;
                }
                return msg;
            }
            catch (Exception ex)
            {
                msg.codigo = 0;
                msg.mensaje = MensajesXynthesis.ErrDesconocido;
                log.EscribaLog("AREAS", "Action:nuevaArea " + ex.Message, "");
                return msg;
            }


            ////xyt.xyp_RECalCostTicket(FechaInicial, FechaFinal);

            ////try
            ////{
            ////    xyt.xyp_RECalCostTicket(FechaInicial, FechaFinal);
            ////}
            ////catch (Exception ex)
            ////{
            ////    throw ex;
            ////}

        }
    }
}
