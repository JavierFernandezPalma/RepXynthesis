using CrystalDecisions.CrystalReports.Engine;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xynthesis.AccesoDatos;
using Xynthesis.Modelo;
using Xynthesis.Utilidades;

namespace Xynthesis.Web.Controllers
{
    public class ReportesMomentoController : Controller
    {
        Utilidades.LogXynthesis log = new LogXynthesis();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        xynthesisEntities xyt = new xynthesisEntities();
        public FileStreamResult rep;
        public int num;
        List<System.Collections.IList> listReportes = new List<System.Collections.IList>();
        List<string> nombresReportes = new List<string>();

        // GET: ReportesMomento
        public ActionResult ReportesMomentaneos()
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            //ViewData["confProgr"] = xyt.xy_reportes.ToList();
            var lista = xyt.xy_reportes.ToList();
            return View(lista);
        }

        [HttpPost]
        public ActionResult ReportesMomentaneos(List<string> reportes, string formato, string FechaInicial, string FechaFinal)
        {
            return reporte(Convert.ToInt32(reportes[0]), listaProc(reportes, FechaInicial, FechaFinal), formato, FechaInicial, FechaFinal, nomrpt(reportes));
        }



        public FileResult reporte(int id, List<System.Collections.IList> reportesImprimir, string formato, string FechaInicial, string FechaFinal, List<string> nomrpt)
        {
            ZipFile zip = new ZipFile();
            MemoryStream output = new MemoryStream();
            ReportDocument rd = new ReportDocument();
            string nombreDelZip = "Reportes_Xyntesis.zip";

            try
            {


                    for (int i = 0; i < reportesImprimir.Count; i++)
                    {
                        rd.Load(Path.Combine(Server.MapPath("~/Reportes"), nomrpt[i] + ".rpt"));
                        rd.SetDataSource(reportesImprimir[i]);


                        if (formato == "pdf")
                        {
                            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                            stream.Seek(0, SeekOrigin.Begin);
                            
                            if(reportesImprimir.Count == 1)
                            {
                                Response.Buffer = false;
                                Response.ClearContent();
                                Response.ClearHeaders();

                                return File(stream, "application/pdf", nomrpt[i] + ".pdf");
                            }

                            zip.AddEntry(nomrpt[i] + ".pdf", stream);

                        }
                        else
                        {
                            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                            stream.Seek(0, SeekOrigin.Begin);

                            if (reportesImprimir.Count == 1)
                            {
                                Response.Buffer = false;
                                Response.ClearContent();
                                Response.ClearHeaders();

                                return File(stream, "application/xls", nomrpt[i] + ".xls");
                            }

                        zip.AddEntry(nomrpt[i] + ".xls", stream);

                        }

                    }

                    zip.Save(output);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

            }
            catch (Exception ex)
            {
                throw ex;
            }


            return File(output.ToArray(), "application/zip", nombreDelZip);
        }


        public void Comprime(IEnumerable<string> files)
        {
            //using (ZipFile zf = new ZipFile())
            //{
            //    zf.AddFiles(_files, false, "");
            //    zf.Save(@"d:\prueba.zip");
            //}

        }

