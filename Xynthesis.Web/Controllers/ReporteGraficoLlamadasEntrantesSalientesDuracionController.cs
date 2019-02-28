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
    public class ReporteGraficoLlamadasEntrantesSalientesDuracionController : Xynthesis.Web.Models.FormatoReporte
    {
        xynthesisEntities xyt = new xynthesisEntities();
        ADReporteGraficoLlamadasEntrantesSalientesDuracion repGrafTiemp = new ADReporteGraficoLlamadasEntrantesSalientesDuracion();
        LogXynthesis log = new LogXynthesis();
        Constantes cons = new Constantes();
        public int contador;
        public ActionResult ListaLlamadasEntrantesSalientesDuracion(string paraPaginacion, string filtro, string FechaInicial, string FechaFinal, int? page)
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


            try
            {
                List<xyp_RepGrafNumberAmountsBySubscriber_Result> lista;

                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
                    lista = repGrafTiemp.ObtenerListaLlamadasEntrantesSalientesDuracion(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["areas"].ToString()).ToList();
                else
                    lista = repGrafTiemp.ObtenerListaLlamadasEntrantesSalientesDuracion(null, null, null, null).ToList();

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
                log.EscribaLog("REPORTE", "Action:ListaLlamadasEntrantesSalientesDuracion " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }
        public ActionResult ListaLlamadasEntrantesSalientesDuracion_(string FechaInicial, string FechaFinal, string[] usuarioId, string[] areaId, int? page)
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

            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            if (FechaInicial == "" || FechaFinal == "")
            {
                return RedirectToAction("ListaLlamadasEntrantesSalientesDuracion", "ReporteGraficoLlamadasEntrantesSalientesDuracion");
            }
            else
            {
                List<xyp_RepGrafNumberAmountsBySubscriber_Result> lista = repGrafTiemp.ObtenerListaLlamadasEntrantesSalientesDuracion(FechaInicial, FechaFinal, user, are).ToList();
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
                return View("ListaLlamadasEntrantesSalientesDuracion", lista.ToPagedList(pageIndex, pageSize));
            }
        }

        public ActionResult Reportes(string opcion)
        {
            if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
                return View("ListaLlamadasEntrantesSalientesDuracion", new List<xyp_RepGrafNumberAmountsBySubscriber_Result>().ToPagedList(1, 1));
            else
                return ReporteFormato(opcion, "ReporteGraficoLlamadasEntrantesSalientesDuracion", "ObtenerListaLlamadasEntrantesSalientesDuracion",
                    Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["areas"].ToString());
        }

        public JsonResult ConCober()
        {
            List<xyp_RepGrafNumberAmountsBySubscriber_Result> lista;

            var fe1 = Session["FechaInicial"].ToString();
            var fe2 = Session["FechaFinal"].ToString();

            try
            {
                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
                    lista = repGrafTiemp.ObtenerListaLlamadasEntrantesSalientesDuracion(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["areas"].ToString()).ToList();
                else
                    lista = repGrafTiemp.ObtenerListaLlamadasEntrantesSalientesDuracion(null, null, null, null).ToList();

                var res_ = from s in lista group s by s.Usuario into grupo orderby grupo.Key select new { cob = grupo.Key, suma = grupo.Sum(r => Convert.ToInt32(r.NroLlamadasEntrantes)) };

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