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
using Xynthesis.Utilidades;
using Xynthesis.AccesoDatos;

namespace Xynthesis.Web.Controllers
{
    public class ReporteClaseDeLlamadasController   :Xynthesis.Web.Models.FormatoReporte
    {
    
        xynthesisEntities xyt = new xynthesisEntities();
        ADReporteClaseDeLlamadas repClasLlam = new ADReporteClaseDeLlamadas();
        LogXynthesis log = new LogXynthesis();
        Constantes cons = new Constantes();
        public int contador;
        public ActionResult ListaClaseDeLlamadas(string paraPaginacion, string filtro, string FechaInicial, string FechaFinal, int? page)
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
            //

            try
            {
                List<xyp_CallTypesAmountReport_Result> lista;

                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
                    lista = repClasLlam.ObtenerListaClaseDeLlamadas(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString()).ToList();
                else
                    lista = repClasLlam.ObtenerListaClaseDeLlamadas(null, null).ToList();

                int pageSize = 10;
                int pageNumber = (page ?? 1);

                ViewBag.fechaini = Convert.ToDateTime(Session["FechaInicial"]).ToString("dd-MM-yyyy");
                ViewBag.fechafin = Convert.ToDateTime(Session["FechaFinal"]).ToString("dd-MM-yyyy");

                if (ViewBag.fechaini != "01-01-0001" && ViewBag.fechafin != "01-01-0001")
                {
                    ViewBag.fechaini = Convert.ToDateTime(Session["FechaInicial"]).ToString("dd-MM-yyyy") + " A";
                    ViewBag.fechafin = Convert.ToDateTime(Session["FechaFinal"]).ToString("dd-MM-yyyy");
                }
                else
                {
                    ViewBag.fechaini = null;
                    ViewBag.fechafin = null;
                }

                return View(lista.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                log.EscribaLog("REPORTE", "Action:ListaClaseDeLlamadas " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }
        public ActionResult ListaClaseDeLlamadas_(string FechaInicial, string FechaFinal, int? page)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            if (FechaInicial == "" || FechaFinal == "")
            {
                return RedirectToAction("ListaClaseDeLlamadas", "ReporteClaseDeLlamadas");
            }
            else
            {
                List<xyp_CallTypesAmountReport_Result> lista = repClasLlam.ObtenerListaClaseDeLlamadas(FechaInicial, FechaFinal).ToList();
                int pageSize = 10;
                int pageIndex = 1;
                int pageNumber = (page ?? 1);
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

                Session["FechaInicial"] = FechaInicial;
                Session["FechaFinal"] = FechaFinal;

                ViewBag.fechaini = Convert.ToDateTime(Session["FechaInicial"]).ToString("dd-MM-yyyy") + " A";
                ViewBag.fechafin = Convert.ToDateTime(Session["FechaFinal"]).ToString("dd-MM-yyyy");

                return View("ListaClaseDeLlamadas", lista.ToPagedList(pageIndex, pageSize));
            }
        }


        public ActionResult Reportes(string opcion)
        {
            ////if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
            ////    return View("ListaClaseDeLlamadas", new List<xyp_CallTypesAmountReport_Result>().ToPagedList(1, 1));
            ////else
            ////    return Reportes_(opcion, Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), "ReporteClaseDeLlamadas");
            if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
                return View("ListaClaseDeLlamadas", new List<xyp_CallTypesAmountReport_Result>().ToPagedList(1, 1));
            else
                return ReporteFormato(opcion, "ReporteClaseDeLlamadas", "ObtenerListaClaseDeLlamadas",
                    Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString());
        }

    }
}
