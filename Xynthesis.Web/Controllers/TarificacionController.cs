using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Xynthesis.AccesoDatos;
using Xynthesis.Utilidades;
using PagedList;
using PagedList.Mvc;
using Xynthesis.Modelo;
using Xynthesis.Utilidades.Mensajes;

namespace Xynthesis.Web.Controllers
{
    public class TarificacionController : Controller
    {
        Utilidades.Constantes cons = new Constantes();
        AccesoDatos.ADCoberturas cob = new ADCoberturas();
        AccesoDatos.ADTarificacion tarificacion = new ADTarificacion();
        Utilidades.LogXynthesis log = new LogXynthesis();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        xynthesisEntities xyt = new xynthesisEntities();
        public ActionResult Index(int? page)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null && Session["LoginDominio"] == null) 
            {
                return RedirectToAction("Login", "Acceso");
            }
            try
            {
                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageIndex = 1;
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                var tar = tarificacion.ObtenerListaUsuarios();

                IPagedList<xyp_SelOperators_Result> tar_ = null;
                tar_ = tar.ToList().ToPagedList(pageIndex, pageSize);
                return View("index", tar_);
            }
            catch (Exception ex)
            {
                log.EscribaLog("TARIFICACION", "Action:Index " + ex.Message, Session["Nom_DomainUser"].ToString());
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
                var res = tarificacion.OrdenFiltro(sortOrder, searchString);

                var pagedList1 = res.ToPagedList(pageIndex, pageSize);
                return View("index", pagedList1);
            }
            catch (Exception ex)
            {
                log.EscribaLog("CLIENTE", "Action:OrdenFiltro " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult Edit(int id)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            try
            {
                xy_operators tar = tarificacion.buscarTarificacionId(id);
                List<SelectListItem> listaUnit = new List<SelectListItem>();
                listaUnit.Add(new SelectListItem { Text = "-Seleccione Opción-", Value = "Seleccione" });
                listaUnit.Add(new SelectListItem { Text = "Minutos", Value = "M", Selected = (tar.Unit == "M" ? true : false) });
                listaUnit.Add(new SelectListItem { Text = "Segundos", Value = "S", Selected = (tar.Unit == "S" ? true : false) });
                ViewData["listaCobertura"] = new SelectList(tarificacion.obtenerListaCoberturas(), "Ide_Coverage", "Nom_Coverage", Convert.ToInt32(tar.Ide_Coverage));
                ViewData["listaUnit"] = listaUnit;
                if (tar.Ide_Coverage != null)
                {
                    int cobertura = Convert.ToInt32(tar.Ide_Coverage);
                    ViewData["listCobertura"] = tarificacion.obtenerCoberturaXId(cobertura);
                }
                else
                {
                    ViewData["listCobertura"] = new xy_coverage();
                }
                ViewData["listRate"] = tarificacion.buscarExtensionesId(id);
                ViewBag.mensaje = msg;
                return View(tar);
            }
            catch (Exception ex)
            {
                log.EscribaLog("TARIFICACION", "Action:Edit Get " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, xy_operators res, string DdUnit, string DdlCobertura )
        {
            xy_operators tar = null;
            msg = new Mensaje();
            msg.codigo = -1;

            //bool exists = (from nomb in xyt.xy_operators
            //               where nomb.Nom_Operator == res.Nom_Operator || res.Nom_Operator == null
            //               select nomb).Any();
            int exists_ = (from nomb in xyt.xy_operators
                           where nomb.Nom_Operator == res.Nom_Operator && nomb.Ide_Operator != res.Ide_Operator
                           select nomb).Count();

            if (exists_ > 0)
            {
                ViewBag.Message = MensajesXynthesis.existeRegi;
                Session["mensale"] = MensajesXynthesis.existeRegi;
                Session["codigo"] = "0";
                return RedirectToAction("Edit");
            }



            try
            {
                


                if (ModelState.IsValid)
                { }
                try
                {
                    if (Convert.ToDecimal(res.vlr_Cost) > 0)
                    { }
                    msg = tarificacion.guardarEdicion(id, res, DdUnit, DdlCobertura);
                    Session["mensale"] = MensajesXynthesis.Actualiza;
                    Session["codigo"] = "1";
                }
                catch
                {
                    msg.codigo = 1;
                    msg.mensaje = Xynthesis.Utilidades.Mensajes.MensajesXynthesis.datosInvalido;
                    Session["mensale"] = MensajesXynthesis.datosInvalido;
                    Session["codigo"] = "0";
                    return RedirectToAction("Edit");
                }
                ViewBag.mensaje = msg;

                tar = tarificacion.buscarTarificacionId(id);
                List<SelectListItem> listaUnit = new List<SelectListItem>();
                listaUnit.Add(new SelectListItem { Text = "-Seleccione Opción-", Value = "Seleccione" });
                listaUnit.Add(new SelectListItem { Text = "Minutos", Value = "M", Selected = (tar.Unit == "M" ? true : false) });
                listaUnit.Add(new SelectListItem { Text = "Segundos", Value = "S", Selected = (tar.Unit == "S" ? true : false) });
                ViewData["listaCobertura"] = new SelectList(tarificacion.obtenerListaCoberturas(), "Ide_Coverage", "Nom_Coverage", Convert.ToInt32(tar.Ide_Coverage));
                ViewData["listaUnit"] = listaUnit;
                if (tar.Ide_Coverage != null)
                {
                    int cobertura = Convert.ToInt32(tar.Ide_Coverage);
                    ViewData["listCobertura"] = tarificacion.obtenerCoberturaXId(cobertura);
                }
                else
                {
                    ViewData["listCobertura"] = new xy_coverage();
                }
                ViewData["listRate"] = tarificacion.buscarExtensionesId(id);
                return  RedirectToAction("Index");
                //return View("Edit", tar);
            }
            catch (Exception ex)
            {
                Session["mensale"] = MensajesXynthesis.ErrDesconocido;
                Session["codigo"] = "0";
                log.EscribaLog("TARIFICACION", "Action:Create_Post " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult EliminarRegistro()
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
            try
            {
                msg = tarificacion.EliminarOperador(id);
                if (msg.codigo == 1)
                {
                    Session["mensale"] = MensajesXynthesis.Elimina;
                    Session["codigo"] = "1";
                }
                else
                {
                    Session["mensale"] = MensajesXynthesis.NoProcesa;
                    Session["codigo"] = "0";
                }
                ViewBag.mensale = msg.mensaje;
                xy_operators xy_operators_ = new xy_operators();
                List<SelectListItem> listaUnit = new List<SelectListItem>();
                ViewData["listaCobertura"] = new SelectList(cob.listacoberturas(), "Ide_Coverage", "Nom_Coverage", 0);
                ViewData["listaUnit"] = listaUnit;
                ViewData["listRate"] = new xy_rates();

             
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = msg;
                log.EscribaLog("TARIFICACION", "Action:DeleteConfirmed " + ex.Message, Session["Nom_DomainUser"].ToString());
                return Json(new { success = false });
            }
        }

        public ActionResult Delete(int id)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            try
            {
                xy_operators tar = tarificacion.buscarTarificacionId(id);
                List<SelectListItem> listaUnit = new List<SelectListItem>();
                listaUnit.Add(new SelectListItem { Text = "-Seleccione Opción-", Value = "Seleccione" });
                listaUnit.Add(new SelectListItem { Text = "Minutos", Value = "M", Selected = (tar.Unit == "M" ? true : false) });
                listaUnit.Add(new SelectListItem { Text = "Segundos", Value = "S", Selected = (tar.Unit == "S" ? true : false) });
                ViewData["listaCobertura"] = new SelectList(cob.listacoberturas(), "Ide_Coverage", "Nom_Coverage", Convert.ToInt32(tar.Ide_Coverage));
                ViewData["listaUnit"] = listaUnit;
                if (tar.Ide_Coverage != null)
                {
                    int cobertura = Convert.ToInt32(tar.Ide_Coverage);
                    ViewData["listCobertura"] = tarificacion.obtenerCoberturaXId(cobertura);
                }
                else
                {
                    ViewData["listCobertura"] = new xy_coverage();
                }
                ViewData["listRate"] = tarificacion.buscarExtensionesId(id);   //(from s in db.xy_rates where s.Ide_Operator == id select s).ToList();
                ViewBag.mensaje = msg;
                return View(tar);
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = msg;
                log.EscribaLog("TARIFICACION", "Action:Delete_Get " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        // POST:  xy_subscriber/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            try
            {
                msg = tarificacion.EliminarOperador(id);
                ViewBag.mensaje = msg;
                xy_operators xy_operators_ = new xy_operators();
                List<SelectListItem> listaUnit = new List<SelectListItem>();
                ViewData["listaCobertura"] = new SelectList(cob.listacoberturas(), "Ide_Coverage", "Nom_Coverage", 0);
                ViewData["listaUnit"] = listaUnit;
                ViewData["listRate"] = new xy_rates();
                return View("Delete", xy_operators_);
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = msg;
                log.EscribaLog("TARIFICACION", "Action:DeleteConfirmed " + ex.Message, Session["Nom_DomainUser"].ToString());
                return View("Delete");
            }
        }



    }
}