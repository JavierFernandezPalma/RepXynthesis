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
    public class ADCliente
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();

        //Metodo 
        public IQueryable<xy_cliente> ObtenerListaUsuarios()
        {
            return from s in xyt.xy_cliente select s;
        }

        public List<xy_cliente> OrdenFiltro(string sortOrder, string searchString)
        {
            List<xy_cliente> res;
            try
            {
                int totalRegis = (from x in xyt.xy_cliente select x).Count();
                if (!String.IsNullOrEmpty(searchString))
                {
                    res = xyt.xy_cliente.Where(s => s.nombreCliente.ToUpper().Contains(searchString.ToUpper())).ToList();
                }
                else
                {
                    res = xyt.xy_cliente.ToList();
                }
                if (sortOrder != null)
                {
                    if (sortOrder.Equals("name_desc"))
                        return res.OrderByDescending(s => s.nombreCliente).ToList();
                    else
                        return res.OrderBy(s => s.nombreCliente).ToList();
                }
                else
                    return res.OrderBy(s => s.nombreCliente).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public xy_cliente buscarClientexId(int id)
        {
            return xyt.xy_cliente.Find(id); 
        }

        public Xynthesis.Utilidades.Mensaje nuevoCliente(xy_cliente nuevo)
        {
            msg = new Mensaje();
            try
            {

                if ((from s in xyt.xy_cliente where s.nombreCliente == nuevo.nombreCliente select s).Count() <= 0)
                {
                    xyt.xy_cliente.Add(nuevo);
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
                msg.codigo = 0;
                msg.mensaje = MensajesXynthesis.ErrDesconocido;
                log.EscribaLog("CLIENTE", "Action:nuevoCliente " + ex.Message, "");
                return msg;
            }
        }

        public Xynthesis.Utilidades.Mensaje guardarEdicion(xy_cliente xy_cliente)
        {
            msg = new Mensaje();
            try
            {
                if ((from s in xyt.xy_cliente where s.nombreCliente == xy_cliente.nombreCliente && s.Idcliente!= xy_cliente.Idcliente select s).Count() <= 0)
                {
                    xyt.Entry(xy_cliente).State = System.Data.EntityState.Modified;
                    xyt.SaveChanges();
                    msg.codigo = 1;
                    msg.mensaje = MensajesXynthesis.Actualiza;
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
                msg.codigo = 0;
                msg.mensaje = MensajesXynthesis.ErrDesconocido;
                log.EscribaLog("CLIENTE", "Action:guardarEdicion " + ex.Message, "");
                return msg;
            }
        }

        public Xynthesis.Utilidades.Mensaje EliminarCliente(int id)
        {
            msg = new Mensaje();
            try
            {
                if ((from s in xyt.xy_numbers where s.Idcliente == id select s).Count() <= 0)
                {
                    xy_cliente cliente = buscarClientexId(id);
                    xyt.xy_cliente.Remove(cliente);
                    xyt.SaveChanges();
                    msg.codigo = 1;
                    msg.mensaje = MensajesXynthesis.Elimina;
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
                msg.codigo = 0;
                msg.mensaje = MensajesXynthesis.ErrDesconocido;
                log.EscribaLog("CLIENTE", "Action:EliminarCliente " + ex.Message, "");
                return msg;
            }
        }



    }
}
