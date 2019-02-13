using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xynthesis.AccesoDatos;
using Xynthesis.Utilidades;
using Xynthesis.Modelo;
using PagedList;
using PagedList.Mvc;
using Xynthesis.Utilidades.Mensajes;

namespace Xynthesis.Web.Controllers
{
    public class SucursalController : Controller
    {
        xynthesisEntities xyt = new xynthesisEntities();
        ADSucursal adsu = new ADSucursal();
        LogXynthesis lg = new LogXynthesis();
        Constantes cons = new Constantes();
        Mensaje msg = new Mensaje();
        public ActionResult ListaSucursales(int? page)
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
                return View("ListaSucursales", adsu.listasucursales().ToList().ToPagedList(pageIndex, pageSize));

            }
            catch (Exception ex)
            {
                lg.EscribaLog("SUCURSALES", "Action:ListaSucursales " + ex.Message, Session["Nom_DomainUser"].ToString());
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
                var res = adsu.OrdenFiltro(sortOrder, searchString);

                var pagedList1 = res.ToPagedList(pageIndex, pageSize);
                return View("ListaSucursales", pagedList1);
            }
            catch (Exception ex)
            {
                lg.EscribaLog("ORDEN FILTRO", "Action:OrdenFiltro " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }



        //Nueva Sucursal
        public ActionResult NuevaSucursal()
        {
            msg = new Mensaje();
            msg.codigo = -1;
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            ViewData["Selgeo"] = new SelectList((from s in xyt.xy_geography where s.Nom_Country.ToUpper()=="COLOMBIA" select new { s.Ide_Geography, s.Nom_City }).ToList(), "Ide_Geography", "Nom_City",1);
            ViewBag.mensaje = msg;
            return View();
        }

        [HttpPost]
        public ActionResult NuevaSucursal(xy_sucursal nuevo, string DdlUbicacion, string NombreSucursal)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            try
            {
                bool exists = (from nomb in xyt.xy_sucursal
                               where nomb.NombreSucursal == NombreSucursal
                               select nomb).Any();

                if (exists == true)
                {
                    ViewData["Selgeo"] = new SelectList((from s in xyt.xy_geography where s.Nom_Country.ToUpper() == "COLOMBIA" select new { s.Ide_Geography, s.Nom_City }).ToList(), "Ide_Geography", "Nom_City", 1);
                    ViewBag.Message = MensajesXynthesis.existeRegi;
                    Session["mensale"] = MensajesXynthesis.existeRegi;
                    Session["codigo"] = "0";
                    return View("NuevaSucursal");
                }
                
                nuevo.Ide_Geography = Convert.ToInt32( DdlUbicacion);
                if (ModelState.IsValid && nuevo.NombreSucursal!="")
                        msg = adsu.NuevaSucursal(nuevo);
                ViewBag.message_ok = MensajesXynthesis.Nuevo;
                Session["mensale"] = MensajesXynthesis.Nuevo;
                Session["codigo"] = "1";
                ViewData["Selgeo"] = new SelectList((from s in xyt.xy_geography where s.Nom_Country.ToUpper() == "COLOMBIA" select new { s.Ide_Geography, s.Nom_City }).ToList(), "Ide_Geography", "Nom_City", 1);
                ViewBag.mensaje = msg;
                return RedirectToAction("ListaSucursales");
            }
            catch (Exception error)
            {
                Session["codigo"] = "0";
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                lg.EscribaLog("SUCURSAL", "Action:NuevaSucursal " + error.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("ListaSucursales");
                
            }
        }

        public ActionResult EditarSucursal(int id)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            try
            {
                xy_sucursal editsucursal = xyt.xy_sucursal.Find(id);
                ViewData["Selgeo"] = new SelectList((from s in xyt.xy_geography where s.Nom_Country.ToUpper() == "COLOMBIA" select new { s.Ide_Geography, s.Nom_City }).ToList(), "Ide_Geography", "Nom_City", editsucursal.Ide_Geography);
                var pais = (from t in xyt.xy_geography where t.Nom_Country != "INDEFINIDO" select t);
                Session["idgeo"] = editsucursal.Ide_Geography;
                ViewData["Sucursal"] = editsucursal;
                Session["Sucursal"] = editsucursal;
                ViewBag.mensaje = msg;
                return View(pais.ToList());

            }
            catch (Exception error)
            {
                lg.EscribaLog("SUCURSAL", "Action:EditarSucursal " + error.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }

        }


        [HttpPost]
        public ActionResult EditarSucursal(xy_sucursal editSuc, string IdSucursal, string NombreSucursal, string DdlUbicacion)
        {
            msg = new Mensaje();
            msg.codigo = -1;

            int exists_ = (from nomb in xyt.xy_sucursal
                           where nomb.NombreSucursal == NombreSucursal && nomb.IdSucursal != editSuc.IdSucursal
                           select nomb).Count();

            if (exists_ > 0)
            {
                ViewBag.Message = MensajesXynthesis.existeRegi;
                Session["mensale"] = MensajesXynthesis.existeRegi;
                Session["codigo"] = "0";
                return RedirectToAction("EditarCobertura");
            }


            try
            {
                if (ModelState.IsValid && NombreSucursal != "")
                {
                    editSuc.Ide_Geography = Convert.ToInt32(DdlUbicacion);
                    msg = adsu.EditarSucursal(editSuc, IdSucursal);
                    if (msg.codigo == 1)
                    {
                        Session["mensale"] = MensajesXynthesis.Actualiza;
                        Session["codigo"] = "1";
                    }
                    else
                    {
                        ViewData["Selgeo"] = new SelectList((from s in xyt.xy_geography where s.Nom_Country.ToUpper() == "COLOMBIA" select new { s.Ide_Geography, s.Nom_City }).ToList(), "Ide_Geography", "Nom_City", editSuc.Ide_Geography);
                        Session["mensale"] = MensajesXynthesis.existeRegi;
                        Session["codigo"] = "0";
                        return RedirectToAction("EditarSucursal");
                    }
                }
                ViewData["Selgeo"] = new SelectList((from s in xyt.xy_geography where s.Nom_Country.ToUpper() == "COLOMBIA" select new { s.Ide_Geography, s.Nom_City }).ToList(), "Ide_Geography", "Nom_City", 1);
                ViewBag.mensaje = msg;
                xy_sucursal editsucursal = xyt.xy_sucursal.Find(editSuc.IdSucursal);
                var pais = (from t in xyt.xy_geography where t.Nom_Country != "INDEFINIDO" select t);
                Session["idgeo"] = editsucursal.Ide_Geography;
                ViewData["Sucursal"] = editsucursal;
                Session["Sucursal"] = editsucursal;
                //return View("EditarSucursal", pais.ToList());
                return RedirectToAction("ListaSucursales", pais.ToList());
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = msg;
                Session["mensale"] = MensajesXynthesis.ErrDesconocido;
                Session["codigo"] = "0";
                lg.EscribaLog("SUCURSAL", "Action:EditarSucursal " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("ListaSucursales");

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
        public ActionResult EliminaTar(int id, string DdlUbicacion)
        {

            try
            {
                adsu.EliminarSucursal(id);
                ViewBag.Message = MensajesXynthesis.Elimina;
                Session["mensale"] = MensajesXynthesis.Elimina;
                Session["codigo"] = "1";
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                Session["codigo"] = "0";
                lg.EscribaLog("SUCURSAL", "Action:EliminarSucursal " + ex.Message, Session["Nom_DomainUser"].ToString());
                return Json(new { success = false });

            }

        }




        public ActionResult MostrarGeagraphic() //Accion para mostrar en dropdowlist Descripcion de Id_Geographic
        {
            var pais = (from t in xyt.xy_geography where t.Nom_Country != "INDEFINIDO" select t);
            // ViewData["ItemSel"] = pais;
            if (Session["idgeo"] != null)
                ViewData["IdGeography"] = Session["idgeo"].ToString();

            if (Session["Sucursal"] != null)
                ViewData["Sucursal"] = Session["Sucursal"];


            return PartialView(pais.ToList());
        }

        public ActionResult MostrarDropDownlist() //Accion para mostrar en dropdowlist Descripcion de Profesion pero ID 
        {
            var pais = (from t in xyt.xy_geography where t.Nom_Country != "INDEFINIDO" select t);
            return PartialView(pais.ToList());
        }


    }
}
