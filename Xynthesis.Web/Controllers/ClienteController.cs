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
    public class ClienteController : Controller
    {
        Utilidades.Constantes cons = new Constantes();
        AccesoDatos.ADCliente cliente = new ADCliente();
        Utilidades.LogXynthesis log = new LogXynthesis();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        xynthesisEntities xyt = new xynthesisEntities();

        // GET: Cliente
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
                var xy_cliente_ = cliente.ObtenerListaUsuarios(); 

                IPagedList<xy_cliente> clie = null;
                clie = xy_cliente_.ToList().ToPagedList(pageIndex, pageSize);
                return View("index", clie);
            }
            catch (Exception ex)
            {
                log.EscribaLog("CLIENTE", "Action:Index " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
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
                var res = cliente.OrdenFiltro(sortOrder, searchString);

                var pagedList1 = res.ToPagedList(pageIndex, pageSize);
                return View("index", pagedList1);
            }
            catch (Exception ex)
            {
                log.EscribaLog("CLIENTE", "Action:OrdenFiltro " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult NuevoCliente()
        {
            msg = new Mensaje();
            msg.codigo = -1;
            try
            {

                ViewBag.mensaje = msg;
                return View();

            }
            catch (Exception ex)
            {
                log.EscribaLog("CLIENTE", "Action:Create " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoCliente([Bind(Include = "Idcliente,nombreCliente")] xy_cliente xy_cliente, string nombreCliente)
        {

            msg = new Mensaje();
            msg.codigo = -1;
            try
            {

                bool exists = (from nomb in xyt.xy_cliente
                               where nomb.nombreCliente == nombreCliente
                               select nomb).Any();

                if (exists == true)
                {
                    ViewBag.Message = MensajesXynthesis.existeRegi;
                    Session["mensale"] = MensajesXynthesis.existeRegi;
                    Session["codigo"] = "0";
                    return View("NuevoCliente");
                }



                if (ModelState.IsValid && xy_cliente.nombreCliente!="")
                {
                    msg = cliente.nuevoCliente(xy_cliente);
                    ViewBag.mensaje = msg;
                    ViewBag.message_ok = MensajesXynthesis.Nuevo;
                    Session["mensale"] = MensajesXynthesis.Nuevo;
                    Session["codigo"] = "1";
                    return RedirectToAction("Index");
                }
                ViewBag.mensaje = msg;
                return View(xy_cliente);

            }
            catch (Exception ex)
            {
                Session["codigo"] = "0";
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                log.EscribaLog("CLIENTE", "Action:Create " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult EditarCliente(int id)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            try
            {
                if (id.Equals(null))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                xy_cliente xy_cliente = cliente.buscarClientexId(id);    //db.xy_cliente.Find(id);
                if (xy_cliente == null)
                {
                    return HttpNotFound();
                }
                ViewBag.mensaje = msg;
                return View(xy_cliente);
            }
            catch (Exception ex)
            {
                log.EscribaLog("CLIENTE", "Action:Edit " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCliente([Bind(Include = "Idcliente,nombreCliente")] xy_cliente xy_cliente)
        {
            msg = new Mensaje();
            msg.codigo = -1;

            bool exists = (from nomb in xyt.xy_cliente
                           where nomb.nombreCliente == xy_cliente.nombreCliente
                           select nomb).Any();

            if (exists == true)
            {
                ViewBag.Message = MensajesXynthesis.existeRegi;
                Session["mensale"] = MensajesXynthesis.existeRegi;
                Session["codigo"] = "0";
                return View("EditarCliente");
            }

            try
            {
                if (ModelState.IsValid && xy_cliente.nombreCliente != "")
                {
                    msg = cliente.guardarEdicion(xy_cliente);
                    ViewBag.mensaje = msg;
                    ViewBag.message_ok = MensajesXynthesis.Actualiza;
                    Session["mensale"] = MensajesXynthesis.Actualiza;
                    Session["codigo"] = "1";
                    return RedirectToAction("Index");
                }
                ViewBag.mensaje = msg;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Session["codigo"] = "0";
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                log.EscribaLog("CLIENTE", "Action:Edir Post " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Index");
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


        [HttpPost, ActionName("EliminarRegistro")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminaTar(int id)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

                
                msg = new Mensaje();
                msg.codigo = -1;
                try
                {
                    msg = cliente.EliminarCliente(id);
                    ViewBag.mensaje = msg;
                    ViewBag.Message = MensajesXynthesis.Elimina;
                    Session["mensale"] = MensajesXynthesis.Elimina;
                    Session["codigo"] = "1";
                //if (msg.codigo == 0)
                //    return View("EliminarCliente", cliente.buscarClientexId(id));
                //else
                //    return View("EliminarCliente", new xy_cliente());
                return Json(new { success = true });

            }
                catch (Exception ex)
                {
                    ViewBag.mensaje = msg;
                    log.EscribaLog("CLIENTE", "Action:DeleteConfirmed " + ex.Message, Session["Nom_DomainUser"].ToString());
                    Session["mensale"] = MensajesXynthesis.NoProcesa;
                    Session["codigo"] = "0";
                    return Json(new { success = false });
            }           


        }


    }
}