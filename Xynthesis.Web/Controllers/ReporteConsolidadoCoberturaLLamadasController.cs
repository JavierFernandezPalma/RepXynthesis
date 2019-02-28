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
    public class ReporteConsolidadoCoberturaLLamadasController : Xynthesis.Web.Models.FormatoReporte
    {
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();
        xynthesisEntities xyt = new xynthesisEntities();
        Utilidades.Constantes cons = new Constantes();
        ADReporteConsolidadoCoberturaLLamadas coberll = new ADReporteConsolidadoCoberturaLLamadas();
        public int contador;
        public ActionResult ConsolidaCoberturaLlamadas(string paraPaginacion, string filtro, string extension, string FechaInicial, string FechaFinal, int? page)
        {

            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            ViewData["usuario"] = xyt.xyp_SelUsuarios().ToList();

            ViewData["area"] = xyt.xyp_SelAreas().ToList();

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
                Session["usuarios"] = null;
                Session["areas"] = null;
            }

            if (FechaInicial == null & Session["FechaInicial"] != null & valor == 0 & page == null)
            {
                Session["FechaInicial"] = null;
                Session["FechaFinal"] = null;
                Session["usuarios"] = null;
                Session["areas"] = null;
            }



            List<xyp_RepConsolidadoCoberturaLLamadas_Result> lista; 
            try
            {
                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
                    lista = coberll.ObtenerConsolidadoCoberturaLlamadas(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["areas"].ToString()).ToList();
                else
                    lista = coberll.ObtenerConsolidadoCoberturaLlamadas(null, null, null, null).ToList();

                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageNumber = (page ?? 1);

                ViewBag.fechaini = Session["FechaInicial"];
                ViewBag.fechafin = Session["FechaFinal"];
                ViewBag.extension = Session["Extension"];

                return View(lista.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                log.EscribaLog("REPORTE", "Action:ConsolidaCoberturaLlamadas " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }


        public ActionResult ConsolidaCoberturaLlamadas_(string FechaInicial, string FechaFinal, string[] usuarioId, string[] areaId, int? page)
        {
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

            try
            {
                List<xyp_RepConsolidadoCoberturaLLamadas_Result> lista = coberll.ObtenerConsolidadoCoberturaLlamadas(FechaInicial, FechaFinal, user, are).ToList();
                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageIndex = 1;
                int pageNumber = (page ?? 1);
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

                Session["FechaInicial"] = FechaInicial;
                Session["FechaFinal"] = FechaFinal;

                ViewBag.fechaini = Session["FechaInicial"];
                ViewBag.fechafin = Session["FechaFinal"];

                return View("ConsolidaCoberturaLlamadas", lista.ToPagedList(pageIndex, pageSize));

            }
            catch (Exception ex)
            {
                log.EscribaLog("REPORTE", "Action:ConsolidaCoberturaLlamadas " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }


        public ActionResult Reportes(string opcion)
        {
            if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
                return View("ConsolidaCoberturaLlamadas", new List<xyp_RepConsolidadoCoberturaLLamadas_Result>().ToPagedList(1, 1));
            else
                return ReporteFormato(opcion, "ReporteConsolidadoCoberturaLLamadas", "ObtenerConsolidadoCoberturaLlamadas",
                    Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["areas"].ToString());
        }


        public JsonResult ConCober()
        {
            List<xyp_RepConsolidadoCoberturaLLamadas_Result> lista;
            try
            {
                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
                    lista = coberll.ObtenerConsolidadoCoberturaLlamadas(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["areas"].ToString()).ToList();
                else
                    lista = coberll.ObtenerConsolidadoCoberturaLlamadas(null, null, null, null).ToList();

                 var res_ = from s in lista group s by s.cobertura  into grupo orderby grupo.Key select new  { cob = grupo.Key, suma= grupo.Sum(r=>Convert.ToInt32(r.llamadasRealizadas))};
                

                return Json(res_, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.EscribaLog("REPORTE", "Action:ConCober " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }

        }

    }
    }