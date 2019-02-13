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
    public class ADOperadorTarifaInter
    {

        xynthesisEntities xyt = new xynthesisEntities();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();
        public List<xyp_SelOperTarifInterna_Result> ObtenerListaOperadoresInter()
        {
            try
            {
                return xyt.xyp_SelOperTarifInterna(null).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<xy_operators> ObtenerOperadorInternacional()
        {

            try
            {
                var oper = (from s in xyt.xy_operators
                           join c in xyt.xy_coverage on s.Ide_Coverage equals c.Ide_Coverage
                           where c.Internacional == true
                           select s).ToList();
                return oper; 

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IQueryable<xy_pais> ObtenerPaises()
        {
            var pais = from s in xyt.xy_pais select s;
            return pais;
        }


        public List<xyp_SelOperTarifInterna_Result> OrdenFiltro(string sortOrder, string searchString)
        {
            List<xyp_SelOperTarifInterna_Result> res;
            try
            {
                int totalRegis = (from x in xyt.xy_numbers select x).Count();
                if (!String.IsNullOrEmpty(searchString))
                {
                    res = xyt.xyp_SelOperTarifInterna(null).Where(s => s.Nom_Operator.ToUpper().Contains(searchString.ToUpper())).ToList();
                }
                else
                {
                    res = xyt.xyp_SelOperTarifInterna(null).ToList();
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

        public xy_operador_tarifa_inter buscarOperadorInterId(long id)
        {
            try
            {
                return xyt.xy_operador_tarifa_inter.Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<xy_operador_tarifa_inter> buscarOperadorPais(long Idoperador, long Idpais)
        {
            try
            {
                return (from s in xyt.xy_operador_tarifa_inter where s.IdOperador == Idoperador  &&  s.IdPais == Idpais  select s).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public Xynthesis.Utilidades.Mensaje nuevaOperadorTarifaInter(string DdlOperador, string DdlPais, string VlrInternaMinsinIva, string VlrInternaMinconIva, string Fijo, string Movil)
        {
            msg = new Mensaje();
            try
            {
                xy_operador_tarifa_inter oper = new xy_operador_tarifa_inter();
                oper.IdOperador = Convert.ToInt32(DdlOperador);
                oper.IdPais = Convert.ToInt32(DdlPais);
                oper.VlrInternaMinsinIva = Convert.ToDecimal(VlrInternaMinsinIva);
                oper.VlrInternaMinconIva = Convert.ToDecimal(VlrInternaMinconIva);
                if ((from s in xyt.xy_operador_tarifa_inter where s.IdOperador == oper.IdOperador && s.IdPais  ==oper.IdPais  select s).Count() <= 0)
                {
                    if (Fijo != null)
                        if (Fijo == "on")
                            oper.Fijo = 1;
                        else
                            oper.Fijo = 0;
                    else
                        oper.Fijo = 0;



                        //   int verif = (from s in xyt.xy_operador_tarifa_inter
                        //             where s.IdOperador == oper.IdOperador
                        //             && s.IdPais == oper.IdPais
                        //             && s.Fijo == oper.Fijo
                        //             select s).Count();
                        //}
                     if (Movil != null)
                          if (Movil == "on")
                            oper.Movil = 1;
                        else
                            oper.Movil = 0;
                     else
                        oper.Movil = 0;
                    //int verif_ = (from s in xyt.xy_operador_tarifa_inter
                    //              where s.IdOperador == oper.IdOperador
                    //              && s.IdPais == oper.IdPais
                    //              && s.Movil == oper.Movil
                    //              select s).Count();
                    //if (verif_ > 0)
                    //{

                    //}
                    //else
                    //        oper.Movil = 0;

                    //  xyt.xy_operador_tarifa_inter.Add(oper);
                    //  xyt.SaveChanges();
                    xyt.xyp_Insoperador_tarifa_inter((int)oper.IdOperador, (int)oper.IdPais, oper.VlrInternaMinsinIva, oper.VlrInternaMinconIva,(sbyte) oper.Movil, (sbyte)oper.Fijo);
                    msg.codigo = 1;
                    msg.mensaje = MensajesXynthesis.Nuevo;
                }
                else
                {
                    msg.codigo = 0;
                    msg.mensaje = MensajesXynthesis.existeRegi;
                    //msg.mensaje = MensajesXynthesis.NoProcesa;
                }
                return msg;

            }
            catch (Exception ex)
            {
                msg.codigo = 0;
                msg.mensaje = MensajesXynthesis.ErrDesconocido;
                log.EscribaLog("OPERADORTARINTER", "Action:nuevaOperadorTarifaInter " + ex.Message, "");
                return msg;
            }
        }

        public Xynthesis.Utilidades.Mensaje guardarEdicion(int? idtarifaInter, string DdlOperador, string DdlPais, string VlrInternaMinsinIva, string VlrInternaMinconIva, string Fijo, string Movil)
        {

            try {
                xy_operador_tarifa_inter oper = new xy_operador_tarifa_inter();
                oper = xyt.xy_operador_tarifa_inter.Find(idtarifaInter);
                oper.IdOperador = Convert.ToInt32(DdlOperador);
                oper.IdPais = Convert.ToInt32(DdlPais);
                oper.VlrInternaMinsinIva = Convert.ToDecimal(VlrInternaMinsinIva);
                oper.VlrInternaMinconIva = Convert.ToDecimal(VlrInternaMinconIva);
                if (Fijo != null)
                {
                    oper.Fijo = 1;
                    //int verif = (from s in xyt.xy_operador_tarifa_inter
                    //             where s.IdOperador == oper.IdOperador
                    //             && s.IdPais == oper.IdPais
                    //             && s.Fijo == oper.Fijo
                    //             select s).Count();
                    //if (verif > 0)
                    //{

                    //}
                }
                else
                {
                    oper.Fijo = 0;

                }
                if (Movil != null)
                {
                    oper.Movil = 1;
                    //int verif_ = (from s in xyt.xy_operador_tarifa_inter
                    //              where s.IdOperador == oper.IdOperador
                    //              && s.IdPais == oper.IdPais
                    //              && s.Movil == oper.Movil
                    //              select s).Count();

                }
                else
                    oper.Movil = 0;


                xyt.xyp_Updoperador_tarifa_inter((int)oper.idtarifaInter, (int)oper.IdOperador, (int)oper.IdPais, oper.VlrInternaMinsinIva, oper.VlrInternaMinconIva, (sbyte)oper.Movil,(sbyte) oper.Fijo);
               // xyt.SaveChanges();
                msg.codigo = 1;
                msg.mensaje = MensajesXynthesis.Actualiza;
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

        public Int32 eliminarOperadorTarifaInter(long id)
        {
            try
            {
                xy_operador_tarifa_inter xy_operador_tarifa_inter = xyt.xy_operador_tarifa_inter.Find(id);
                xyt.xy_operador_tarifa_inter.Remove(xy_operador_tarifa_inter);
                xyt.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
