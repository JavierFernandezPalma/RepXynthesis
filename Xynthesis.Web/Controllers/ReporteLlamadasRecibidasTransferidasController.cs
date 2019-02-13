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
    public class ReporteLlamadasRecibidasTransferidasController : Xynthesis.Web.Models.FormatoReporte
    {
        xynthesisEntities xyt = new xynthesisEntities();
        ADReporteLlamadasRecibidasTransferidas repLlamRecibTransf = new ADReporteLlamadasRecibidasTransferidas();
        LogXynthesis log = new LogXynthesis();
        Constantes cons = new Constantes();
        public int contador;
        public ActionResult ListaLlamadasRecibidasTransferidas(string paraPaginacion, string filtro, string FechaInicial, string FechaFinal, int? page)

        {

            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            ViewData["usuario"] = (from t in xyt.xy_subscriber
                                   where t.Ide_Subscriber != -1
                                   orderby t.Nom_Subscriber ascending
                                   select t).ToList();
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
            }

            if (FechaInicial == null & Session["FechaInicial"] != null & valor == 0 & page == null)
            {
                Session["FechaInicial"] = null;
                Session["FechaFinal"] = null;
                Session["usuarios"] = null;
            }

            //if (FechaInicial == null & Session["FechaInicial"] != null & valor >= 1)
            //{
            //}
            //Fin de lineas agregadas

            try
            {
                List<xyp_ReceiveAndTransferCalls_Result> lista;

                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
                    lista = repLlamRecibTransf.ObtenerListaLlamadasRecibidasTransferidas(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString()).ToList();
                else
                    lista = repLlamRecibTransf.ObtenerListaLlamadasRecibidasTransferidas(null, null, null).ToList();

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
                log.EscribaLog("REPORTE", "Action:ListaLlamadasRecibidasTransferidas " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }
        public ActionResult ListaLlamadasRecibidasTransferidas_(string FechaInicial, string FechaFinal, string[] usuarioId, int? page)
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
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            if (FechaInicial == "" || FechaFinal == "")
            {
                return RedirectToAction("ListaLlamadasRecibidasTransferidas", "ReporteLlamadasRecibidasTransferidas");
            }
            else
            {
                List<xyp_ReceiveAndTransferCalls_Result> lista = repLlamRecibTransf.ObtenerListaLlamadasRecibidasTransferidas(FechaInicial, FechaFinal, user).ToList();
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
                return View("ListaLlamadasRecibidasTransferidas", lista.ToPagedList(pageIndex, pageSize));
            }
        }

        public ActionResult Reportes(string opcion)
        {
            if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
                return View("ListaLlamadasRecibidasTransferidas", new List<xyp_ReceiveAndTransferCalls_Result>().ToPagedList(1, 1));
            else
                return ReporteFormato(opcion, "ReporteLlamadasRecibidasTransferencias", "ObtenerListaLlamadasRecibidasTransferidas",
                    Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString());


        }
    }
}