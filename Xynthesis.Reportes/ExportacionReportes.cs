using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xynthesis.AccesoDatos;
using Xynthesis.Modelo;
using Xynthesis.Utilidades;

namespace Xynthesis.Reportes
{
    public class ExportacionReportes
    {
        public static IEnumerable ObtenerReporte(string namestore, params object[] argumentos)
        {
            ADReportes oReporte = new ADReportes();
            MethodInfo method = oReporte.GetType().GetMethod(namestore);
            method.GetParameters();
            List<object> parameters = new List<object>();
            foreach (var p in argumentos)
            {
                  parameters.Add(p as string);
            }

            var param = parameters.ToArray();

            object result = method.Invoke(oReporte, param);

            IEnumerable enumo = result as IEnumerable;

            return enumo;

           
        }

        public static Stream GenerarRptArchivo(string extension, string namestore, string reporte, params object[] argumentos)
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(FuncionesComunes.ObtenerValorConfiguracion(Constantes.RutaRpt),reporte+".rpt"));
                rd.SetDataSource(ObtenerReporte(namestore, argumentos));

                Stream stream;
                if (extension.Equals(tipoExportacion.pdf))
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                else
                    if (extension.Equals(tipoExportacion.xls))
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                else
                    stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.WordForWindows);
                stream.Seek(0, SeekOrigin.Begin);

                return stream;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public static List<Xynthesis.Modelo.xyp_SelReports_Result> ObtenerReportesProgramados(Int32 IdConfiguracion)
        {
            try
            {
                ADReportes oReporte = new ADReportes();
                return oReporte.ObtenerReportesProgramados(IdConfiguracion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void GenerarReporteProgramado(List<xyp_SelReports_Result> lstreporte,params object[] argumentos)
        {
            Stream ArchivoRPT = null;
            try
            {
                xyp_SelReports_Result oInfReporte = lstreporte.FirstOrDefault();
           
                List<Attachment> lstAttachment = new List<Attachment>();
                foreach (var rpt in lstreporte)
                { //Mandar parametros desde aqui
                    if (numerodeparametros(rpt.NombreRpt) == 3)
                    {
                        var fi = argumentos[0];
                        var ff = argumentos[1];
                         ArchivoRPT = GenerarRptArchivo(rpt.FormatoArchivo, rpt.MetodoRpt, rpt.NombreRpt, fi,ff,"");
                    }
                    else if (numerodeparametros(rpt.NombreRpt) == 5)
                    {
                        var fi = argumentos[0];
                        var ff = argumentos[1];
                        ArchivoRPT = GenerarRptArchivo(rpt.FormatoArchivo, rpt.MetodoRpt, rpt.NombreRpt, fi, ff, "","","");
                    }
                    else if (numerodeparametros(rpt.NombreRpt) == 4)
                    {
                        var fi = argumentos[0];
                        var ff = argumentos[1];
                        ArchivoRPT = GenerarRptArchivo(rpt.FormatoArchivo, rpt.MetodoRpt, rpt.NombreRpt, fi, ff, "", "");
                    }
                    //Stream ArchivoRPT = GenerarRptArchivo(rpt.FormatoArchivo, rpt.MetodoRpt, rpt.NombreRpt, argumentos);
                    lstAttachment.Add(new System.Net.Mail.Attachment(ArchivoRPT, rpt.NombreRpt + "." + rpt.FormatoArchivo));
                }
                var Correos = oInfReporte.EmailFrom.Split(';').ToArray();

                Utilidades.FuncionesComunes.EnviarCorreo(Correos.ToList(), oInfReporte.Asunto, oInfReporte.Mensaje, lstAttachment);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static int numerodeparametros(string nomrpt)
        {
            if (nomrpt == "ReportePorPeriodoTiempo" || nomrpt == "ReporteConsolidadoCoberturaLLamadas" || nomrpt == "ReporteGraficoLlamadasEntrantesSalientesDuracion" || nomrpt == "ReporteGraficoEstadisticoLlamadasEntrantesSalientes" || nomrpt == "CoberturaLlamadas" || nomrpt == "ConsumoPorCentrosCostos" || nomrpt == "ReporteTiempoDedicado" || nomrpt == "ReporteLlamadasEntrantesSalientes" || nomrpt == "ReporteLlamadasRecibidasTransferencias" || nomrpt == "ReporteTiempoPromedioAntesContestar")
            {
                return 3;
            }
            else if (nomrpt == "ReporteLlamadasEntrantes" || nomrpt == "ReporteLlamadasAbiertasCerradas" || nomrpt == "HistoriaConsumos")
            {
                return 5;
            }
            else if (nomrpt == "ReporteLlamadasSalientes" || nomrpt == "TopLlamadaCampeonaXCosto" || nomrpt == "LlamadaCampeonaXDuracion" || nomrpt == "ConsumosPersonales")
            {
                // Parametro inicial extension en "ConsumosPersonales"
                return 4;
            }
            else
            {
                return 2;
            }

        }

    }
}
