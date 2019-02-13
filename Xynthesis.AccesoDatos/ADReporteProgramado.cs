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
  public class ADReporteProgramado
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
        LogXynthesis log = new LogXynthesis();

        public IQueryable<xy_configuracionrptprogramado> ObtenerListaRepProgramado()
        {
            try
            {
                return from s in xyt.xy_configuracionrptprogramado select s;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<xy_configuracionrptprogramado> OrdenFiltro(string sortOrder, string searchString)
        {
            List<xy_configuracionrptprogramado> res;
            try
            {
                int totalRegis = (from x in xyt.xy_configuracionrptprogramado select x).Count();
                if (!String.IsNullOrEmpty(searchString))
                {
                    res = (from s in xyt.xy_configuracionrptprogramado where s.Nombre.ToUpper().Contains(searchString.ToUpper()) select s).ToList();

                }
                else
                {
                    res = (from s in xyt.xy_configuracionrptprogramado select s).ToList();
                }
                if (sortOrder != null)
                {
                    if (sortOrder.Equals("name_desc"))
                        return res.OrderByDescending(s => s.Nombre).ToList();
                    else
                        return res.OrderBy(s => s.Nombre).ToList();
                }
                else
                    return res.OrderBy(s => s.Nombre).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public Xynthesis.Utilidades.Mensaje NuevoRepProgramado(xy_sucursal nuevo)
        {            
            
            msg = new Mensaje();
            try
            {
                if ((from nxs in xyt.xy_sucursal select nxs).Count() <= 0)
                {
                  

                    xyt.xyp_InsSucursal(nuevo.NombreSucursal, nuevo.Ide_Geography);
                    xyt.SaveChanges();
                    msg.codigo = 1;
                    msg.mensaje = MensajesXynthesis.Nuevo;
                }
                else
                {
                    msg.codigo = 0;
                    msg.mensaje = MensajesXynthesis.NoProcesa;
                }
                return msg;
            }
            catch (Exception ex)
            {
                log.EscribaLog("SUCURSAL", "Action:NuevaSucursal " + ex.Message, "");
                msg.codigo = 0;
                msg.mensaje = MensajesXynthesis.ErrDesconocido;
                return msg;
            }
        }

            
        public void EditarRepProgram(xy_configuracionrptprogramado update)
        {
            try
            {
                xy_configuracionrptprogramado updaterepProgra = xyt.xy_configuracionrptprogramado.Find(update.ConfiguracionId);
                updaterepProgra.Nombre = update.Nombre;
                updaterepProgra.HoraEjecucion = update.HoraEjecucion;
                updaterepProgra.FormatoArchivo = update.FormatoArchivo;
                updaterepProgra.EmailFrom = update.EmailFrom;
                updaterepProgra.Asunto = update.Asunto;
                updaterepProgra.Mensaje = update.Mensaje;

                xyt.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void EliminarRepProgram(int id)
        {
            try
            {
                xyt.xyp_DelRepProgra(id);
                //xyt.xy_configuracionrptprogramado.Remove(eli);
                xyt.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
