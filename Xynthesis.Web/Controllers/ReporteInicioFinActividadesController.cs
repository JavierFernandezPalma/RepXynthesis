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
    public class ReporteInicioFinActividadesController : Controller
    {
        ADReporteInicioFinActividades repIniFin = new ADReporteInicioFinActividades();
        LogXynthesis log = new LogXynthesis();
        Constantes cons = new Constantes();
        public int contador;
        public ActionResult ListaInicioFin(string paraPaginacion, string filtro, string FechaInicial, string FechaFinal,  int? page)
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

            try
            {
                List<xyp_SelActivityFirstAndLast_Result> lista;

                if (Session["FechaInicial"] != null &&   Session["FechaFinal"] != null)
                    lista = repIniFin.ObtenerListaInicioFin(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), null).ToList();
                else
                    lista = repIniFin.ObtenerListaInicioFin(null, null, null).ToList();
                
                int pageSize = 10;
                int pageNumber = (page ?? 1);

                ViewBag.fechaini = Convert.ToDateTime(Session["FechaInicial"]).ToString("dd-MM-yyyy");
                ViewBag.fechafin = Convert.ToDateTime(Session["FechaFinal"]).ToString("dd-MM-yyyy");

                if(ViewBag.fechaini != "01-01-0001" && ViewBag.fechafin != "01-01-0001")
                {
                    ViewBag.fechaini = Convert.ToDateTime(Session["FechaInicial"]).ToString("dd-MM-yyyy") + " A";
                    ViewBag.fechafin = Convert.ToDateTime(Session["FechaFinal"]).ToString("dd-MM-yyyy");
                }else
                {
                    ViewBag.fechaini = null;
                    ViewBag.fechafin = null;
                }

                return View(lista.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                log.EscribaLog("REPORTE", "Action:ListaInicioFin " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }
        public ActionResult ListaInicioFin_(string FechaInicial, string FechaFinal, int? page)
        {
            if(FechaInicial == "" || FechaFinal == "")
            {
                return RedirectToAction("ListaInicioFin", "ReporteInicioFinActividades");
            }
            else
            {
                List<xyp_SelActivityFirstAndLast_Result> lista = repIniFin.ObtenerListaInicioFin(FechaInicial, FechaFinal, null).ToList();
                int pageSize = 10;
                int pageIndex = 1;
                int pageNumber = (page ?? 1);
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

                Session["FechaInicial"] = FechaInicial;
                Session["FechaFinal"] = FechaFinal;

                ViewBag.fechaini = Convert.ToDateTime(Session["FechaInicial"]).ToString("dd-MM-yyyy") + " A";
                ViewBag.fechafin = Convert.ToDateTime(Session["FechaFinal"]).ToString("dd-MM-yyyy");

                return View("ListaInicioFin", lista.ToPagedList(pageIndex, pageSize));
            }
        }

        public ActionResult Reportes(string opcion)
        {

            if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
            {
                return RedirectToAction("ListaInicioFin", "ReporteInicioFinActividades");
            }
            else
            {

                var FechaInicial = Session["FechaInicial"].ToString();
                var FechaFinal = Session["FechaFinal"].ToString();


                switch (opcion)
                {
                    case "pdf":

                        return (ReportePDF(FechaInicial, FechaFinal));

                    case "excel":

                        return (ReporteEXCEL(FechaInicial, FechaFinal));

                    case "word":

                        return (ReporteWORD(FechaInicial, FechaFinal));

                    default:
                        return View();

                }
            }
        }

        public ActionResult ReportePDF(string FechaInicial, string FechaFinal)
        {
            List<xyp_SelActivityFirstAndLast_Result> fechas = new List<xyp_SelActivityFirstAndLast_Result>();

            fechas = repIniFin.ObtenerListaInicioFin(FechaInicial, FechaFinal, null).ToList();

            ReportDocument rd = new ReportDocument();

            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "xyp_SelActivityFirstAndLast.rpt"));
            rd.SetDataSource(fechas);


            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                //=====================Inicio para Exportar a PDF============================================
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "InicioYFinDeActividades.pdf");
                //=====================Fin para Exportar a PDF===============================================

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult ReporteEXCEL(string FechaInicial, string FechaFinal)
        {
            List<xyp_SelActivityFirstAndLast_Result> fechas = new List<xyp_SelActivityFirstAndLast_Result>();

            fechas = repIniFin.ObtenerListaInicioFin(FechaInicial, FechaFinal, null).ToList();

            ReportDocument rd = new ReportDocument();

            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "xyp_SelActivityFirstAndLast.rpt"));
            rd.SetDataSource(fechas);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {

                //=====================Inicio para Exportar a Excel==========================================
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/xls", "InicioYFinDeActividades.xls");
                //=====================Fin para Exportar a Excel=============================================

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult ReporteWORD(string FechaInicial, string FechaFinal)
        {
            List<xyp_SelActivityFirstAndLast_Result> fechas = new List<xyp_SelActivityFirstAndLast_Result>();

            fechas = repIniFin.ObtenerListaInicioFin(FechaInicial, FechaFinal, null).ToList();

            ReportDocument rd = new ReportDocument();

            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "xyp_SelActivityFirstAndLast.rpt"));
            rd.SetDataSource(fechas);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {

                //=====================Inicio para Exportar a Word===========================================
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.WordForWindows);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/doc", "InicioYFinDeActividades.doc");
                //=====================Fin para Exportar a Word==============================================
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}