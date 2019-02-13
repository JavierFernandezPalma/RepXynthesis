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
    public class ReporteNumeroMasMarcadoController : Xynthesis.Web.Models.FormatoReporte
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();
        Utilidades.Constantes cons = new Constantes();
        ADReporteNumeroMasMarcado frlla = new ADReporteNumeroMasMarcado();
        public int contador;
        // GET: ReporteNumeroMasMarcado
        public ActionResult NumerosMasMarcados(string paraPaginacion, string filtro, string FechaInicial, string FechaFinal, int? page)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            ViewData["Origen"] = (from o in xyt.xyp_GrupNumSuscriber() orderby o.Ide_Number ascending select o).ToList();

            ViewData["Destino"] = (from d in xyt.xy_numbers
                                orderby d.Ide_Number ascending
                                select d).ToList();

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
                Session["Origenes"] = null;
                Session["Destinos"] = null;
            }

            if (FechaInicial == null & Session["FechaInicial"] != null & valor == 0 & page == null)
            {
                Session["FechaInicial"] = null;
                Session["FechaFinal"] = null;
                Session["Origenes"] = null;
                Session["Destinos"] = null;
            }

            //if (FechaInicial == null & Session["FechaInicial"] != null & valor >= 1)
            //{
            //}
            //Fin de lineas agregadas

            List<xyp_SelDialedNumber_Result> lista;
            try
            {
                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
                    lista = frlla.ObtenerNumeroMasMarcado(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["Origenes"].ToString(), Session["Destinos"].ToString()).ToList();
                else
                    lista = frlla.ObtenerNumeroMasMarcado(null, null, null, null).ToList();

                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageNumber = (page ?? 1);

                ViewBag.fechaini = Session["FechaInicial"];
                ViewBag.fechafin = Session["FechaFinal"];

                return View(lista.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                log.EscribaLog("REPORTE", "Action:NumerosMasMarcados " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }

        public ActionResult NumerosMasMarcados_(string FechaInicial, string FechaFinal, string[] origenId, string[] destinoId, int? page)
        {

            string origen = "";
            string ori;
            if (origenId == null)
            {
                ori = "";
            }
            else
            {
                for (var i = 0; i < origenId.Length; i++)
                {
                    origen += origenId[i].ToString() + "|";
                }
                ori = origen;
            }

            Session["Origenes"] = ori;

            ViewData["Origen"] = (from o in xyt.xyp_GrupNumSuscriber() orderby o.Ide_Number ascending select o).ToList();

            string destino = "";
            string dest;
            if (destinoId == null)
            {
                dest = "";
            }
            else
            {
                for (var i = 0; i < destinoId.Length; i++)
                {
                    destino += destinoId[i].ToString() + "|";
                }
                dest = destino;
            }

            Session["Destinos"] = dest;


            ViewData["Destino"] = (from d in xyt.xy_numbers
                                   orderby d.Ide_Number ascending
                                   select d).ToList();

            try
            {
                List<xyp_SelDialedNumber_Result> lista = frlla.ObtenerNumeroMasMarcado(FechaInicial, FechaFinal, ori, dest).ToList();
                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageIndex = 1;
                int pageNumber = (page ?? 1);
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

                Session["FechaInicial"] = FechaInicial;
                Session["FechaFinal"] = FechaFinal;

                ViewBag.fechaini = Session["FechaInicial"];
                ViewBag.fechafin = Session["FechaFinal"];

                return View("NumerosMasMarcados", lista.ToPagedList(pageIndex, pageSize));

            }
            catch (Exception ex)
            {
                log.EscribaLog("REPORTE", "Action:NumeroMasMarcado " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }

        public ActionResult Reportes(string opcion)
        {
            ////if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
            ////    return View("NumerosMasMarcados", new List<xyp_SelDialedNumber_Result>().ToPagedList(1, 1));
            ////else
            ////    return Reportes_(opcion, Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), "NumeroMasMarcado");
            if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
                return View("NumerosMasMarcados", new List<xyp_SelDialedNumber_Result>().ToPagedList(1, 1));
            else
                return ReporteFormato(opcion, "NumeroMasMarcado", "ObtenerNumeroMasMarcado",
                    Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["Origenes"].ToString(), Session["Destinos"].ToString());


        }



    }
}