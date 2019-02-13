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
    public class ADOperador
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();
        public IQueryable<xy_operators> ObtenerListaOperadores()
        {
            return from s in xyt.xy_operators select s;
        }

        public List<xy_operators> OrdenFiltro(string sortOrder, string searchString)
        {
            List<xy_operators> res;
            try
            {
                int totalRegis = (from x in xyt.xy_operators select x).Count();
                if (!String.IsNullOrEmpty(searchString))
                {
                    //res = xyt.xy_operators.Where(s => s.Nom_Operator.ToUpper().Contains(searchString.ToUpper())).ToList();
                    res = (from s in xyt.xy_operators where s.Nom_Operator.ToUpper().Contains(searchString.ToUpper()) || s.Cod_Operator.ToUpper().Contains(searchString.ToUpper()) select s).ToList();
                }
                else
                {
                    //res = xyt.xy_operators.ToList();
                    res = (from s in xyt.xy_operators select s).ToList();
                }
                if (sortOrder != null)
                {
                    if (sortOrder.Equals("name_desc"))
                        return res.OrderByDescending(s => s.Nom_Operator).ToList();
                    else
                        return res.OrderBy(s => s.Nom_Operator).ToList();
                }
                else
                    return res.OrderBy(s => s.Nom_Operator).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public xy_operators buscarOperadorxId(int id)
        {
            return xyt.xy_operators.Find(id);
        }

        public Xynthesis.Utilidades.Mensaje nuevoOperador(xy_operators nuevo)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            try
            {
                if ((from s in xyt.xy_operators where s.Nom_Operator == nuevo.Nom_Operator select s).Count() <= 0)
                {
                        xyt.xy_operators.Add(nuevo);
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
                log.EscribaLog("OPERADORES", "Action:nuevoOperador " + ex.Message, "");
                return msg;
            }
        }

        public Xynthesis.Utilidades.Mensaje guardarEdicion(xy_operators xy_operators_)
        {
            msg = new Mensaje();
            try
            {
                if ((from s in xyt.xy_operators where s.Nom_Operator == xy_operators_.Nom_Operator && s.Ide_Operator != xy_operators_.Ide_Operator select s).Count() <= 0)
                {
                    xyt.Entry(xy_operators_).State = System.Data.EntityState.Modified;
                    msg.codigo = 1;
                    msg.mensaje = MensajesXynthesis.Actualiza;
                    xyt.SaveChanges();
                    
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
                log.EscribaLog("OPERADOR", "Action:guardarEdicion " + ex.Message, "");
                return msg;
            }
        }

        public Xynthesis.Utilidades.Mensaje EliminarOperador(int id)
        {
            msg = new Mensaje();
            try
            {
                if ((from s in xyt.xy_operador_tarifa_inter where s.IdOperador == id select s).Count() <= 0)
                {
                    if ((from s in xyt.xy_rates where s.Ide_Operator == id select s).Count() <= 0)
                    {
                        xy_operators operador = buscarOperadorxId(id);
                        xyt.xy_operators.Remove(operador);
                        xyt.SaveChanges();
                        msg.codigo = 1;
                        msg.mensaje = MensajesXynthesis.Elimina;
                        return msg;
                    }
                    else
                    {
                        msg.codigo = 0;
                        msg.mensaje = MensajesXynthesis.NoProcesa;
                        return msg;
                    }
                }
                else
                {
                    msg.codigo = 0;
                    msg.mensaje = MensajesXynthesis.ErrDesconocido;
                    return msg;
                }

            }
            catch (Exception ex)
            {
                msg.codigo = 0;
                msg.mensaje = MensajesXynthesis.ErrDesconocido;
                log.EscribaLog("OPERADOR", "Action:EliminarOperador " + ex.Message, "");
                return msg;
            }
        }

    }
}
