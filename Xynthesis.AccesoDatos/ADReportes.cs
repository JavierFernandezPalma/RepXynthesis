using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Modelo;
using Xynthesis.Utilidades;

namespace Xynthesis.AccesoDatos
{
    public class ADReportes
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
        LogXynthesis log = new LogXynthesis();

        public List<xyp_SelFrequentExtensionNumber_Result> ObtenerFrecuenciaDeLlamadas(string fecini, string fecfin)
        {
            try
            {
                return xyt.xyp_SelFrequentExtensionNumber(fecini, fecfin).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<xyp_CallTypesAmountReport_Result> ObtenerListaClaseDeLlamadas(string FechaInicial, string FechaFinal)
        {
            try
            {
                return xyt.xyp_CallTypesAmountReport(FechaInicial, FechaFinal).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<xyp_ReceiveCalls_Result> ObtenerCoberturaLlamadas(string fecini, string fecfin, string usuario, string area)
        {
            try
            {
                return xyt.xyp_ReceiveCalls(fecini, fecfin, usuario, area).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<xyp_RepConsolidadoCoberturaLLamadas_Result> ObtenerConsolidadoCoberturaLlamadas(string fecini, string fecfin, string usuari, string area)
        {
            try
            {
                return xyt.xyp_RepConsolidadoCoberturaLLamadas(fecini, fecfin, usuari, area).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<xyp_SelConsumeByExtensionAndUser_Result> ObtenerConsumosPersonales(string fecini, string fecfin, string usuario, string area, string extension, string cobertura, string destino)
        {
            //XYNP_SelConsumeByExtensionAndUser
            try
            {
                return xyt.xyp_SelConsumeByExtensionAndUser(fecini, fecfin, usuario, area, extension, cobertura, destino).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<xyp_SelConsumeByCostCenter_Result> ObtenerConsumosPorcentroCostos(string fecini, string fecfin, string area, string cobertura)
        {
            try
            {
                return xyt.xyp_SelConsumeByCostCenter(fecini, fecfin, area, cobertura).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<xyp_RepReceiveCallsLlamEntranSalien_Result> ObtenerListaLlamadasEntrantesSalientes(string FechaInicial, string FechaFinal, string usuario, string area)
        {
            try
            {
                return xyt.xyp_RepReceiveCallsLlamEntranSalien(FechaInicial, FechaFinal, usuario, area).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<xyp_ReceiveCallsTiempoPromedio_repo_Result> ObtenerListaTiempoPromedioAntesContestar(string FechaInicial, string FechaFinal)
        public List<xyp_ReceiveCallsTiempoPromedio_Result> ObtenerListaTiempoPromedioAntesContestar(string FechaInicial, string FechaFinal, string usuario, string area)
        {
            try
            {
                return xyt.xyp_ReceiveCallsTiempoPromedio(FechaInicial, FechaFinal, usuario, area).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<xyp_ReceiveCallsTiempoPromedio_Result> ObtenerListaTiempoPromedioAntesContestarResumido(string FechaInicial, string FechaFinal, string usuario, string area)
        {
            try
            {
                return xyt.xyp_ReceiveCallsTiempoPromedio(FechaInicial, FechaFinal, usuario, area).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<xyp_SelConsumeByHistory_Result> ObtenerHistoriaConsumos(string fecini, string fecfin, string area)
        {
            try
            {
                return xyt.xyp_SelConsumeByHistory(fecini, fecfin, area).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<xyp_SelActivityFirstAndLast_Result> ObtenerListaInicioFin(string FechaInicial, string FechaFinal, string usuarios)
        {
            try
            {
                return xyt.xyp_SelActivityFirstAndLast(FechaInicial, FechaFinal, usuarios).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<xyp_RepCallOpenAndClosed_Result> ObtenerListaLlamadasAbiertasCerradas(string FechaInicial, string FechaFinal, string hora, string usuario, string llamada)
        {
            try
            {
                return xyt.xyp_RepCallOpenAndClosed(FechaInicial, FechaFinal, hora, usuario, llamada).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<xyp_NumberAmountsByInSubscriber_Result> ObtenerListaLlamadasEntrantes(string FechaInicial, string FechaFinal, string usuario, string llamadaEntrante, string extension)
        {
            try
            {
                var lis = xyt.xyp_NumberAmountsByInSubscriber(FechaInicial, FechaFinal, usuario, llamadaEntrante, extension).ToList();
                return lis;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<xyp_ReceiveAndTransferCalls_Result> ObtenerListaLlamadasRecibidasTransferidas(string FechaInicial, string FechaFinal, string usuario)
        {
            try
            {
                return xyt.xyp_ReceiveAndTransferCalls(FechaInicial, FechaFinal, usuario).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<xyp_NumberAmountsByOutSubscriber_Result> ObtenerListaLlamadasSalientes(string FechaInicial, string FechaFinal, string usuario, string llamada)
        {
            try
            {
                return xyt.xyp_NumberAmountsByOutSubscriber(FechaInicial, FechaFinal, usuario, llamada).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<xyp_SelDialedNumber_Result> ObtenerNumeroMasMarcado(string fecini, string fecfin, string origen, string destino)
        {
            try
            {
                return xyt.xyp_SelDialedNumber(fecini, fecfin, origen, destino).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<xyp_SelCallAmountsBySubscriber_Result> ObtenerListaPorPeriodoTiempo(string FechaInicial, string FechaFinal, string usuario)
        {
            try
            {
                return xyt.xyp_SelCallAmountsBySubscriber(FechaInicial, FechaFinal, usuario).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<xyp_CallAmountByContraparte_Result> ObtenerListaTiempoDedicado(string FechaInicial, string FechaFinal, string usuario)
        {
            try
            {
                return xyt.xyp_CallAmountByContraparte(FechaInicial, FechaFinal, usuario).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<xyp_SelDetailChampCallCost_Result> ObtenerTopLlamadaCampeonaCC(string fecini, string fecfin, string area, string llamada, string origen)
        {
            try
            {
                return xyt.xyp_SelDetailChampCallCost(fecini, fecfin, area, llamada, origen).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<xyp_RepPromedioLlamadasHora_Result> ObtenerPromedioLlamadasHora(string FechaInicial, string FechaFinal, string usuario, string area, string rango)
        {
            try
            {
                return xyt.xyp_RepPromedioLlamadasHora(FechaInicial, FechaFinal, usuario, area, rango).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<xyp_SelReports_Result> ObtenerReportesProgramados(Int32 IdConfiguracion)
        {
            try
            {
                return xyt.xyp_SelReports(IdConfiguracion).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<xyp_SelDetailChampCallDuration_Result> ObtenerLlamadaCampeonaDuracion(string FechaInicial, string FechaFinal, string area, string llamada)
        {
            try
            {
                return xyt.xyp_SelDetailChampCallDuration(FechaInicial,  FechaFinal, area, llamada).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<xyp_RepGrafNumberAmountsBySubscriber_Result> ObtenerListaLlamadasEntrantesSalientesDuracion(string FechaInicial, string FechaFinal, string usuario, string area)
        {
            try
            {
                return xyt.xyp_RepGrafNumberAmountsBySubscriber(FechaInicial, FechaFinal, usuario, area).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public List<xyp_RepTarificacionEntraSalieTrans_Result> ObtenerTarificacion(string FechaInicial, string FechaFinal, string usuario, string area, string sucursal)
        {
            try
            {
                return xyt.xyp_RepTarificacionEntraSalieTrans(FechaInicial, FechaFinal, usuario, area, sucursal).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