        //Obtener return Lista
        public List<System.Collections.IList> listaProc(List<string> listadoReportes, string FechaInicial, string FechaFinal)
        {

            for (int i = 0; i < listadoReportes.Count; i++)

            {
                int id = Convert.ToInt32(listadoReportes[i]);

                if (id == 1) //Reporte ReportePorPeriodoTiempo
                {
                    ADReportePorPeriodoTiempo repPorPerioTiempo = new ADReportePorPeriodoTiempo();
                    listReportes.Add(repPorPerioTiempo.ObtenerListaPorPeriodoTiempo(FechaInicial, FechaFinal, "").ToList());


                }
                else if (id == 2) //Reporte ReporteLlamadasEntrantes
                {
                    ADReporteLlamadasEntrantes repLlamadaEntrante = new ADReporteLlamadasEntrantes();
                    listReportes.Add(repLlamadaEntrante.ObtenerListaLlamadasEntrantes(FechaInicial, FechaFinal, "", "", "").ToList());

                }
                else if (id == 3) //Reporte ReporteLlamadasSalientes
                {
                    ADReporteLlamadasSalientes repPorPerioTiempo = new ADReporteLlamadasSalientes();
                    listReportes.Add(repPorPerioTiempo.ObtenerListaLlamadasSalientes(FechaInicial, FechaFinal, "", "").ToList());

                }
                else if (id == 4) //Reporte ReporteTiempoDedicado
                {
                    ADReporteTiempoDedicado repLlamadaEntrante = new ADReporteTiempoDedicado();
                    listReportes.Add(repLlamadaEntrante.ObtenerListaTiempoDedicado(FechaInicial, FechaFinal, "").ToList());

                }
                else if (id == 5) //Llamadas ReporteLlamadasEntrantesSalientes
                {
                    ADReporteGraficoLlamadasEntrantesSalientes repPorPerioTiempo = new ADReporteGraficoLlamadasEntrantesSalientes();
                    listReportes.Add(repPorPerioTiempo.ObtenerListaLlamadasEntrantesSalientes(FechaInicial, FechaFinal, "", "").ToList());

                }
                else if (id == 6) //Llamadas ReporteLlamadasRecibidasTransferencias
                {
                    ADReporteLlamadasRecibidasTransferidas repLlamadaEntrante = new ADReporteLlamadasRecibidasTransferidas();
                    listReportes.Add(repLlamadaEntrante.ObtenerListaLlamadasRecibidasTransferidas(FechaInicial, FechaFinal, "").ToList());

                }
                else if (id == 7) //ReporteTiempoPromedioAntesContestar
                {
                    ADReporteTiempoPromedioAntesContestar repPorPerioTiempo = new ADReporteTiempoPromedioAntesContestar();
                    listReportes.Add(repPorPerioTiempo.ObtenerListaTiempoPromedioAntesContestar(FechaInicial, FechaFinal, "", "").ToList());

                }
                else if (id == 8) //Reporte FrecuenciaLlamadas
                {
                    ADOReporteFrecuenciadellamadas repLlamadaEntrante = new ADOReporteFrecuenciadellamadas();
                    listReportes.Add(repLlamadaEntrante.ObtenerFrecuenciaDeLlamadas(FechaInicial, FechaFinal).ToList());

                }
                else if (id == 9) //Reporte RepPromedioLlamadasHora
                {
                    ADRepPromedioLlamadasHora repPorPerioTiempo = new ADRepPromedioLlamadasHora();
                    listReportes.Add(repPorPerioTiempo.ObtenerPromedioLlamadasHora(FechaInicial, FechaFinal, "", "", "").ToList());

                }
                else if (id == 10) //Reporte TopLlamadaCampeonaXCosto
                {
                    ADReporteTopLlamadaCampeonaCC repLlamadaEntrante = new ADReporteTopLlamadaCampeonaCC();
                    listReportes.Add(repLlamadaEntrante.ObtenerTopLlamadaCampeonaCC(FechaInicial, FechaFinal, "", "", "").ToList());

                }
                else if (id == 11) //Reporte LlamadaCampeonaXDuracion
                {
                    ADLlamadaCampeonaDuracion repPorPerioTiempo = new ADLlamadaCampeonaDuracion();
                    listReportes.Add(repPorPerioTiempo.ObtenerLlamadaCampeonaDuracion(FechaInicial, FechaFinal, "", "").ToList());

                }
                else if (id == 12) //Reporte NumeroMasMarcado
                {
                    ADReporteNumeroMasMarcado repLlamadaEntrante = new ADReporteNumeroMasMarcado();
                    listReportes.Add(repLlamadaEntrante.ObtenerNumeroMasMarcado(FechaInicial, FechaFinal, "", "").ToList());

                }
                else if (id == 13) //Reporte FrecuenciaLlamadas
                {
                    ADOReporteFrecuenciadellamadas repPorPerioTiempo = new ADOReporteFrecuenciadellamadas();
                    listReportes.Add(repPorPerioTiempo.ObtenerFrecuenciaDeLlamadas(FechaInicial, FechaFinal).ToList());

                }
                else if (id == 14) //Reporte ConsumosPersonales
                {
                    ADReporteConsumosPersonales repLlamadaEntrante = new ADReporteConsumosPersonales();
                    listReportes.Add(repLlamadaEntrante.ObtenerConsumosPersonales(FechaInicial, FechaFinal, "", "", "", "", "").ToList()); //un parametro EXTENSION mas +

                }
                else if (id == 15) //Reporte ConsumoPorCentrosCostos
                {
                    ADReporteConsumosPorCentroCost repLlamadaEntrante = new ADReporteConsumosPorCentroCost();
                    listReportes.Add(repLlamadaEntrante.ObtenerConsumosPorcentroCostos(FechaInicial, FechaFinal, "", "").ToList());

                }
                else if (id == 16) //Reporte CoberturaLlamadas
                {
                    ADReporteCoberturaLlamadas repPorPerioTiempo = new ADReporteCoberturaLlamadas();
                    listReportes.Add(repPorPerioTiempo.ObtenerCoberturaLlamadas(FechaInicial, FechaFinal, "", "").ToList());

                }
                else if (id == 17) //Reporte HistoriaConsumos
                {
                    ADReporteHistoriaConsumo repLlamadaEntrante = new ADReporteHistoriaConsumo();
                    listReportes.Add(repLlamadaEntrante.ObtenerHistoriaConsumos(FechaInicial, FechaFinal, "").ToList());

                }
                else if (id == 18) //Reporte ReporteGraficoTiempoPromedioAntesDeContestarNuevo
                {
                    ADReporteGraficoTiempoPromedioAntesDeContestar repPorPerioTiempo = new ADReporteGraficoTiempoPromedioAntesDeContestar();
                    listReportes.Add(repPorPerioTiempo.ObtenerListaTiempoPromedioAntesContestarResumido(FechaInicial, FechaFinal, "", "").ToList());

                }
                else if (id == 19) //Reporte ReporteGraficoEstadisticoLlamadasEntrantesSalientes
                {
                    ADReporteGraficoLlamadasEntrantesSalientes repLlamadaEntrante = new ADReporteGraficoLlamadasEntrantesSalientes();
                    listReportes.Add(repLlamadaEntrante.ObtenerListaLlamadasEntrantesSalientes(FechaInicial, FechaFinal, "", "").ToList());

                }
                else if (id == 20) //Reporte ReporteGraficoLlamadasEntrantesSalientesDuracion
                {
                    ADReporteGraficoLlamadasEntrantesSalientesDuracion repPorPerioTiempo = new ADReporteGraficoLlamadasEntrantesSalientesDuracion();
                    listReportes.Add(repPorPerioTiempo.ObtenerListaLlamadasEntrantesSalientesDuracion(FechaInicial, FechaFinal, "", "").ToList());

                }
                else if (id == 21) //Reporte ReporteConsolidadoCoberturaLLamadas
                {
                    ADReporteConsolidadoCoberturaLLamadas repLlamadaEntrante = new ADReporteConsolidadoCoberturaLLamadas();
                    listReportes.Add(repLlamadaEntrante.ObtenerConsolidadoCoberturaLlamadas(FechaInicial, FechaFinal, "", "").ToList());

                }
                else if (id == 22) //Reporte RepPromedioLlamadasHora
                {
                    ADRepPromedioLlamadasHora repPorPerioTiempo = new ADRepPromedioLlamadasHora();
                    listReportes.Add(repPorPerioTiempo.ObtenerPromedioLlamadasHora(FechaInicial, FechaFinal, "", "", "").ToList()); //====================================

                }
                else if (id == 23) //Reporte RepTarificacionEntraSalieTrans
                {
                    ADRepTarificacionEntraSalieTrans repLlamadaEntrante = new ADRepTarificacionEntraSalieTrans();
                    listReportes.Add(repLlamadaEntrante.ObtenerTarificacion(FechaInicial, FechaFinal, "", "", "").ToList());

                }
                else if (id == 24) //Reporte ReporteLlamadasAbiertasCerradas
                {
                    ADReporteLlamadasAbiertasCerradas repLlamadaEntrante = new ADReporteLlamadasAbiertasCerradas();
                    listReportes.Add(repLlamadaEntrante.ObtenerListaLlamadasAbiertasCerradas(FechaInicial, FechaFinal, "", "", "").ToList());

                }
                else if (id == 25)  //Reporte ReporteSabanaCalls
                {
                    ADReporteSabanaCalls repSabanaCalls = new ADReporteSabanaCalls();
                    listReportes.Add(repSabanaCalls.ObtenerListaSabanaCalls(FechaInicial, FechaFinal).ToList());

                }

            }

            return listReportes;


        }

