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
    public class ADAreas
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();
        public List<xyp_SelAllCostCentre_Result> ObtenerListaAreas()
        {
            try
            {
                int totalRegis = (from x in xyt.xy_costcenters select x).Count();
                return xyt.xyp_SelAllCostCentre(1, totalRegis).ToList();
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        public Xynthesis.Utilidades.Mensaje nuevaArea(xy_costcenters nuevo)
        {
            msg = new Mensaje();
            try
            {
                if ((from s in xyt.xy_costcenters where s.Nom_CostCenter == nuevo.Nom_CostCenter select s).Count() <= 0)
                {
                    xyt.xy_costcenters.Add(nuevo);
                    xyt.SaveChanges();
                    msg.codigo = 1;
                    msg.mensaje =  MensajesXynthesis.Nuevo ;
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
                log.EscribaLog("AREAS", "Action:nuevaArea " + ex.Message, "");
                return msg;
            }
        }


        public List<xyp_SelAllCostCentre_Result> OrdenFiltro(string sortOrder, string searchString)
        {
            List<xyp_SelAllCostCentre_Result> res ;
            try
            {
                int totalRegis = (from x in xyt.xy_costcenters select x).Count();
                if (!String.IsNullOrEmpty(searchString))
                {
                    res = xyt.xyp_SelAllCostCentre(1, totalRegis).Where(s => s.Nom_CostCenter.ToUpper().Contains(searchString.ToUpper())                                     
                                          ).ToList();
                }
                else
                {
                    res = xyt.xyp_SelAllCostCentre(1, totalRegis).ToList();
                }
                if (sortOrder != null)
                {
                    if (sortOrder.Equals("name_desc"))
                        return res.OrderByDescending(s => s.Nom_CostCenter).ToList();
                    else
                        return res.OrderBy(s => s.Nom_CostCenter).ToList();
                }
                else
                return res.OrderBy(s => s.Nom_CostCenter).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Xynthesis.Utilidades.Mensaje guardarEdicion(xy_costcenters xy_costcenters_, string Nom_CostCenter)
        {
            msg = new Mensaje();
            try
            {
                xy_costcenters edit = new xy_costcenters();
                edit = xyt.xy_costcenters.Find(xy_costcenters_.Ide_CostCenter);
                if ((from s in xyt.xy_costcenters where s.Nom_CostCenter == xy_costcenters_.Nom_CostCenter select s).Count() <= 0)
                {
                    edit.Ide_CostCenter = xy_costcenters_.Ide_CostCenter;
                    edit.Nom_CostCenter = xy_costcenters_.Nom_CostCenter;
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
                log.EscribaLog("AREAS", "Action:guardarEdicion " + ex.Message, "");
                return msg;
            }
        }

        public xy_costcenters buscarAreaxId(long? id)
        {
            try
            {
                return  xyt.xy_costcenters.Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Xynthesis.Utilidades.Mensaje EliminarArea(long? id)
        {
            msg = new Mensaje();
            try
            {
                xy_costcenters Area = buscarAreaxId(id);
                if ((from s in xyt.xy_subscriber where s.Ide_CostCenter == Area.Ide_CostCenter select s).Count() <= 0)
                {
                    xyt.xy_costcenters.Remove(Area);
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
            catch (Exception ex)
            {
                msg.codigo = 0;
                msg.mensaje = MensajesXynthesis.ErrDesconocido;
                log.EscribaLog("AREAS", "Action:EliminarArea " + ex.Message, "");
                return msg;
            }
        }

    }
}
