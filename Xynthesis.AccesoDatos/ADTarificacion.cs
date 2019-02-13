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
    public class ADTarificacion
    {
        xynthesisEntities xyt = new xynthesisEntities();
        ADOperador operador = new ADOperador();
        Utilidades.LogXynthesis log = new LogXynthesis();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();

        public List<xyp_SelOperators_Result> ObtenerListaUsuarios()
        {
            return xyt.xyp_SelOperators(null).ToList();
        }

        public List<xyp_SelOperators_Result> OrdenFiltro(string sortOrder, string searchString)
        {
            List<xyp_SelOperators_Result> res;
            try
            {
                int totalRegis = (from x in xyt.xy_operators select x).Count();
                if (!String.IsNullOrEmpty(searchString))
                {
                    res = xyt.xyp_SelOperators(null).Where(s => s.Nom_Operator.ToUpper().Contains(searchString.ToUpper())
                                      || s.Nom_Coverage.ToUpper().Contains(searchString.ToUpper()) ).ToList();
                }
                else
                {
                    res = xyt.xyp_SelOperators(null).ToList();
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

        public List<xyp_SelOperators_Result> editarNumero(string id, int? page, string Nom_Country)
        {
            msg = new Mensaje();
            try
            {
                int totalRegis = (from x in xyt.xy_numbers select x).Count();
                var res = xyt.xyp_SelOperators(null).Where(s => s.Ide_Operator == id.ToString()).ToList();
                return res;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Xynthesis.Utilidades.Mensaje guardarEdicion(int id, xy_operators res, string DdUnit, string DdlCobertura)
        {
            msg = new Mensaje();
            try
            {
                 if (res.NumberDigits == null)
                    xyt.xyp_UpdOperator(id, res.Cod_Operator, res.Nom_Operator, Convert.ToInt32(DdlCobertura), res.vlr_Cost, DdUnit,0);
                else
                    xyt.xyp_UpdOperator(id, res.Cod_Operator, res.Nom_Operator, Convert.ToInt32(DdlCobertura), res.vlr_Cost, DdUnit, Convert.ToInt32(res.NumberDigits.ToString()));
                msg.codigo = 1;
                msg.mensaje =MensajesXynthesis.Actualiza;
                return msg;
            }
            catch (Exception ex)
            {
                msg.codigo = 0;
                msg.mensaje = MensajesXynthesis.ErrDesconocido;
                log.EscribaLog("TARIFICACION", "Action:EliminarOperador " + ex.Message, "");
                return msg;
            }
        }

        public Xynthesis.Utilidades.Mensaje EliminarOperador(int id)
        {
            msg = new Mensaje();
            try
            {
                var vali = (from s in xyt.xy_rates where s.Ide_Operator == id select s).ToList();
                if (vali.Count == 0)
                {
                    xy_operators oper = xyt.xy_operators.Find(id);
                    xyt.xy_operators.Remove(oper);
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
                log.EscribaLog("TARIFICACION", "Action:EliminarOperador " + ex.Message, "");
                return msg;
            }
        }


        public xy_operators buscarTarificacionId(int id)
        {
            return xyt.xy_operators.Find(id);
        }

        public xy_rates buscarExtensionesId(int id)
        {
            return xyt.xy_rates.Find(id);
        }

        public List<xy_coverage> obtenerListaCoberturas()
        {
            return xyt.xy_coverage.ToList();
        }

        public xy_coverage obtenerCoberturaXId(int id)
        {
            return xyt.xy_coverage.Find(id);
        }

    }
}
