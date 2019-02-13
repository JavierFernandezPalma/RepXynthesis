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
    public class ReporteHistoriaConsumosController : Xynthesis.Web.Models.FormatoReporte
    {
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();
        xynthesisEntities xyt = new xynthesisEntities();
        Utilidades.Constantes cons = new Constantes();
        ADReporteHistoriaConsumo histoConsu = new ADReporteHistoriaConsumo();
        public int contador;

        // GET: ReporteHistoriaConsumos
        public ActionResult HistoricoConsumos(string paraPaginacion, string filtro, string FechaInicial, string FechaFinal, int? page)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            ViewData["area"] = (from t in xyt.xy_costcenters select t).ToList();


            List<int> anios = new List<int>();
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 30; i < currentYear; i++)
            {
                anios.Add(i);
            }

            ViewBag.LastTenYears = new SelectList(anios);





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
                Session["area"] = null;
                Session["llamadaentrante"] = null;
                Session["extension"] = null;
            }

            if (FechaInicial == null & Session["FechaInicial"] != null & valor == 0 & page == null)
            {
                Session["FechaInicial"] = null;
                Session["FechaFinal"] = null;
                Session["area"] = null;
                Session["llamadaentrante"] = null;
                Session["extension"] = null;
            }

            

            List<xyp_SelConsumeByHistory_Result> lista;
            try
            {
                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
                    lista = histoConsu.ObtenerHistoriaConsumos(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["area"].ToString(), Session["llamadaentrante"].ToString(), Session["extension"].ToString()).ToList();
                else
                    lista = histoConsu.ObtenerHistoriaConsumos("", "", "", "","").ToList();

                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageNumber = (page ?? 1);

                ViewBag.fechaini = Session["FechaInicial"];
                ViewBag.fechafin = Session["FechaFinal"];

                return View(lista.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                log.EscribaLog("REPORTE", "Action:HistoricoConsumos " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }

        }

        public ActionResult HistoricoConsumos_(string FechaInicial, string FechaFinal, string[] areaId, string[] numentrante, string[] Extension, int? page)
        {
            string area = "";
            string ar;
            if (areaId == null)
            {
                ar = "";
            }
            else
            {
                for (var i = 0; i < areaId.Length; i++)
                {
                    area += areaId[i].ToString() + "|";
                }
                ar = area;
            }

            Session["area"] = ar;
            //=====================Procesar llamada entrante=============================
            string llamadEntr = "";
            string llamEnt;
            if (numentrante == null)
            {
                llamEnt = "";
            }
            else
            {
                for (var i = 0; i < numentrante.Length; i++)
                {
                    llamadEntr += numentrante[i].ToString() + "|";
                }
                llamEnt = llamadEntr;
            }

            Session["llamadaentrante"] = llamEnt;
            //=====================Procesar llamada entrante=============================

            //=====================Procesar llamada entrante=============================
            string extens = "";
            string ext;
            if (Extension == null)
            {
                ext = "";
            }
            else
            {
                for (var i = 0; i < Extension.Length; i++)
                {
                    extens += Extension[i].ToString() + "|";
                }
                ext = extens;
            }

            Session["extension"] = ext;
            //=====================Procesar llamada entrante=============================

            ViewData["area"] = (from t in xyt.xy_costcenters select t).ToList();
            //Select para dropdowlist LlamadaEntrante
            try
            {
                List<xyp_SelConsumeByHistory_Result> lista = histoConsu.ObtenerHistoriaConsumos(FechaInicial, FechaFinal, ar, llamEnt, ext).ToList();
                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageIndex = 1;
                int pageNumber = (page ?? 1);
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

                Session["FechaInicial"] = FechaInicial;
                Session["FechaFinal"] = FechaFinal;

                ViewBag.fechaini = Session["FechaInicial"];
                ViewBag.fechafin = Session["FechaFinal"];

                return View("HistoricoConsumos", lista.ToPagedList(pageIndex, pageSize));

            }
            catch (Exception ex)
            {
                log.EscribaLog("REPORTE", "Action:HistoricoConsumos " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }

        public ActionResult Reportes(string opcion)
        {
            if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
                return View("HistoricoConsumos", new List<xyp_SelConsumeByHistory_Result>().ToPagedList(1, 1));
            else
                return ReporteFormato(opcion, "HistoriaConsumos", "ObtenerHistoriaConsumos",
                    Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["area"].ToString(), Session["llamadaentrante"].ToString(), Session["extension"].ToString());

        }

    }
}