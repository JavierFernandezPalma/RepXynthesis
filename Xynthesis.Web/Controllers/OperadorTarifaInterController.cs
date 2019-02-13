using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xynthesis.AccesoDatos;
using Xynthesis.Utilidades;
using PagedList;
using PagedList.Mvc;
using Xynthesis.Modelo;
using System.Net;
using Xynthesis.Utilidades.Mensajes;

namespace Xynthesis.Web.Controllers
{
    public class OperadorTarifaInterController : Controller
    {
        Utilidades.Constantes cons = new Constantes();
        AccesoDatos.ADOperadorTarifaInter operinter = new ADOperadorTarifaInter();
        AccesoDatos.ADNumeros numeros = new ADNumeros();
        Utilidades.LogXynthesis log = new LogXynthesis();
        xynthesisEntities xyt = new xynthesisEntities();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();

        public ActionResult Index(int? page)
        {
            try
            {
                if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
                {
                    return RedirectToAction("Login", "Acceso");
                }
                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageIndex = 1;
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

                var trInter = operinter.ObtenerListaOperadoresInter();
                var pagedList = trInter.ToPagedList(pageIndex, pageSize);
                return View("index", pagedList);

            }
            catch (Exception ex)
            {
                log.EscribaLog("OPERINTER", "Action:Index " + ex.Message, Session["Nom_DomainUser"].ToString());
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
                var res = operinter.OrdenFiltro(sortOrder, searchString);

                var pagedList1 = res.ToPagedList(pageIndex, pageSize);
                return View("index", pagedList1);
            }
            catch (Exception ex)
            {
                log.EscribaLog("OPERINTER", "Action:OrdenFiltro " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult NuevoOperInter()
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            msg = new Mensaje();
            msg.codigo = -1;
            try
            {

                var pais = operinter.ObtenerPaises();
                SelectList listaPais = new SelectList(pais, "IdPais", "nombrePais");

                ViewData["listaOper"] = new SelectList(operinter.ObtenerOperadorInternacional(), "Ide_Operator", "Nom_Operator", ""); 
                ViewData["listaPais"] = listaPais;
                ViewBag.mensaje = msg;
                return View(new xy_operador_tarifa_inter());
            }
            catch (Exception ex)
            {
                log.EscribaLog("OPERINTER", "Action:Create " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoOperInter(string DdlOperador, string DdlPais, string VlrInternaMinsinIva, string VlrInternaMinconIva, string Fijo, string Movil, xy_operador_tarifa_inter xy_operador_tarifa_inter)
        {
            msg = new Mensaje();
            msg.codigo = -1;

            

            if (VlrInternaMinconIva == "" || VlrInternaMinsinIva == "")
            {
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                Session["codigo"] = "0";
                return RedirectToAction("NuevoOperInter");
            }


            if (ModelState.IsValid)
            { }
            try
            {
                var pais = operinter.ObtenerPaises();
                SelectList listaPais = new SelectList(pais, "IdPais", "nombrePais");

                ViewData["listaOper"] = new SelectList(operinter.ObtenerOperadorInternacional(), "Ide_Operator", "Nom_Operator", "");
                ViewData["listaPais"] = listaPais;

                try
                {

                    if (Convert.ToDecimal(VlrInternaMinsinIva) > 0)
                    { }
                    if (Convert.ToDecimal(VlrInternaMinconIva) > 0)
                    { }
                    msg = operinter.nuevaOperadorTarifaInter(DdlOperador, DdlPais, VlrInternaMinsinIva, VlrInternaMinconIva, Fijo, Movil);
                    if (msg.codigo == 1)
                    {
                        Session["mensale"] = MensajesXynthesis.Nuevo;
                        Session["codigo"] = "1";

                    }
                    else
                    {
                        Session["mensale"] = MensajesXynthesis.NoProcesa;
                        Session["codigo"] = "0";
                        return RedirectToAction("NuevoOperInter");
                    }
                }
                catch
                {
                    msg.codigo = 1;
                    msg.mensaje = Xynthesis.Utilidades.Mensajes.MensajesXynthesis.datosInvalido;
                    Session["mensale"] = MensajesXynthesis.datosInvalido;
                    Session["codigo"] = "0";
                    return RedirectToAction("NuevoOperInter");
                }
               // msg.mensaje = Xynthesis.Utilidades.Mensajes.MensajesXynthesis.datosInvalido;
                ViewBag.mensaje = msg;
                xy_operador_tarifa_inter operint = new xy_operador_tarifa_inter();
                return RedirectToAction("Index");
                //return View("NuevoOperInter", operint);
            }
            catch (Exception ex)
            {
                Session["mensale"] = MensajesXynthesis.ErrDesconocido;
                Session["codigo"] = "0";
                log.EscribaLog("OPERINTER", "Action:Create Post" + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Index");
            }
        }

        public ActionResult EditarOperInter(long id)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            msg = new Mensaje();
            msg.codigo = -1;
            
            try
            {
                xy_operador_tarifa_inter xy_operador_tarifa_inter = operinter.buscarOperadorInterId(id) ;

                var pais = operinter.ObtenerPaises();
                SelectList listaPais = new SelectList(pais, "IdPais", "nombrePais", xy_operador_tarifa_inter.IdPais);

                ViewData["listaOper"] = new SelectList(operinter.ObtenerOperadorInternacional(), "Ide_Operator", "Nom_Operator", xy_operador_tarifa_inter.IdOperador);
                ViewData["listaPais"] = listaPais;
                ViewBag.mensaje = msg;
                if (id.Equals(null))
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    if (xy_operador_tarifa_inter == null)
                    {
                        return HttpNotFound();
                    }
                    return View(xy_operador_tarifa_inter);

            }
            catch (Exception ex)
            {
                log.EscribaLog("OPERINTER", "Action:Edit" + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarOperInter( int? idtarifaInter, string DdlOperador, string DdlPais, string VlrInternaMinsinIva, string VlrInternaMinconIva, string Fijo, string Movil, xy_operador_tarifa_inter xy_operador_tarifa_inter)
        {
            
            msg = new Mensaje();
            msg.codigo = -1;
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            if (VlrInternaMinconIva == "" || VlrInternaMinsinIva == "")
            {
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                Session["codigo"] = "0";
                return RedirectToAction("EditarOperInter");
            }


            try
            {

                var pais = operinter.ObtenerPaises();

                if (ModelState.IsValid)
                { }
                try
                {
                    if (Convert.ToDecimal(VlrInternaMinsinIva) > 0)
                    { }
                    if (Convert.ToDecimal(VlrInternaMinconIva) > 0)
                    { }
                    msg = operinter.guardarEdicion(idtarifaInter, DdlOperador, DdlPais, VlrInternaMinsinIva, VlrInternaMinconIva, Fijo, Movil);
                    Session["mensale"] = MensajesXynthesis.Actualiza;
                    Session["codigo"] = "1";
                }
                catch
                {
                    if (Fijo == null)
                    {

                    }
                    msg.codigo = 1;
                    msg.mensaje = Xynthesis.Utilidades.Mensajes.MensajesXynthesis.datosInvalido;
                    Session["mensale"] = MensajesXynthesis.datosInvalido;
                    Session["codigo"] = "0";
                }

                ViewBag.mensaje = msg;
                SelectList listaPais = new SelectList(pais, "IdPais", "nombrePais", DdlPais);
                ViewData["listaOper"] = new SelectList(operinter.ObtenerOperadorInternacional(), "Ide_Operator", "Nom_Operator", DdlOperador);
                ViewData["listaPais"] = listaPais;
                xy_operador_tarifa_inter xy_operador_tarifa_inter_ = operinter.buscarOperadorInterId((long)idtarifaInter);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                msg.mensaje = Xynthesis.Utilidades.Mensajes.MensajesXynthesis.datosInvalido;
                Session["mensale"] = MensajesXynthesis.ErrDesconocido;
                log.EscribaLog("OPERINTER", "Action:Edit" + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }

        public ActionResult EliminarOperInter(long id)
        {

            try
            {
                    if (id.Equals(null))
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    var oper = operinter.ObtenerOperadorInternacional();

                    xy_operador_tarifa_inter xy_operador_tarifa_inter = operinter.buscarOperadorInterId(id) ;  
                    SelectList listaOper = new SelectList(oper, "Ide_Operator", "Nom_Operator", xy_operador_tarifa_inter.IdOperador);
                    var pais = operinter.ObtenerPaises();
                    SelectList listaPais = new SelectList(pais, "IdPais", "nombrePais", xy_operador_tarifa_inter.IdPais);
                    ViewData["listaOper"] = listaOper;
                    ViewData["listaPais"] = listaPais;
                    if (xy_operador_tarifa_inter == null)
                    {
                        return HttpNotFound();
                    }
                    return View(xy_operador_tarifa_inter);

            }
            catch (Exception ex)
            {
                log.EscribaLog("OPERINTER", "Action:Delete" + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }


        [HttpPost, ActionName("EliminarOperInter")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            try
            {
                    operinter.eliminarOperadorTarifaInter(id);
                    return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                log.EscribaLog("OPERINTER", "Action:Delete Post" + ex.Message, Session["Nom_DomainUser"].ToString());
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
            try
            {
                int res = operinter.eliminarOperadorTarifaInter(id);
                if (res == 1)
                {
                    Session["mensale"] = MensajesXynthesis.Elimina;
                    Session["codigo"] = "1";
                }
                else
                {
                    Session["mensale"] = MensajesXynthesis.NoProcesa;
                    Session["codigo"] = "0";
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                Session["codigo"] = "0";
                return Json(new { success = false });
            }
        }


    }
}