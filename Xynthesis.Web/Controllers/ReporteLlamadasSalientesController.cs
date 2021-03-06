﻿using System;
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
    public class ReporteLlamadasSalientesController : Xynthesis.Web.Models.FormatoReporte
    {
        xynthesisEntities xyt = new xynthesisEntities();
        ADReporteLlamadasSalientes repLlamSalien = new ADReporteLlamadasSalientes();
        LogXynthesis log = new LogXynthesis();
        Constantes cons = new Constantes();
        public int contador;

        //=====================fecha actual=============================
        DateTime fecha_actual = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
        DateTime fechaIni = Convert.ToDateTime((Convert.ToString(DateTime.Now.Year - 1) + "-01-01"));
        //=====================fecha actual=============================


        public ActionResult ListaLlamadasSalientes(string paraPaginacion, string filtro, string FechaInicial, string FechaFinal, int? page)
        {



            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            ViewData["usuario"] = xyt.xyp_SelUsuarios().ToList();

            ////Select para dropdowlist LlamadaEntrante
            //ViewData["llamadaentrante"] = (from row in xyt.xyp_NumberAmountsByOutSubscriber(Convert.ToString(anioActual) + "-01-01", fecha_actual, "", "")
            //                               orderby row.Ide_NumberTarget ascending
            //                               select row).Distinct().ToList();

            ViewData["llamadasaliente"] = (from row in xyt.xy_calls where row.Ide_CallType == 2 && row.Fec_Date >= fechaIni
                                           && row.Fec_Date <= fecha_actual && row.Ide_NumberTarget != "" && row.Num_CallEffectiveDuration > 0
                                           group row.Ide_NumberTarget by row.Ide_NumberTarget into NumberTargetGroup
                                           orderby NumberTargetGroup.Key ascending
                                           select NumberTargetGroup.Key).ToList();



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
                List<xyp_NumberAmountsByOutSubscriber_Result> lista;

                if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
                    lista = repLlamSalien.ObtenerListaLlamadasSalientes(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["llamadaentrante"].ToString()).ToList();
                else
                    lista = repLlamSalien.ObtenerListaLlamadasSalientes("", "", "", "").ToList();

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
                log.EscribaLog("REPORTE", "Action:ListaLlamadasSalientes " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }
        public ActionResult ListaLlamadasSalientes_(string FechaInicial, string FechaFinal, string[] usuarioId, string[] numentrante, int? page)
        {
            string user, llamEnt;


            //=====================Procesar usuario=============================
            string usuario = "";
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


            ViewData["usuario"] = xyt.xyp_SelUsuarios().ToList();

            ////Select para dropdowlist LlamadaEntrante
            //ViewData["llamadaentrante"] = (from row in xyt.xyp_NumberAmountsByOutSubscriber(Convert.ToString(anioActual) + "-01-01", fecha_actual, "", "")
            //                               orderby row.Ide_NumberTarget ascending
            //                               select row).Distinct().ToList();

            ViewData["llamadasaliente"] = (from row in xyt.xy_calls where row.Ide_CallType == 2 && row.Fec_Date >= fechaIni
                                           && row.Fec_Date <= fecha_actual && row.Ide_NumberTarget != "" && row.Num_CallEffectiveDuration > 0
                                           group row.Ide_NumberTarget by row.Ide_NumberTarget into NumberTargetGroup
                                           orderby NumberTargetGroup.Key ascending
                                           select NumberTargetGroup.Key).ToList();

            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            if (FechaInicial == "" || FechaFinal == "")
            {
                return RedirectToAction("ListaLlamadasSalientes", "ReporteLlamadasSalientes");
            }
            else
            {
                List<xyp_NumberAmountsByOutSubscriber_Result> lista = repLlamSalien.ObtenerListaLlamadasSalientes(FechaInicial, FechaFinal, user, llamEnt).ToList();
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
                return View("ListaLlamadasSalientes", lista.ToPagedList(pageIndex, pageSize));
            }
        }

        public ActionResult Reportes(string opcion)
        {
            if (Session["FechaInicial"] == null || Session["FechaFinal"] == null)
                return View("ListaLlamadasSalientes", new List<xyp_NumberAmountsByOutSubscriber_Result>().ToPagedList(1, 1));
            else
                return ReporteFormato(opcion, "ReporteLlamadasSalientes", "ObtenerListaLlamadasSalientes",
                    Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["llamadaentrante"].ToString());
        }
    }
}