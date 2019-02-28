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
    public class RepPromedioLlamadasHoraController :Xynthesis.Web.Models.FormatoReporte
    {
        xynthesisEntities xyt = new xynthesisEntities();
        ADRepPromedioLlamadasHora prollama = new ADRepPromedioLlamadasHora();
        LogXynthesis log = new LogXynthesis();
        Constantes cons = new Constantes();
        public int contador;
        List<string> list = new List<string>();
        // GET: RepPromedioLlamadasHora
        public ActionResult ListaPromedioLlamadaHora(string paraPaginacion, string filtro, string FechaInicial, string FechaFinal, int? page)
        {

            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            ViewData["usuario"] = xyt.xyp_SelUsuarios().ToList();

            ViewData["area"] = xyt.xyp_SelAreas().ToList();


            list.Add("00:00 -  00:59");
            list.Add("01:00 -  01:59");
            list.Add("02:00 -  02:59");
            list.Add("03:00 -  03:59");
            list.Add("04:00 -  04:59");
            list.Add("05:00 -  05:59");
            list.Add("06:00 -  06:59");
            list.Add("07:00 -  07:59");
            list.Add("08:00 -  08:59");
            list.Add("09:00 -  09:59");
            list.Add("10:00 -  10:59");
            list.Add("11:00 -  11:59");
            list.Add("12:00 -  12:59");

            ViewData["rango"] = list;

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
                Session["rangos"] = null;
            }

            if (FechaInicial == null & Session["FechaInicial"] != null & valor == 0 & page == null)
            {
                Session["FechaInicial"] = null;
                Session["FechaFinal"] = null;
                Session["usuarios"] = null;
                Session["areas"] = null;
                Session["rangos"] = null;
            }

            //if (FechaInicial == null & Session["FechaInicial"] != null & valor >= 1)
            //{
            //}
            //Fin de lineas agregadas

            try
            {
                List<xyp_RepPromedioLlamadasHora_Result> lista;

                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
                    lista = prollama.ObtenerPromedioLlamadasHora(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["areas"].ToString(), Session["rangos"].ToString()).ToList();
                else
                    lista = prollama.ObtenerPromedioLlamadasHora(null, null, null, null, null).ToList();

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
                log.EscribaLog("REPORTE", "Action:ListaPromedioLlamadaHora " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }

        public ActionResult ListaPromedioLlamadaHora_(string FechaInicial, string FechaFinal, string[] usuarioId, string[] areaId, string[] rangoId, int? page)
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


            string rango = "";
            string ran;
            if (rangoId == null)
            {
                ran = "";
            }
            else
            {
                foreach (var item in rangoId)
                {
                    rango += item + "|";
                 }
                ran = rango;
            }

            Session["rangos"] = ran;


            list.Add("00:00 -  00:59");
            list.Add("01:00 -  01:59");
            list.Add("02:00 -  02:59");
            list.Add("03:00 -  03:59");
            list.Add("04:00 -  04:59");
            list.Add("05:00 -  05:59");
            list.Add("06:00 -  06:59");
            list.Add("07:00 -  07:59");
            list.Add("08:00 -  08:59");
            list.Add("09:00 -  09:59");
            list.Add("10:00 -  10:59");
            list.Add("11:00 -  11:59");
            list.Add("12:00 -  12:59");


            ViewData["rango"] = list;


            if (FechaInicial == "" || FechaFinal == "")
            {
                return RedirectToAction("ListaPromedioLlamadaHora", "RepPromedioLlamadasHora");
            }
            else
            {
                List<xyp_RepPromedioLlamadasHora_Result> lista = prollama.ObtenerPromedioLlamadasHora(FechaInicial, FechaFinal, user, are, ran).ToList();
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
                return View("ListaPromedioLlamadaHora", lista.ToPagedList(pageIndex, pageSize));
            }
        }


        public ActionResult Reportes(string opcion)
        {
            if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
                return View("ListaPromedioLlamadaHora", new List<xyp_RepPromedioLlamadasHora_Result>().ToPagedList(1, 1));
            else
                return ReporteFormato(opcion, "RepPromedioLlamadasHora", "ObtenerPromedioLlamadasHora",
                    Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["areas"].ToString(), Session["rangos"].ToString());
            //Reportes_(opcion, Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), "RepPromedioLlamadasHora");
        }

        public JsonResult ConPromediolla()
        {
            List<xyp_RepPromedioLlamadasHora_Result> lista;
            try
            {
                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
                    lista = prollama.ObtenerPromedioLlamadasHora(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["areas"].ToString(), Session["rangos"].ToString()).ToList();
                else
                    lista = prollama.ObtenerPromedioLlamadasHora(null, null, null, null, null).ToList();

                var res_ = from s in lista  select s;

                return Json(res_, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.EscribaLog("REPORTE", "Action:ConPromediolla " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }

        }



    }
}