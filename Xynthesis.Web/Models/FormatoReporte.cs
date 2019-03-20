using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xynthesis.Modelo;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System.IO;
using Xynthesis.AccesoDatos;
using Xynthesis.Utilidades;
using System.Web.Mvc;

namespace Xynthesis.Web.Models
{
    public class FormatoReporte : Controller
    {

        //string FechaInicial { get; set; }
        //string FechaFinal { get; set; }
        //List<string> lisUsuarios { get; set; };
        //List<string> lisAreas { get; set; };
        //List<string> lisCoberturas { get; set; };
        //List<string> lisOperadores { get; set; };

        public ActionResult Reportes_(string opcion, string fecini, string fecfin, string nomRpt)
        {

            // if (fecini == null || fecfin == null)
            // {
            //return RedirectToAction("ListaPromedioLlamadaHora", "RepPromedioLlamadasHora");
            //    return View();
            // }
            //else
            // {

            if (Session["FechaInicial"] != null && Session["FechaFinal"] != null)
            {
                var FechaInicial = Session["FechaInicial"].ToString();
                var FechaFinal = Session["FechaFinal"].ToString();
                return (ReporteFormato(opcion, FechaInicial, FechaFinal, nomRpt));
            }
            else
                return (ReporteFormato(opcion, null, null, nomRpt));


            // }
        }
        public ActionResult ReporteFormato(string extension, string nomRpt, string namestore, params object[] argumentos)
        {

            if (extension.ToUpper().Equals("EXCEL"))
                extension = "xls";
            else
               if (extension.ToUpper().Equals("WORD"))
                extension = "doc";


            Stream file = Xynthesis.Reportes.ExportacionReportes.GenerarRptArchivo(extension, namestore, nomRpt, argumentos);

            return File(file, "application/" + extension, nomRpt + "." + extension);


        }
        public ActionResult ReporteFormato(string opcion, string FechaInicial, string FechaFinal, string nomRpt)
        {
            string extension = "";
            if (opcion.Equals("pdf"))
                extension = "pdf";
            else
                if (opcion.Equals("excel"))
                extension = "xls";
            else
                extension = "doc";

            //}
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), nomRpt + ".rpt"));
            if (nomRpt.Equals("RepPromedioLlamadasHora"))
            {
                ADRepPromedioLlamadasHora prollama = new ADRepPromedioLlamadasHora();
                List<xyp_RepPromedioLlamadasHora_Result> lista = new List<xyp_RepPromedioLlamadasHora_Result>();
                if (FechaInicial != null && FechaInicial != null)
                    lista = prollama.ObtenerPromedioLlamadasHora(FechaInicial, FechaFinal, "", "", "").ToList();
                else
                    lista = prollama.ObtenerPromedioLlamadasHora(null, null, null, null, null).ToList();
                rd.SetDataSource(lista);
            }
            else
            {
                if (nomRpt.Equals("CoberturaLlamadas"))
                {
                    ADReporteCoberturaLlamadas coberll = new ADReporteCoberturaLlamadas();
                    List<xyp_ReceiveCalls_Result> lista = new List<xyp_ReceiveCalls_Result>();
                    if (FechaInicial != null && FechaInicial != null)
                        lista = coberll.ObtenerCoberturaLlamadas(FechaInicial, FechaFinal, "", "").ToList();
                    else
                        lista = coberll.ObtenerCoberturaLlamadas(null, null, null, null).ToList();
                    rd.SetDataSource(lista);
                }
                else
                {
                    if (nomRpt.Equals("ReporteGraficoEstadisticoLlamadasEntrantesSalientes"))
                    {
                        ADReporteGraficoLlamadasEntrantesSalientes repLlamEntrSali = new ADReporteGraficoLlamadasEntrantesSalientes();
                        List<xyp_RepReceiveCallsLlamEntranSalien_Result> lista = new List<xyp_RepReceiveCallsLlamEntranSalien_Result>();
                        lista = repLlamEntrSali.ObtenerListaLlamadasEntrantesSalientes(FechaInicial, FechaFinal, "", "").ToList();
                        rd.SetDataSource(lista);
                    }
                    else
                    {
                        if (nomRpt.Equals("ReporteGraficoLlamadasEntrantesSalientesDuracion"))
                        {
                            ADReporteGraficoLlamadasEntrantesSalientesDuracion repGrafTiemp = new ADReporteGraficoLlamadasEntrantesSalientesDuracion();
                            List<xyp_RepGrafNumberAmountsBySubscriber_Result> lista = new List<xyp_RepGrafNumberAmountsBySubscriber_Result>();
                            lista = repGrafTiemp.ObtenerListaLlamadasEntrantesSalientesDuracion(FechaInicial, FechaFinal, "", "").ToList();
                            rd.SetDataSource(lista);
                        }
                        else
                        {
                            if (nomRpt.Equals("ReporteGraficoTiempoPromedioAntesDeContestarNuevo"))
                            {
                                ADReporteGraficoTiempoPromedioAntesDeContestar repGrafTiemp = new ADReporteGraficoTiempoPromedioAntesDeContestar();
                                List<xyp_ReceiveCallsTiempoPromedio_Result> lista = new List<xyp_ReceiveCallsTiempoPromedio_Result>();
                                lista = repGrafTiemp.ObtenerListaTiempoPromedioAntesContestarResumido(FechaInicial, FechaFinal, "", "").ToList();
                                rd.SetDataSource(lista);
                            }
                            else
                            {
                                if (nomRpt.Equals("ReporteConsolidadoCoberturaLLamadas"))
                                {
                                    ADReporteConsolidadoCoberturaLLamadas coberll = new ADReporteConsolidadoCoberturaLLamadas();
                                    List<xyp_RepConsolidadoCoberturaLLamadas_Result> lista = new List<xyp_RepConsolidadoCoberturaLLamadas_Result>();
                                    lista = coberll.ObtenerConsolidadoCoberturaLlamadas(FechaInicial, FechaFinal, "", "").ToList();
                                    rd.SetDataSource(lista);
                                }
                                else
                                {
                                    if (nomRpt.Equals("ReporteClaseDeLlamadas"))
                                    {
                                        ADReporteClaseDeLlamadas repClasLlam = new ADReporteClaseDeLlamadas();
                                        List<xyp_CallTypesAmountReport_Result> lista = new List<xyp_CallTypesAmountReport_Result>();
                                        lista = repClasLlam.ObtenerListaClaseDeLlamadas(FechaInicial, FechaFinal).ToList();
                                        rd.SetDataSource(lista);
                                    }
                                    else
                                    {
                                        if (nomRpt.Equals("ConsumosPersonales"))
                                        {

                                            ADReporteConsumosPersonales Consper = new ADReporteConsumosPersonales();
                                            List<xyp_SelConsumeByExtensionAndUser_Result> lista = new List<xyp_SelConsumeByExtensionAndUser_Result>();
                                            if (FechaInicial != null && FechaInicial != null)
                                                lista = Consper.ObtenerConsumosPersonales(FechaInicial, FechaFinal, "", "", "", "", "").ToList();
                                            else
                                                lista = Consper.ObtenerConsumosPersonales(null, null, null, null, null, null, null).ToList();
                                            rd.SetDataSource(lista);
                                        }
                                        else
                                        {
                                            if (nomRpt.Equals("ConsumoPorCentrosCostos"))
                                            {
                                                ADReporteConsumosPorCentroCost consuCC = new ADReporteConsumosPorCentroCost();
                                                List<xyp_SelConsumeByCostCenter_Result> lista = new List<xyp_SelConsumeByCostCenter_Result>();
                                                lista = consuCC.ObtenerConsumosPorcentroCostos(FechaInicial, FechaFinal, "", "").ToList();
                                                rd.SetDataSource(lista);
                                            }
                                            else
                                            {
                                                if (nomRpt.Equals("FrecuenciaLlamadas"))
                                                {
                                                    ADOReporteFrecuenciadellamadas frlla = new ADOReporteFrecuenciadellamadas();
                                                    List<xyp_SelFrequentExtensionNumber_Result> lista = new List<xyp_SelFrequentExtensionNumber_Result>();
                                                    lista = frlla.ObtenerFrecuenciaDeLlamadas(FechaInicial, FechaFinal).ToList();
                                                    rd.SetDataSource(lista);
                                                }
                                                else
                                                {
                                                    if (nomRpt.Equals("ReporteGraficoEstadisticoLlamadasEntrantesSalientes"))
                                                    {
                                                        ADReporteGraficoLlamadasEntrantesSalientes repLlamEntrSali = new ADReporteGraficoLlamadasEntrantesSalientes();
                                                        List<xyp_RepReceiveCallsLlamEntranSalien_Result> lista = new List<xyp_RepReceiveCallsLlamEntranSalien_Result>();
                                                        lista = repLlamEntrSali.ObtenerListaLlamadasEntrantesSalientes(FechaInicial, FechaFinal, "", "").ToList();
                                                        rd.SetDataSource(lista);
                                                    }
                                                    else
                                                    {
                                                        if (nomRpt.Equals("ReporteGraficoLlamadasEntrantesSalientesDuracion"))
                                                        {
                                                            ADReporteGraficoLlamadasEntrantesSalientesDuracion repGrafTiemp = new ADReporteGraficoLlamadasEntrantesSalientesDuracion();
                                                            List<xyp_RepGrafNumberAmountsBySubscriber_Result> lista = new List<xyp_RepGrafNumberAmountsBySubscriber_Result>();
                                                            lista = repGrafTiemp.ObtenerListaLlamadasEntrantesSalientesDuracion(FechaInicial, FechaFinal, "", "").ToList();
                                                            rd.SetDataSource(lista);
                                                        }
                                                        else
                                                        {
                                                            if (nomRpt.Equals("ReporteGraficoTiempoPromedioAntesDeContestarNuevo"))
                                                            {
                                                                ADReporteGraficoTiempoPromedioAntesDeContestar repGrafTiemp = new ADReporteGraficoTiempoPromedioAntesDeContestar();
                                                                List<xyp_ReceiveCallsTiempoPromedio_Result> lista = new List<xyp_ReceiveCallsTiempoPromedio_Result>();
                                                                lista = repGrafTiemp.ObtenerListaTiempoPromedioAntesContestarResumido(FechaInicial, FechaFinal, "", "").ToList();
                                                                rd.SetDataSource(lista);
                                                            }
                                                            else
                                                            {
                                                                if (nomRpt.Equals("HistoriaConsumos"))
                                                                {
                                                                    ADReporteHistoriaConsumo histoConsu = new ADReporteHistoriaConsumo();
                                                                    List<xyp_SelConsumeByHistory_Result> lista = new List<xyp_SelConsumeByHistory_Result>();
                                                                    lista = histoConsu.ObtenerHistoriaConsumos(FechaInicial, FechaFinal, "").ToList();
                                                                    rd.SetDataSource(lista);
                                                                }
                                                                else
                                                                {
                                                                    if (nomRpt.Equals("ReporteLlamadaInicioFinDia"))
                                                                    {
                                                                        ADReporteInicioFinActividades repIniFin = new ADReporteInicioFinActividades();
                                                                        List<xyp_SelActivityFirstAndLast_Result> lista = new List<xyp_SelActivityFirstAndLast_Result>();
                                                                        lista = repIniFin.ObtenerListaInicioFin(FechaInicial, FechaFinal, "").ToList();
                                                                        rd.SetDataSource(lista);
                                                                    }
                                                                    else
                                                                    {
                                                                        if (nomRpt.Equals("ReporteLlamadasEntrantes"))
                                                                        {
                                                                            //ADReporteLlamadasEntrantes repLlamEntra = new ADReporteLlamadasEntrantes();
                                                                            //List<xyp_NumberAmountsByOutSubscriber_Result> lista = new List<xyp_NumberAmountsByOutSubscriber_Result>();
                                                                            //lista = repLlamEntra.ObtenerListaLlamadasEntrantes(FechaInicial, FechaFinal).ToList();
                                                                            //rd.SetDataSource(lista);
                                                                        }
                                                                        else
                                                                        {
                                                                            if (nomRpt.Equals("ReporteLlamadasEntrantesSalientes"))
                                                                            {
                                                                                ADReporteLlamadasEntrantesSalientes repLlamEntrSali = new ADReporteLlamadasEntrantesSalientes();
                                                                                List<xyp_RepReceiveCallsLlamEntranSalien_Result> lista = new List<xyp_RepReceiveCallsLlamEntranSalien_Result>();
                                                                                lista = repLlamEntrSali.ObtenerListaLlamadasEntrantesSalientes(FechaInicial, FechaFinal,"", "").ToList();
                                                                                rd.SetDataSource(lista);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (nomRpt.Equals("ReporteLlamadasRecibidasTransferencias"))
                                                                                {
                                                                                    ADReporteLlamadasRecibidasTransferidas repLlamRecibTransf = new ADReporteLlamadasRecibidasTransferidas();
                                                                                    List<xyp_ReceiveAndTransferCalls_Result> lista = new List<xyp_ReceiveAndTransferCalls_Result>();
                                                                                    lista = repLlamRecibTransf.ObtenerListaLlamadasRecibidasTransferidas(FechaInicial, FechaFinal, "").ToList();
                                                                                    rd.SetDataSource(lista);
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (nomRpt.Equals("ReporteLlamadasSalientes"))
                                                                                    {
                                                                                        ADReporteLlamadasSalientes repLlamSalien = new ADReporteLlamadasSalientes();
                                                                                        List<xyp_NumberAmountsByOutSubscriber_Result> lista = new List<xyp_NumberAmountsByOutSubscriber_Result>();
                                                                                        lista = repLlamSalien.ObtenerListaLlamadasSalientes(FechaInicial, FechaFinal, "","").ToList();
                                                                                        rd.SetDataSource(lista);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (nomRpt.Equals("NumeroMasMarcado"))
                                                                                        {
                                                                                            ADReporteNumeroMasMarcado frlla = new ADReporteNumeroMasMarcado();
                                                                                            List<xyp_SelDialedNumber_Result> lista = new List<xyp_SelDialedNumber_Result>();
                                                                                            lista = frlla.ObtenerNumeroMasMarcado(FechaInicial, FechaFinal, "", "").ToList();
                                                                                            rd.SetDataSource(lista);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            if (nomRpt.Equals("ReportePorPeriodoTiempo"))
                                                                                            {
                                                                                                ADReportePorPeriodoTiempo repPorPerioTiempo = new ADReportePorPeriodoTiempo();
                                                                                                List<xyp_SelCallAmountsBySubscriber_Result> lista = new List<xyp_SelCallAmountsBySubscriber_Result>();
                                                                                                lista = repPorPerioTiempo.ObtenerListaPorPeriodoTiempo(FechaInicial, FechaFinal,"").ToList();
                                                                                                rd.SetDataSource(lista);
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                if (nomRpt.Equals("ReporteTiempoDedicado"))
                                                                                                {
                                                                                                    ADReporteTiempoDedicado rtiemdedi = new ADReporteTiempoDedicado();
                                                                                                    List<xyp_CallAmountByContraparte_Result> lista = new List<xyp_CallAmountByContraparte_Result>();
                                                                                                    lista = rtiemdedi.ObtenerListaTiempoDedicado(FechaInicial, FechaFinal, "").ToList();
                                                                                                    rd.SetDataSource(lista);
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    if (nomRpt.Equals("ReporteTiempoPromedioAntesContestar"))
                                                                                                    {
                                                                                                        ADReporteTiempoPromedioAntesContestar repTiemPromAntContest = new ADReporteTiempoPromedioAntesContestar();
                                                                                                        List<xyp_ReceiveCallsTiempoPromedio_Result> lista = new List<xyp_ReceiveCallsTiempoPromedio_Result>();
                                                                                                        lista = repTiemPromAntContest.ObtenerListaTiempoPromedioAntesContestar(FechaInicial, FechaFinal, "", "").ToList();
                                                                                                        rd.SetDataSource(lista);
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        if (nomRpt.Equals("TopLlamadaCampeonaXCosto"))
                                                                                                        {
                                                                                                            ADReporteTopLlamadaCampeonaCC TllamadaCCC = new ADReporteTopLlamadaCampeonaCC();
                                                                                                            List<xyp_SelDetailChampCallCost_Result> lista = new List<xyp_SelDetailChampCallCost_Result>();
                                                                                                            lista = TllamadaCCC.ObtenerTopLlamadaCampeonaCC(FechaInicial, FechaFinal, "", "", "").ToList();
                                                                                                            rd.SetDataSource(lista);
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            if (nomRpt.Equals("ReporteLlamadasAbiertasCerradas"))
                                                                                                            {
                                                                                                                ADReporteLlamadasAbiertasCerradas repLlamAbierCerra = new ADReporteLlamadasAbiertasCerradas();
                                                                                                                List<xyp_RepCallOpenAndClosed_Result> lista = new List<xyp_RepCallOpenAndClosed_Result>();
                                                                                                                if (FechaInicial != null && FechaInicial != null)
                                                                                                                    lista = repLlamAbierCerra.ObtenerListaLlamadasAbiertasCerradas(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["hora"].ToString(), "","").ToList();
                                                                                                                else
                                                                                                                    lista = repLlamAbierCerra.ObtenerListaLlamadasAbiertasCerradas(null, null, null, null, null).ToList();

                                                                                                                rd.SetDataSource(lista);
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                if (nomRpt.Equals("LlamadaCampeonaXDuracion"))
                                                                                                                {
                                                                                                                    ADLlamadaCampeonaDuracion repLlamAbierCerra = new ADLlamadaCampeonaDuracion();
                                                                                                                    List<xyp_SelDetailChampCallDuration_Result> lista = new List<xyp_SelDetailChampCallDuration_Result>();
                                                                                                                    if (FechaInicial != null && FechaInicial != null)
                                                                                                                        lista = repLlamAbierCerra.ObtenerLlamadaCampeonaDuracion(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), "", "").ToList();
                                                                                                                    else
                                                                                                                        lista = repLlamAbierCerra.ObtenerLlamadaCampeonaDuracion(null, null, null, null).ToList();

                                                                                                                    rd.SetDataSource(lista);
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    if (nomRpt.Equals("RepTarificacionEntraSalieTrans"))
                                                                                                                    {
                                                                                                                        ADRepTarificacionEntraSalieTrans repTar = new ADRepTarificacionEntraSalieTrans();
                                                                                                                        List<xyp_RepTarificacionEntraSalieTrans_Result> lista = new List<xyp_RepTarificacionEntraSalieTrans_Result>();
                                                                                                                        if (FechaInicial != null && FechaInicial != null)
                                                                                                                            lista = repTar.ObtenerTarificacion(Session["FechaInicial"].ToString(), Session["FechaFinal"].ToString(), Session["usuarios"].ToString(), Session["areas"].ToString(), Session["sucursales"].ToString()).ToList();
                                                                                                                        else
                                                                                                                            lista = repTar.ObtenerTarificacion(null, null, null, null, null).ToList();

                                                                                                                        rd.SetDataSource(lista);
                                                                                                                    }

                                                                                                                }
                                                                                                            }

                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }


            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {

                Stream stream;
                //=====================Inicio para Exportar a PDF============================================
                if (extension.Equals("pdf"))
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                else
                    if (extension.Equals("xls"))
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                else
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.WordForWindows);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/" + extension, nomRpt + "." + extension);
                //=====================Fin para Exportar a PDF===============================================
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }



    }
}