        public List<string> nomrpt(List<string> idReporte)
        {

            for (int i = 0; i < idReporte.Count; i++)

            {
                int id = Convert.ToInt32(idReporte[i]);

                if (id == 1)
                {
                    string nom = "ReportePorPeriodoTiempo";
                    nombresReportes.Add(nom);
                }
                else if (id == 2)
                {
                    string nom = "ReporteLlamadasEntrantes";
                    nombresReportes.Add(nom);
                }
                else if (id == 3)
                {
                    string nom = "ReporteLlamadasSalientes";
                    nombresReportes.Add(nom);
                }
                else if (id == 4)
                {
                    string nom = "ReporteTiempoDedicado";
                    nombresReportes.Add(nom);
                }
                else if (id == 5)
                {
                    string nom = "ReporteLlamadasEntrantesSalientes";
                    nombresReportes.Add(nom);
                }
                else if (id == 6)
                {
                    string nom = "ReporteLlamadasRecibidasTransferencias";
                    nombresReportes.Add(nom);
                }
                else if (id == 7)
                {
                    string nom = "ReporteTiempoPromedioAntesContestar";
                    nombresReportes.Add(nom);
                }
                else if (id == 8)
                {
                    string nom = "FrecuenciaLlamadas";
                    nombresReportes.Add(nom);
                }
                else if (id == 9)
                {
                    string nom = "RepPromedioLlamadasHora";
                    nombresReportes.Add(nom);
                }
                else if (id == 10)
                {
                    string nom = "TopLlamadaCampeonaXCosto";
                    nombresReportes.Add(nom);
                }
                else if (id == 11)
                {
                    string nom = "LlamadaCampeonaXDuracion";
                    nombresReportes.Add(nom);
                }
                else if (id == 12)
                {
                    string nom = "NumeroMasMarcado";
                    nombresReportes.Add(nom);
                }
                else if (id == 13)
                {
                    string nom = "FrecuenciaLlamadas";
                    nombresReportes.Add(nom);
                }
                else if (id == 14)
                {
                    string nom = "ConsumosPersonales";
                    nombresReportes.Add(nom);
                }
                else if (id == 15)
                {
                    string nom = "ConsumoPorCentrosCostos";
                    nombresReportes.Add(nom);
                }
                else if (id == 16)
                {
                    string nom = "CoberturaLlamadas";
                    nombresReportes.Add(nom);
                }
                else if (id == 17)
                {
                    string nom = "HistoriaConsumos";
                    nombresReportes.Add(nom);
                }
                else if (id == 18)
                {
                    string nom = "ReporteGraficoTiempoPromedioAntesDeContestarNuevo";
                    nombresReportes.Add(nom);
                }
                else if (id == 19)
                {
                    string nom = "ReporteGraficoEstadisticoLlamadasEntrantesSalientes";
                    nombresReportes.Add(nom);
                }
                else if (id == 20)
                {
                    string nom = "ReporteGraficoLlamadasEntrantesSalientesDuracion";
                    nombresReportes.Add(nom);
                }
                else if (id == 21)
                {
                    string nom = "ReporteConsolidadoCoberturaLLamadas";
                    nombresReportes.Add(nom);
                }
                else if (id == 22)
                {
                    string nom = "RepPromedioLlamadasHora";
                    nombresReportes.Add(nom);
                }
                else if (id == 23)
                {
                    string nom = "RepTarificacionEntraSalieTrans";
                    nombresReportes.Add(nom);
                }
                else if (id == 24)
                {
                    string nom = "ReporteLlamadasAbiertasCerradas";
                    nombresReportes.Add(nom);
                }
                else if (id == 25)
                {
                    string nom = "ReporteSabanaCalls";
                    nombresReportes.Add(nom);
                }

            }

            return nombresReportes;

        }
    }
}