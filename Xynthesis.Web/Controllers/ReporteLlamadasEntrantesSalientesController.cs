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
    public class ReporteLlamadasEntrantesSalientesController : Xynthesis.Web.Models.FormatoReporte
    {
        xynthesisEntities xyt = new xynthesisEntities();
        ADReporteLlamadasEntrantesSalientes repLlamEntrSali = new ADReporteLlamadasEntrantesSalientes();
        LogXynthesis log = new LogXynthesis();
        Constantes cons = new Constantes();
        public int contador;
        public ActionResult ListaLlamadasEntrantesSalientes(string paraPaginacion, string filtro, string FechaInicial, string FechaFinal, int? page)

        {

            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            ViewData["usuario"] = (from t in xyt.xy_subscriber
                                   where t.Ide_Subscriber != -1
                                   orderby t.Nom_Subscriber ascending
                                   select t).Distinct().ToList();

            ViewData["area"] = (from a in xyt.xy_costcenters
                                orderby a.Nom_CostCenter ascending
                                select a).Distinct().ToList();


            //ViewData["area"] = (from cos in xyt.xy_costcenters where (from a in xyt.xy_costcenters
            //                    group a.Cod_CostCenter by a.Ide_CostCenter into CostCenterGroup
            //                    select CostCenterGroup.Key).Distinct().Contains(cos.Ide_CostCenter)
            //                    select cos).ToList();




            //ViewData["area"] = (from a in xyt.xy_costcenters
            //                    group a.Cod_CostCenter by a.Cod_CostCenter into CostCenterGroup
            //                    orderby CostCenterGroup.Key ascending
            //                    select CostCenterGroup.Key).ToList();


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
                List<xyp_RepReceiveCallsLlamEntranSalien_Result> lista;

                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
                    lista = repLlamEntrSali.ObtenerListaLlamadasEntrantesSalientes(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["areas"].ToString()).ToList();
                else
                    lista = repLlamEntrSali.ObtenerListaLlamadasEntrantesSalientes(null, null, null, null).ToList();

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
                log.EscribaLog("REPORTE", "Action:ListaLlamadasEntrantesSalientes " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }
        public ActionResult ListaLlamadasEntrantesSalientes_(string FechaInicial, string FechaFinal, string[] usuarioId, string[] areaId, int? page)
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
                return RedirectToAction("ListaLlamadasEntrantesSalientes", "ReporteLlamadasEntrantesSalientes");
            }
            else
            {
                List<xyp_RepReceiveCallsLlamEntranSalien_Result> lista = repLlamEntrSali.ObtenerListaLlamadasEntrantesSalientes(FechaInicial, FechaFinal, user, are).ToList();
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
                return View("ListaLlamadasEntrantesSalientes", lista.ToPagedList(pageIndex, pageSize));
            }
        }

        public ActionResult Reportes(string opcion)
        {
            if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
                return View("ListaLlamadasEntrantesSalientes", new List<xyp_RepReceiveCallsLlamEntranSalien_Result>().ToPagedList(1, 1));
            else
                return ReporteFormato(opcion, "ReporteLlamadasEntrantesSalientes", "ObtenerListaLlamadasEntrantesSalientes",
                    Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["areas"].ToString());
        }
    }
}