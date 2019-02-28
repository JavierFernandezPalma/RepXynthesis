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
    public class ReporteConsumosPorCentrosCostosController : Xynthesis.Web.Models.FormatoReporte
    {

        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();
        Utilidades.Constantes cons = new Constantes();
        xynthesisEntities xyt = new xynthesisEntities();
        ADReporteConsumosPorCentroCost consuCC = new ADReporteConsumosPorCentroCost();
        // GET: ReporteConsumosPorCentrosCostos
        public int contador;
        public ActionResult ConsumosXcc(string paraPaginacion, string filtro,  string FechaInicial, string FechaFinal, int? page)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            ViewData["area"] = xyt.xyp_SelAreas().ToList();

            ViewData["cobertura"] = (from c in xyt.xy_coverage where c.Ide_Coverage != -1 orderby c.Nom_Coverage ascending select c).ToList();

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
                Session["cobertura"] = null;
            }

            if (FechaInicial == null & Session["FechaInicial"] != null & valor == 0 & page == null)
            {
                Session["FechaInicial"] = null;
                Session["FechaFinal"] = null;
                Session["area"] = null;
                Session["cobertura"] = null;
            }


            List<xyp_SelConsumeByCostCenter_Result> lista;
            try
            {
                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
                    lista = consuCC.ObtenerConsumosPorcentroCostos( Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["area"].ToString(), Session["cobertura"].ToString()).ToList();
                else
                    lista = consuCC.ObtenerConsumosPorcentroCostos( null, null, null, null).ToList();

                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageNumber = (page ?? 1);

                ViewBag.fechaini = Session["FechaInicial"];
                ViewBag.fechafin = Session["FechaFinal"];

                return View(lista.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                log.EscribaLog("REPORTE", "Action:ConsumosXcc " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }

        public ActionResult ConsumosXcc_(string FechaInicial, string FechaFinal, string[] areaId, string[] coberturaId, int? page)
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
            //user = usuario;
            //Char separador = '|';
            //String[] ides = usuario.Split(separador);

            //String[] ides = usuarioId;

            ViewData["area"] = xyt.xyp_SelAreas().ToList();

            string cobertura = "";
            string cober;
            if (coberturaId == null)
            {
                cober = "";
            }
            else
            {
                for (var i = 0; i < coberturaId.Length; i++)
                {
                    cobertura += coberturaId[i].ToString() + "|";
                }
                cober = cobertura;
            }

            Session["cobertura"] = cober;
            ViewData["cobertura"] = (from c in xyt.xy_coverage where c.Ide_Coverage != -1 orderby c.Nom_Coverage ascending select c).ToList();

            try
            {
                List<xyp_SelConsumeByCostCenter_Result> lista = consuCC.ObtenerConsumosPorcentroCostos(FechaInicial, FechaFinal, ar, cober).ToList();
                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageIndex = 1;
                int pageNumber = (page ?? 1);
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

                Session["FechaInicial"] = FechaInicial;
                Session["FechaFinal"] = FechaFinal;

                ViewBag.fechaini = Session["FechaInicial"];
                ViewBag.fechafin = Session["FechaFinal"];

                return View("ConsumosXcc", lista.ToPagedList(pageIndex, pageSize));

            }
            catch (Exception ex)
            {
                log.EscribaLog("REPORTE", "Action:ConsumosXcc " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }

        public ActionResult Reportes(string opcion)
        {
            ////if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
            ////    return View("ConsumosXcc", new List<xyp_SelConsumeByCostCenter_Result>().ToPagedList(1, 1));
            ////else
            ////    return Reportes_(opcion, Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), "ConsumoPorCentrosCostos");
            if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
                return View("ConsumosXcc", new List<xyp_SelConsumeByCostCenter_Result>().ToPagedList(1, 1));
            else
                return ReporteFormato(opcion, "ConsumoPorCentrosCostos", "ObtenerConsumosPorcentroCostos",
                    Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["area"].ToString(), Session["cobertura"].ToString());
        }

               

    }
}