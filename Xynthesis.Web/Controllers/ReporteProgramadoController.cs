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
using Xynthesis.Utilidades.Mensajes;
using System.Net;
using System.Configuration;

namespace Xynthesis.Web.Controllers
{
    public class ReporteProgramadoController : Controller
    {
        Utilidades.Constantes cons = new Constantes();
        AccesoDatos.ADReporteProgramado repProg = new ADReporteProgramado();
        AccesoDatos.ADUsuarios usua = new ADUsuarios();
        Utilidades.LogXynthesis log = new LogXynthesis();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        xynthesisEntities xyt = new xynthesisEntities();



        public ActionResult ListaRepProgramado(int? page)
        {

            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }


            try
            {
                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageIndex = 1;
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                return View("ListaRepProgramado", repProg.ObtenerListaRepProgramado().ToList().ToPagedList(pageIndex, pageSize));

            }
            catch (Exception ex)
            {

                log.EscribaLog("LISTA DE REPORTE PROGRAMADO", "Action:Index " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }


        public ActionResult OrdenFiltro(string sortOrder, string searchString, int? page)
        {
            try
            {
                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageIndex = 1;
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var res = repProg.OrdenFiltro(sortOrder, searchString);

                var pagedList1 = res.ToPagedList(pageIndex, pageSize);
                return View("ListaRepProgramado", pagedList1);
            }
            catch (Exception ex)
            {
                log.EscribaLog("ORDEN FILTRO", "Action:OrdenFiltro " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }


        
        public ActionResult RepProgramado()
        {
            ViewBag.Frecuencia = new SelectList(xyt.xy_frecuencia.ToList(), "FrecuenciaId", "NombreFrecuencia");

            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            ViewData["confProgr"] = xyt.xy_configuracionrptprogramado.ToList();

            var lista = xyt.xy_reportes.ToList();

            return View(lista);
        }

        [HttpPost]
        public ActionResult RepProgramado(string NombreConfiguracion,string iden, string formato, string FechaInicial, string FechaFinal,  string horaEjec, string email, string copiaCorreo, string asunto, string txtMensaje, string opFrecuencia, string txtNumDia, string opDiaSemana, string txtDiaNumMes)
        {
            if(NombreConfiguracion == null)
            {
                return RedirectToAction("RepProgramado");
            }
            int IdFrecuencia = 0;
            if (opFrecuencia == "diaria")
            {
                IdFrecuencia = Convert.ToInt32(txtNumDia); //Cada IdFrecuencia dias
            }else if(opFrecuencia == "semanal")
            {
                IdFrecuencia = Convert.ToInt32(opDiaSemana); //los opDiaSemana de cada semana
            }else if(opFrecuencia == "mensual")
            {
                IdFrecuencia = Convert.ToInt32(txtDiaNumMes); //los txtDiaNumMes de cada mes
            }



            //Frecuencia
            
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }



            bool exists = (from nomb in xyt.xy_configuracionrptprogramado
                           where nomb.Nombre == NombreConfiguracion
                           select nomb).Any();

            if (exists == true)
            {
                ViewBag.Message = MensajesXynthesis.existeRegi;
                Session["mensale"] = MensajesXynthesis.existeRegi;
                Session["codigo"] = "0";
                return RedirectToAction("ListaRepProgramado");
            }

            string valor = iden;
            Char separador = '|';
            String[] letras = iden.Split(separador);
            string caracte = email.Replace(",",";");
            

            string correo = caracte.ToLower();
            string copiaCor = copiaCorreo.ToLower();

            try
            {
                for(int i = 0; i<letras.Length; i++)
                {
                    if (! letras[i].Equals(""))
                        xyt.xyp_InsRepProg(Convert.ToInt32(letras[i]), NombreConfiguracion, Convert.ToDateTime(horaEjec), formato, correo, copiaCor, asunto, txtMensaje, "", IdFrecuencia, FechaInicial, FechaFinal, opFrecuencia, IdFrecuencia);
                }
                ViewBag.Message = MensajesXynthesis.Nuevo;
                Session["mensale"] = MensajesXynthesis.Nuevo;
                Session["codigo"] = "1";
                return RedirectToAction("ListaRepProgramado");
            }
            catch (Exception error)
            {
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                Session["codigo"] = "0";
                log.EscribaLog("reporte", "Action:Create " + error.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }   


        }



        public ActionResult EditarRepProg(int? id)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            ViewData["confProgr"] = xyt.xy_reportes.ToList();
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                xy_configuracionrptprogramado confrep = xyt.xy_configuracionrptprogramado.Find(id);

                ViewData["listaReportes"] = (from s in xyt.xy_configuracionreporte where s.CondiguracionId == confrep.ConfiguracionId select s).ToList();

                if (confrep == null)
                {
                    return HttpNotFound();
                }

                string fecha = Convert.ToString(confrep.HoraEjecucion.ToString("hh:mm"));

                return View(confrep);

            }
            catch (Exception ex)
            {
                log.EscribaLog("EDITAR USUARIO", "Action:EditarRepProg " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarRepProg(xy_configuracionrptprogramado xy_subscriber, int ConfiguracionId, string Nombre, string iden, string horaRep, string FormatoArchivo, string EmailFrom, string Asunto, string txtMensaje)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            string caracte = EmailFrom.Replace(",", ";");


            string correo = caracte.ToLower();
     


            try {
                xyt.xyp_UpdRepProgra(ConfiguracionId, iden);
                xy_configuracionrptprogramado confrep = xyt.xy_configuracionrptprogramado.Find(xy_subscriber.ConfiguracionId);                

                confrep.FormatoArchivo = FormatoArchivo;
                confrep.EmailFrom = correo;
                confrep.HoraEjecucion = Convert.ToDateTime(horaRep);
                confrep.Asunto = Asunto;
                confrep.Mensaje = txtMensaje;

                repProg.EditarRepProgram(confrep);
                ViewBag.Message = MensajesXynthesis.Actualiza;
                Session["mensale"] = MensajesXynthesis.Actualiza;
                Session["codigo"] = "1";
                return RedirectToAction("ListaRepProgramado");
               

            }
            catch (Exception ex)
            {
                ViewBag.Message = MensajesXynthesis.NoProcesa;
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                Session["codigo"] = "0";
                log.EscribaLog("EDITAR USUARIO", "Action:EditarRepProg " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }

        }

        public ActionResult EliminarRegistro(long? id)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            return PartialView("../PopupDel/EliminarRegistro");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarRegistro(int id, int? page)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {

                return RedirectToAction("Login", "Acceso");
            }
            

            try
            {
                ViewBag.Message = "";

                int verifica = xyt.xy_configuracionrptprogramado.Where(i => i.ConfiguracionId == id).Count();
                if (verifica > 0)
                {
                    repProg.EliminarRepProgram(id);
                    ViewBag.Message = MensajesXynthesis.Elimina;
                    Session["mensale"] = MensajesXynthesis.Elimina;
                    Session["codigo"] = "1";
                }
                else
                {
                    ViewBag.Message = MensajesXynthesis.NoProcesa;
                    Session["mensale"] = MensajesXynthesis.NoProcesa;
                    Session["codigo"] = "0";
                }

                if (ViewBag.Message != "")
                    return Json(new { success = true });
                else
                    return Json(new { success = true });

            }
            catch (Exception ex)
            {
                log.EscribaLog("ELIMINAR REPORTEPROGRA", "Action:EliminarRepProgr " + ex.Message, Session["Nom_DomainUser"].ToString());
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                Session["codigo"] = "0";
                return Json(new { success = false });
            }


        }


        public ActionResult MostrarFrecuencia() //Accion para mostrar en dropdowlist Descripcion de Turno pero ID 
        {
            using (var xyt = new xynthesisEntities())
            {
                return PartialView(xyt.xy_frecuencia.ToList());
            }
        }

    }
}