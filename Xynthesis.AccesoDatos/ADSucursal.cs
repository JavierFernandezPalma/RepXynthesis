using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Modelo;
using Xynthesis.Utilidades;
using Xynthesis.Utilidades.Mensajes;

namespace Xynthesis.AccesoDatos
{
    
    public class ADSucursal
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
        LogXynthesis log = new LogXynthesis();

        
        public List<xyp_SelSucursales_Result> listasucursales() 
        {
            try
            {
                List<xyp_SelSucursales_Result> lista = new List<xyp_SelSucursales_Result>();
                lista = xyt.xyp_SelSucursales().ToList();
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<xyp_SelSucursales_Result> OrdenFiltro(string sortOrder, string searchString)
        {
            List<xyp_SelSucursales_Result> res;
            try
            {
                
                int totalRegis = (from x in xyt.xy_sucursal select x).Count();
                if (!String.IsNullOrEmpty(searchString))
                {
                    res = (from s in xyt.xyp_SelSucursales() where s.NombreSucursal.ToUpper().Contains(searchString.ToUpper()) select s).ToList();

                }
                else
                {
                    res = (from s in xyt.xyp_SelSucursales() select s).ToList();
                }
                if (sortOrder != null)
                {
                    if (sortOrder.Equals("name_desc"))
                        return res.OrderByDescending(s => s.NombreSucursal).ToList();
                    else
                        return res.OrderBy(s => s.NombreSucursal).ToList();
                }
                else
                    return res.OrderBy(s => s.NombreSucursal).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Xynthesis.Utilidades.Mensaje NuevaSucursal(xy_sucursal nuevo)
        {
            msg = new Mensaje();
            try
            {
                if ((from nxs in xyt.xy_sucursal where nxs.NombreSucursal.Equals(nuevo.NombreSucursal)  select nxs).Count()<=0)
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

        public Xynthesis.Utilidades.Mensaje EditarSucursal(xy_sucursal editSuc, string IdSucursal)
        {
            msg = new Mensaje();
            try
            {
                Int64 idSuc = Convert.ToInt32(IdSucursal);

                //bool exists = (from nomb in xyt.xy_sucursal
                //               where nomb.NombreSucursal == editSuc.NombreSucursal
                //               select nomb).Any();
                int exists_ = (from nomb in xyt.xy_sucursal
                               where nomb.NombreSucursal == editSuc.NombreSucursal && nomb.IdSucursal != editSuc.IdSucursal
                               select nomb).Count();

                if (exists_ > 0)
                {
                    msg.codigo = 0;
                    msg.mensaje = MensajesXynthesis.existeRegi;
                    return msg;
                }

                if ((from nxs in xyt.xy_sucursal where nxs.NombreSucursal.Equals(editSuc.NombreSucursal) && nxs.IdSucursal != idSuc select nxs).Count() <= 0)
                {

                    int res = xyt.xyp_UpdSucursal(editSuc.IdSucursal, editSuc.NombreSucursal, editSuc.Ide_Geography);
                    
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
                log.EscribaLog("SUCURSAL", "Action:EditarSucursal " + ex.Message, "");
                msg.codigo = 0;
                msg.mensaje = MensajesXynthesis.ErrDesconocido;
                return msg;
            }

        }

        public void EliminarSucursal(int id)
        {
            try
            {
                var sucu = new xy_sucursal { IdSucursal = id };
                xyt.xy_sucursal.Attach(sucu);
                xyt.xy_sucursal.Remove(sucu);
                xyt.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }      
            
        }

    }


}
