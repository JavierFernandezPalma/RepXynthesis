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
    public class ReporteFrecuenciadellamadasController : Xynthesis.Web.Models.FormatoReporte
    {
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();
        Utilidades.Constantes cons = new Constantes();
        ADOReporteFrecuenciadellamadas frlla = new ADOReporteFrecuenciadellamadas();
        public int contador;

        public ActionResult FrecuenciaLlamadas(string paraPaginacion, string filtro, string FechaInicial, string FechaFinal, int? page)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

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
            }

            if (FechaInicial == null & Session["FechaInicial"] != null & valor == 0 & page == null)
            {
                Session["FechaInicial"] = null;
                Session["FechaFinal"] = null;
            }

            //if (FechaInicial == null & Session["FechaInicial"] != null & valor >= 1)
            //{
            //}
            //Fin de lineas agregadas
            List<xyp_SelFrequentExtensionNumber_Result> lista;
            try
            {
                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
                    lista = frlla.ObtenerFrecuenciaDeLlamadas(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString()).ToList();
                else
                    lista = frlla.ObtenerFrecuenciaDeLlamadas(null, null).ToList();

                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla); 
                int pageNumber = (page ?? 1);

                ViewBag.fechaini = Session["FechaInicial"];
                ViewBag.fechafin = Session["FechaFinal"];

                return View(lista.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                log.EscribaLog("REPORTE", "Action:FrecuenciaLlamadas " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }


        public ActionResult FrecuenciaLlamadas_(string FechaInicial, string FechaFinal, int? page)
        {

            try
            {
                List<xyp_SelFrequentExtensionNumber_Result> lista = frlla.ObtenerFrecuenciaDeLlamadas(FechaInicial, FechaFinal).ToList();
                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageIndex = 1;
                int pageNumber = (page ?? 1);
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

                Session["FechaInicial"] = FechaInicial;
                Session["FechaFinal"] = FechaFinal;

                ViewBag.fechaini = Session["FechaInicial"];
                ViewBag.fechafin = Session["FechaFinal"];

                return View("FrecuenciaLlamadas", lista.ToPagedList(pageIndex, pageSize));

            }
            catch (Exception ex)
            {
                log.EscribaLog("REPORTE", "Action:FrecuenciaLlamadas " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }

        public ActionResult Reportes(string opcion)
        {
            ////if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
            ////    return View("FrecuenciaLlamadas", new List<xyp_SelFrequentExtensionNumber_Result>().ToPagedList(1, 1));
            ////else
            ////    return Reportes_(opcion, Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), "FrecuenciaLlamadas");
            if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
                return View("FrecuenciaLlamadas", new List<xyp_SelFrequentExtensionNumber_Result>().ToPagedList(1, 1));
            else
                return ReporteFormato(opcion, "FrecuenciaLlamadas", "ObtenerFrecuenciaDeLlamadas",
                    Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString());
        }

       

    }
}