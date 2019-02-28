using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xynthesis.Modelo;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using PagedList.Mvc;
using PagedList;
using System.IO;
using Xynthesis.AccesoDatos;
using Xynthesis.Utilidades;

namespace Xynthesis.Web.Controllers
{
    public class ReporteConsumosPersonalesController : Xynthesis.Web.Models.FormatoReporte
    {
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();
        Utilidades.Constantes cons = new Constantes();
        xynthesisEntities xyt = new xynthesisEntities();
        ADReporteConsumosPersonales Consper= new ADReporteConsumosPersonales();
        public int contador;
        // GET: ReporteConsumosPersonales

        //=====================fecha actual=============================
        DateTime fecha_actual = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
        DateTime fechaIni = Convert.ToDateTime((Convert.ToString(DateTime.Now.Year - 1) + "-01-01"));
        //=====================fecha actual=============================

        public ActionResult ConsumosPersonales(string paraPaginacion, string filtro, string FechaInicial, string FechaFinal, int? page)
        {

            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            ViewData["extension"] = (from e in xyt.xy_numbers
                                     where e.Tip_Extension == 1
                                     orderby e.Ide_Number ascending
                                   select e).ToList();

            ViewData["usuario"] = xyt.xyp_SelUsuarios().ToList();

            ViewData["area"] = xyt.xyp_SelAreas().ToList();

            ViewData["cobertura"] = (from filasCob in xyt.xy_coverage
                                orderby filasCob.Nom_Coverage ascending
                                select filasCob).ToList();

            //ViewData["destino"] = (from row in xyt.xyp_DestinoLlamadaCampeonaArea()
            //                       orderby row.target ascending
            //                       select row).ToList();

            ViewData["destino"] = (from row in xyt.xy_calls where row.Ide_CallType == 2 && row.Fec_Date >= fechaIni
                                           && row.Fec_Date <= fecha_actual && row.Ide_NumberTarget != "" && row.Num_CallEffectiveDuration > 0
                                           group row.Ide_NumberTarget by row.Ide_NumberTarget into NumberTargetGroup
                                           orderby NumberTargetGroup.Key ascending
                                           select NumberTargetGroup.Key).ToList();

            //Inicio de lineas agregadas
            if (Session["FechaInicial"] != null)
            {
                contador++;
            }

            int valor = contador;

            if (FechaInicial == null & Session["FechaInicial"] != null & valor == 1 & page == null)
            {
                Session["FechaInicial"] = null;
                Session["FechaFinal"] = null;
                Session["extensiones"] = null;
                Session["usuarios"] = null;
                Session["areas"] = null;
                Session["coberturas"] = null;
                Session["destinos"] = null;
            }

            if (FechaInicial == null & Session["FechaInicial"] != null & valor == 0 & page == null)
            {
                Session["FechaInicial"] = null;
                Session["FechaFinal"] = null;
                Session["extensiones"] = null;
                Session["usuarios"] = null;
                Session["areas"] = null;
                Session["coberturas"] = null;
                Session["destinos"] = null;
            }

                //if (FechaInicial == null & Session["FechaInicial"] != null & valor >= 1)
                //{
                //}
                //Fin de lineas agregadas

               
            try
            {

                List<xyp_SelConsumeByExtensionAndUser_Result> lista;

                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
                    lista = Consper.ObtenerConsumosPersonales(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["areas"].ToString(), Session["extensiones"].ToString(), Session["coberturas"].ToString(), Session["destinos"].ToString()).ToList();
                else
                    lista = Consper.ObtenerConsumosPersonales( null, null,null, null, null, null, null).ToList();

                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageNumber = (page ?? 1);

                ViewBag.fechaini = Session["FechaInicial"];
                ViewBag.fechafin = Session["FechaFinal"];

                return View(lista.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                log.EscribaLog("REPORTE", "Action:ConsumosPersonales " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }

        public ActionResult ConsumosPersonales_(string FechaInicial, string FechaFinal, string[] extensionId, string[] usuarioId, string[] areaId, string[] coberturaId, string[] destinoId, int? page)
        {

            string extension = "";
            string ext;
            if (extensionId == null)
            {
                ext = "";
            }
            else
            {
                for (var i = 0; i < extensionId.Length; i++)
                {
                    extension += extensionId[i].ToString() + "|";
                }
                ext = extension;
            }

            Session["extensiones"] = ext;

            ViewData["extension"] = (from e in xyt.xy_numbers
                                     where e.Tip_Extension == 1
                                     orderby e.Ide_Number ascending
                                     select e).ToList();


            string usuario = "";
            string user;
            if (usuarioId == null)
            {
                user = "";
            }
            else
            {
                for (var i = 0; i < usuarioId.Length; i++)
                {
                    usuario += usuarioId[i].ToString() + "|";
                }
                user = usuario;
            }

            Session["usuarios"] = user;
            //user = usuario;
            //Char separador = '|';
            //String[] ides = usuario.Split(separador);

            //String[] ides = usuarioId;

            ViewData["usuario"] = xyt.xyp_SelUsuarios().ToList();

            string area = "";
            string are;
            if (areaId == null)
            {
                are = "";
            }
            else
            {
                for (var i = 0; i < areaId.Length; i++)
                {
                    area += areaId[i].ToString() + "|";
                }
                are = area;
            }

            Session["areas"] = are;

            ViewData["area"] = xyt.xyp_SelAreas().ToList();

            string cobertura = "";
            string cob;
            if (coberturaId == null)
            {
                cob = "";
            }
            else
            {
                for (var i = 0; i < coberturaId.Length; i++)
                {
                    cobertura += coberturaId[i].ToString() + "|";
                }
                cob = cobertura;
            }

            Session["coberturas"] = cob;

            ViewData["cobertura"] = (from filasCob in xyt.xy_coverage
                                     orderby filasCob.Nom_Coverage ascending
                                     select filasCob).ToList();

            string destino = "";
            string dest;
            if (destinoId == null)
            {
                dest = "";
            }
            else
            {
                for (var i = 0; i < destinoId.Length; i++)
                {
                    destino += destinoId[i].ToString() + "|";
                }
                dest = destino;
            }

            Session["destinos"] = dest;

            ViewData["destino"] = (from row in xyt.xy_calls where row.Ide_CallType == 2 && row.Fec_Date >= fechaIni
                                   && row.Fec_Date <= fecha_actual && row.Ide_NumberTarget != "" && row.Num_CallEffectiveDuration > 0
                                   group row.Ide_NumberTarget by row.Ide_NumberTarget into NumberTargetGroup
                                   orderby NumberTargetGroup.Key ascending
                                   select NumberTargetGroup.Key).ToList();

            try
            {
                List<xyp_SelConsumeByExtensionAndUser_Result> lista = Consper.ObtenerConsumosPersonales(FechaInicial, FechaFinal, user, are, ext, cob, dest).ToList();
                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageIndex = 1;
                int pageNumber = (page ?? 1);
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

                Session["FechaInicial"] = FechaInicial;
                Session["FechaFinal"] = FechaFinal;

                ViewBag.fechaini = Session["FechaInicial"];
                ViewBag.fechafin = Session["FechaFinal"];

                return View("ConsumosPersonales", lista.ToPagedList(pageIndex, pageSize));

            }
            catch (Exception ex)
            {
                log.EscribaLog("REPORTE", "Action:ConsumosPersonales " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }

        public ActionResult Reportes(string opcion)
        {
            if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
                return View("ConsumosPersonales", new List<xyp_SelConsumeByExtensionAndUser_Result>().ToPagedList(1, 1));
            else
                return ReporteFormato(opcion, "ConsumosPersonales", "ObtenerConsumosPersonales",
                    Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["areas"].ToString(), Session["extensiones"].ToString(), Session["coberturas"].ToString(), Session["destinos"].ToString());
        }


    }
}