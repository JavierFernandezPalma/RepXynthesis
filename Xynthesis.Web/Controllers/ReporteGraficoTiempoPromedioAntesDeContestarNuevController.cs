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
    public class ReporteGraficoTiempoPromedioAntesDeContestarNuevController : Xynthesis.Web.Models.FormatoReporte
    {
        xynthesisEntities xyt = new xynthesisEntities();
        ADReporteGraficoTiempoPromedioAntesDeContestar repGrafTiemp = new ADReporteGraficoTiempoPromedioAntesDeContestar();
        LogXynthesis log = new LogXynthesis();
        Constantes cons = new Constantes();
        public int contador;
        public ActionResult ListaTiempoPromedioAntesContestar(string paraPaginacion, string filtro, string FechaInicial, string FechaFinal, int? page)

        {

            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            ViewData["usuario"] = (from t in xyt.xy_subscriber
                                   where t.Ide_Subscriber != -1
                                   orderby t.Nom_Subscriber ascending
                                   select t).ToList();

            ViewData["area"] = (from a in xyt.xy_costcenters
                                orderby a.Nom_CostCenter ascending
                                select a).ToList();

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

            //if (FechaInicial == null & Session["FechaInicial"] != null & valor >= 1)
            //{
            //}
            //Fin de lineas agregadas

            try
            {
                List<xyp_ReceiveCallsTiempoPromedio_Result> lista;

                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
                    lista = repGrafTiemp.ObtenerListaTiempoPromedioAntesContestarResumido(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["areas"].ToString()).ToList();
                else
                    lista = repGrafTiemp.ObtenerListaTiempoPromedioAntesContestarResumido(null, null, null, null).ToList();

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
                log.EscribaLog("REPORTE", "Action:ListaTiempoPromedioAntesContestar " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }
        public ActionResult ListaTiempoPromedioAntesContestar_(string FechaInicial, string FechaFinal, string[] usuarioId, string[] areaId, int? page)
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
            //user = usuario;
            //Char separador = '|';
            //String[] ides = usuario.Split(separador);

            //String[] ides = usuarioId;

            ViewData["usuario"] = (from t in xyt.xy_subscriber
                                   where t.Ide_Subscriber != -1
                                   orderby t.Nom_Subscriber ascending
                                   select t).ToList();

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


            ViewData["area"] = (from a in xyt.xy_costcenters
                                orderby a.Nom_CostCenter ascending
                                select a).ToList();

            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            if (FechaInicial == "" || FechaFinal == "")
            {
                return RedirectToAction("ListaTiempoPromedioAntesContestar", "ReporteTiempoPromedioAntesContestar");
            }
            else
            {
                List<xyp_ReceiveCallsTiempoPromedio_Result> lista = repGrafTiemp.ObtenerListaTiempoPromedioAntesContestarResumido(FechaInicial, FechaFinal, user, are).ToList();
                int pageSize = 10;
                int pageIndex = 1;
                int pageNumber = (page ?? 1);
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

                Session["FechaInicial"] = FechaInicial;
                Session["FechaFinal"] = FechaFinal;

                try
                {
                    ViewBag.fechaini = Convert.ToDateTime(Session["FechaInicial"]).ToString("dd-MM-yyyy") + " A";
                    ViewBag.fechafin = Convert.ToDateTime(Session["FechaFinal"]).ToString("dd-MM-yyyy");
                }
                catch (Exception ex)
                {

                }
                return View("ListaTiempoPromedioAntesContestar", lista.ToPagedList(pageIndex, pageSize));
            }
        }

        public ActionResult Reportes(string opcion)
        {
            ////if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
            ////    return View("ListaTiempoPromedioAntesContestar", new List<xyp_ReceiveCallsTiempoPromedio_Result>().ToPagedList(1, 1));
            ////else
            ////    return Reportes_(opcion, Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), "ReporteGraficoTiempoPromedioAntesDeContestarNuevo");
            if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
                return View("ListaTiempoPromedioAntesContestar", new List<xyp_ReceiveCallsTiempoPromedio_Result>().ToPagedList(1, 1));
            else
                return ReporteFormato(opcion, "ReporteGraficoTiempoPromedioAntesDeContestarNuevo", "ObtenerListaTiempoPromedioAntesContestarResumido",
                    Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["areas"].ToString());
        }


        public JsonResult ConCober()
        {
            List<xyp_ReceiveCallsTiempoPromedio_Result> lista;
            try
            {
                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
                    lista = repGrafTiemp.ObtenerListaTiempoPromedioAntesContestarResumido(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["areas"].ToString()).ToList();
                else
                    lista = repGrafTiemp.ObtenerListaTiempoPromedioAntesContestarResumido(null, null, null, null).ToList();

                var res_ = from s in lista group s by s.Nom_Subscriber into grupo orderby grupo.Key select new { cob = grupo.Key, suma = grupo.Sum(r => Math.Round(Convert.ToDecimal(r.waiting))) };

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