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
    public class ReporteLlamadasAbiertasCerradasController : Xynthesis.Web.Models.FormatoReporte
    {
        xynthesisEntities xyt = new xynthesisEntities();
        ADReporteLlamadasAbiertasCerradas repLlamAbierCerra = new ADReporteLlamadasAbiertasCerradas();
        LogXynthesis log = new LogXynthesis();
        Constantes cons = new Constantes();
        public int contador;
        public ActionResult ListaLlamadasAbiertasCerradas(string paraPaginacion, string filtro, string FechaInicial, string FechaFinal, Decimal? hora, int? page)
        {
            int anioActual = DateTime.Now.Year - 1;
            DateTime Hoy = DateTime.Today;
            string fecha_actual = Hoy.ToString("yyyy-MM-dd");

            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            ViewData["usuario"] = (from t in xyt.xy_subscriber
                                   where t.Ide_Subscriber != -1
                                   orderby t.Nom_Subscriber ascending
                                   select t).ToList();

            ViewData["llamadaentrante"] = (from row in xyt.xyp_NumberAmountsByInSubscriber(Convert.ToString(anioActual) + "-01-01", fecha_actual, "", "", "")
                                           orderby Convert.ToDouble(row.Ide_SubscriberEmployee) ascending
                                           select row).Distinct().ToList();

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
                Session["llamadaentrante"] = null;
            }

            if (FechaInicial == null & Session["FechaInicial"] != null & valor == 0 & page == null)
            {
                Session["FechaInicial"] = null;
                Session["FechaFinal"] = null;
                Session["usuarios"] = null;
                Session["llamadaentrante"] = null;
            }


            try
            {
                List<xyp_RepCallOpenAndClosed_Result> lista;

                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null && Session["hora"] != null)
                    lista = repLlamAbierCerra.ObtenerListaLlamadasAbiertasCerradas(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["hora"].ToString(), Session["usuarios"].ToString(), Session["llamadaentrante"].ToString()).ToList();
                else
                    lista = repLlamAbierCerra.ObtenerListaLlamadasAbiertasCerradas("", "","", "","").ToList();

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
                log.EscribaLog("REPORTE", "Action:ListaLlamadasAbiertasCerradas " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }
        public ActionResult ListaLlamadasAbiertasCerradas_(string FechaInicial, string FechaFinal, string hora, string[] usuarioId,  string[] numentrante, int? page)
        {
            //=====================fecha actual=============================
            int anioActual = DateTime.Now.Year - 1;
            DateTime Hoy = DateTime.Today;
            string fecha_actual = Hoy.ToString("yyyy-MM-dd");
            //=====================fecha actual=============================

            //=====================Procesar usuario=============================
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
            //=====================Procesar usuario=============================

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

            ViewData["usuario"] = (from t in xyt.xy_subscriber
                                   where t.Ide_Subscriber != -1
                                   orderby t.Nom_Subscriber ascending
                                   select t).ToList();
            //Select para dropdowlist LlamadaEntrante
            ViewData["llamadaentrante"] = (from row in xyt.xyp_NumberAmountsByInSubscriber(Convert.ToString(anioActual) + "-01-01", fecha_actual, "", "", "")
                                           orderby Convert.ToDouble(row.Ide_SubscriberEmployee) ascending
                                           select row).Distinct().ToList();

            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            if (FechaInicial == "" || FechaFinal == "")
            {
                return RedirectToAction("ListaLlamadasAbiertasCerradas", "ReporteLlamadasAbiertasCerradas");
            }
            else
            {
                List<xyp_RepCallOpenAndClosed_Result> lista = repLlamAbierCerra.ObtenerListaLlamadasAbiertasCerradas(FechaInicial, FechaFinal,hora, user, llamEnt).ToList();
                int pageSize = 10;
                int pageIndex = 1;
                int pageNumber = (page ?? 1);
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

                Session["FechaInicial"] = FechaInicial;
                Session["FechaFinal"] = FechaFinal;
                Session["hora"] = hora;

                try
                {
                    ViewBag.fechaini = Convert.ToDateTime(Session["FechaInicial"]).ToString("dd-MM-yyyy") + " A";
                    ViewBag.fechafin = Convert.ToDateTime(Session["FechaFinal"]).ToString("dd-MM-yyyy");
                }
                catch (Exception ex)
                {

                }
                return View("ListaLlamadasAbiertasCerradas", lista.ToPagedList(pageIndex, pageSize));
            }
        }

        public ActionResult Reportes(string opcion)
        {
            if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
                return View("ListaLlamadasAbiertasCerradas", new List<xyp_RepCallOpenAndClosed_Result>().ToPagedList(1, 1));
            else
                return ReporteFormato(opcion, "ReporteLlamadasAbiertasCerradas", "ObtenerListaLlamadasAbiertasCerradas",
                    Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(),Session["hora"].ToString(), Session["usuarios"].ToString(), Session["llamadaentrante"].ToString());

        }

    }
}