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
    public class ReporteLlamadaCampeonaDuracionController : Xynthesis.Web.Models.FormatoReporte
    {

        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();
        Utilidades.Constantes cons = new Constantes();
        ADLlamadaCampeonaDuracion llamadaCam = new ADLlamadaCampeonaDuracion();
        xynthesisEntities xyt = new xynthesisEntities();
        public int contador;

        // GET: ReporteLlamadaCampeonaDuracion
        public ActionResult LlamadaCampeonaDuracion(string paraPaginacion, string filtro, string FechaInicial, string FechaFinal, int? page)
        {
            int anioActual = DateTime.Now.Year - 1;
            DateTime Hoy = DateTime.Today;
            string fecha_actual = Hoy.ToString("yyyy-MM-dd");

            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            ViewData["area"] = xyt.xyp_SelAreas().ToList();

            ViewData["llamadaentrante"] = (from row in xyt.xyp_NumberAmountsByInSubscriber(Convert.ToString(anioActual) + "-01-01", fecha_actual, "", "", "")
                                           group row.Ide_NumberSource by row.Ide_NumberSource into NumberSourceGroup
                                           orderby NumberSourceGroup.Key ascending
                                           select NumberSourceGroup.Key).ToList();
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
            }

            if (FechaInicial == null & Session["FechaInicial"] != null & valor == 0 & page == null)
            {
                Session["FechaInicial"] = null;
                Session["FechaFinal"] = null;
                Session["area"] = null;
                Session["llamadaentrante"] = null;
            }

            
            List<xyp_SelDetailChampCallDuration_Result> lista;
            try
            {
                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
                    lista = llamadaCam.ObtenerLlamadaCampeonaDuracion(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["area"].ToString(), Session["llamadaentrante"].ToString()).ToList();
                else
                    lista = llamadaCam.ObtenerLlamadaCampeonaDuracion("", "", "","").ToList();

                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageNumber = (page ?? 1);

                ViewBag.fechaini = Session["FechaInicial"];
                ViewBag.fechafin = Session["FechaFinal"];

                return View(lista.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                log.EscribaLog("REPORTE", "Action:LlamadaCampeonaDuracion " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }

        public ActionResult LlamadaCampeonaDuracion_(string FechaInicial, string FechaFinal, string[] areaId, string[] numentrante, int? page)
        {
            //=====================fecha actual=============================
            int anioActual = DateTime.Now.Year - 1;
            DateTime Hoy = DateTime.Today;
            string fecha_actual = Hoy.ToString("yyyy-MM-dd");
            //=====================fecha actual=============================

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


            //user = usuario;
            //Char separador = '|';
            //String[] ides = usuario.Split(separador);

            //String[] ides = usuarioId;

            ViewData["area"] = xyt.xyp_SelAreas().ToList();

            ViewData["llamadaentrante"] = (from row in xyt.xyp_NumberAmountsByInSubscriber(Convert.ToString(anioActual) + "-01-01", fecha_actual, "", "", "")
                                           group row.Ide_NumberSource by row.Ide_NumberSource into NumberSourceGroup
                                           orderby NumberSourceGroup.Key ascending
                                           select NumberSourceGroup.Key).ToList();

            try
            {
                List<xyp_SelDetailChampCallDuration_Result> lista = llamadaCam.ObtenerLlamadaCampeonaDuracion(FechaInicial, FechaFinal, ar, llamEnt).ToList();
                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageIndex = 1;
                int pageNumber = (page ?? 1);
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

                Session["FechaInicial"] = FechaInicial;
                Session["FechaFinal"] = FechaFinal;

                ViewBag.fechaini = Session["FechaInicial"];
                ViewBag.fechafin = Session["FechaFinal"];

                return View("LlamadaCampeonaDuracion", lista.ToPagedList(pageIndex, pageSize));

            }
            catch (Exception ex)
            {
                log.EscribaLog("REPORTE", "Action:LlamadaCampeonaDuracion_ " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }

        public ActionResult Reportes(string opcion)
        {
            if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
                return View("LlamadaCampeonaDuracion", new List<xyp_SelDetailChampCallDuration_Result>().ToPagedList(1, 1));
            else
                return ReporteFormato(opcion, "LlamadaCampeonaXDuracion", "ObtenerLlamadaCampeonaDuracion",
                    Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["area"].ToString(), Session["llamadaentrante"].ToString());

        }


    }
}   