using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xynthesis.AccesoDatos;
using Xynthesis.Utilidades;
using Xynthesis.Utilidades.Mensajes;
using PagedList;
using PagedList.Mvc;
using Xynthesis.Modelo;


namespace Xynthesis.Web.Controllers
{
    public class AreasController : Controller
    {
        Utilidades.Constantes cons = new Constantes();
        AccesoDatos.ADAreas area = new ADAreas();
        Utilidades.LogXynthesis log = new LogXynthesis();
        xynthesisEntities xyt = new xynthesisEntities();
        Mensaje msg = new Mensaje();
    
        // GET: Áreas
        public ActionResult Index(int? page)
        {
            try
            {
                string MaxRegGrilla = cons.MaxRegGrilla;
                int pageSize = MaxRegGrilla == null ? 8 : Convert.ToInt32(MaxRegGrilla);
                int pageIndex = 1;
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                return View("Index", area.ObtenerListaAreas().ToPagedList(pageIndex, pageSize));
            }
            catch (Exception ex)
            {
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
                var res = area.OrdenFiltro(sortOrder,searchString);

                var pagedList1 = res.ToPagedList(pageIndex, pageSize);
                return View("index", pagedList1);
            }
            catch (Exception ex)
            {
                log.EscribaLog("AREAS", "Action:OrdenFiltro " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        // GET:  xy_subscriber/Create
        public ActionResult NuevaArea()
        {
            msg = new Mensaje();
            msg.codigo = -1;
            try
            {
                if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
                {
                    return RedirectToAction("Login", "Acceso");
                }
                xy_costcenters area = new xy_costcenters();
                ViewBag.mensaje = msg;
                return View("NuevaArea", area);

            }
            catch (Exception ex)
            {
                log.EscribaLog("AREAS", "Action:Create " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        public ActionResult NuevaArea(xy_costcenters nuevo, string Nom_CostCenter)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            try
            {
                if (ModelState.IsValid)
                {

                    bool exists = (from nomb in xyt.xy_costcenters
                                   where nomb.Nom_CostCenter == Nom_CostCenter
                                   select nomb).Any();

                    if (exists == true)
                    {
                        ViewBag.Message = MensajesXynthesis.existeRegi;
                        Session["mensale"] = MensajesXynthesis.existeRegi;
                        Session["codigo"] = "0";
                        return View("NuevaArea");
                    }
                    
                }

                if(Nom_CostCenter =="")
                {
                    Session["codigo"] = "0";
                    Session["mensale"] = MensajesXynthesis.NoProcesa;
                    return RedirectToAction("Index");
                }


                area.nuevaArea(nuevo);
                ViewBag.mensaje = msg;
                ViewBag.message_ok = MensajesXynthesis.Nuevo;
                Session["mensale"] = MensajesXynthesis.Nuevo;
                Session["codigo"] = "1";
                return RedirectToAction("Index");
            }
            catch (Exception error)
            {
                Session["codigo"] = "0";
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                ViewBag.mensaje = msg;
                log.EscribaLog("AREAS", "Action:Create " + error.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult EditarArea(long? id)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            try
            {

                xy_costcenters ccos = area.buscarAreaxId(id);
                ViewBag.mensaje = msg;
                return View(ccos);

            }
            catch (Exception ex)
            {
                ViewBag.mensaje = msg;
                log.EscribaLog("AREAS", "Action:Edit_Get " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarArea(xy_costcenters xy_costcenters_, string Nom_CostCenter)
        {
                msg = new Mensaje();
                msg.codigo = -1;
            try
            {
                if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
                {
                    return RedirectToAction("Login", "Acceso");
                }

                
                    if (ModelState.IsValid  && Nom_CostCenter!="")
                {
                    msg = area.guardarEdicion(xy_costcenters_, Nom_CostCenter);
                    ViewBag.mensaje = msg;
                    if(msg.codigo == 1)
                    {
                        Session["mensale"] = MensajesXynthesis.Actualiza;
                        Session["codigo"] = "1";
                        return RedirectToAction("Index");
                    }else
                    {
                        Session["mensale"] = MensajesXynthesis.existeRegi;
                        Session["codigo"] = "0";
                        return RedirectToAction("EditarArea");
                    }                    
                }
                ViewBag.mensaje = msg;
                return View("EditarArea", xy_costcenters_);
            }
            catch (Exception ex)
            {
                Session["mensale"] = MensajesXynthesis.ErrDesconocido;
                Session["codigo"] = "0";
                ViewBag.mensaje = msg;
                log.EscribaLog("AREAS", "Action:Edit_post " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Index");
            }
        }


        // GET:  xy_subscriber/Delete/5
        public ActionResult EliminarArea(long? id)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            try
            {
                if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
                {
                    return RedirectToAction("Login", "Acceso");
                }
                xy_costcenters xy_costcenters_ = area.buscarAreaxId(id);
                if (xy_costcenters_ == null)
                {
                    ViewBag.mensaje = msg;
                    return HttpNotFound();
                }
                ViewBag.mensaje = msg;
                return View("EliminarArea",xy_costcenters_);
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = msg;
                log.EscribaLog("AREAS", "Action:Delete_get " + ex.Message, Session["Nom_DomainUser"].ToString());
                // return RedirectToAction("Error", "Error");
                return RedirectToAction("Index");
            }

        }

        // POST:  xy_subscriber/Delete/5
        [HttpPost, ActionName("EliminarArea")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            try
            {
               
               msg  =  area.EliminarArea(id);
               ViewBag.mensaje = msg;
               xy_costcenters xy_costcenters_ = new xy_costcenters();
                xy_costcenters_.Cod_CostCenter = "";
               return View("EliminarArea", xy_costcenters_);
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = msg;
                log.EscribaLog("AREAS", "Action:DeleteConfirmed " + ex.Message, Session["Nom_DomainUser"].ToString());
                return View("EliminarArea");

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
            try
            {

                msg = area.EliminarArea(id);
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
                ViewBag.mensaje = msg;
                xy_costcenters xy_costcenters_ = new xy_costcenters();
                xy_costcenters_.Cod_CostCenter = "";
                //return View("EliminarArea", xy_costcenters_);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Session["mensale"] = MensajesXynthesis.ErrDesconocido;
                Session["codigo"] = "0";
                ViewBag.mensaje = msg;
                log.EscribaLog("AREAS", "Action:DeleteConfirmed " + ex.Message, Session["Nom_DomainUser"].ToString());
                return Json(new { success = true });
                //return View("EliminarArea");

            }

        }

    }
}