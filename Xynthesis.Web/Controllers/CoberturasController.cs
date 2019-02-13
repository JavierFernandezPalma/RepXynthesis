using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//Using agregados
using PagedList.Mvc;
using PagedList;
using Xynthesis.AccesoDatos;
using Xynthesis.Utilidades;
using Xynthesis.Modelo;
using Xynthesis.Utilidades.Mensajes;
namespace Xynthesis.Web.Controllers
{
    public class CoberturasController : Controller
    {
        LogXynthesis lg = new LogXynthesis();
        ADCoberturas cobertura = new ADCoberturas();
        xynthesisEntities xyt = new xynthesisEntities();
        Constantes cons = new Constantes();
        public ActionResult ListaCoberturas(int? page)
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
                return View("ListaCoberturas", cobertura.listacoberturas().ToList().ToPagedList(pageIndex, pageSize));

            }
            catch (Exception ex)
            {

                lg.EscribaLog("LISTA DE COBERTURAS", "Action:ListaCoberturas " + ex.Message, Session["Nom_DomainUser"].ToString());
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
                var res = cobertura.OrdenFiltro(sortOrder, searchString);

                var pagedList1 = res.ToPagedList(pageIndex, pageSize);
                return View("ListaCoberturas", pagedList1);
            }
            catch (Exception ex)
            {
                lg.EscribaLog("ORDEN FILTRO", "Action:OrdenFiltro " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult NuevaCobertura()
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            return View();
        }
        [HttpPost]
        public ActionResult NuevaCobertura(xy_coverage nuevo, string Movil, string Nacional, string Internacional, string Nom_Coverage)
        {

            bool exists = (from nomb in xyt.xy_coverage
                           where nomb.Nom_Coverage == Nom_Coverage
                           select nomb).Any();

            if (exists == true)
            {
                ViewBag.Message = MensajesXynthesis.existeRegi;
                Session["mensale"] = MensajesXynthesis.existeRegi;
                Session["codigo"] = "0";
                return View("NuevaCobertura");
            }


            try
            {
                if (Movil != null)
                    if (Movil.Equals("on"))
                        nuevo.Movil = true;
                    else
                        nuevo.Movil = false;
                else
                    nuevo.Movil = false;

                if (Nacional != null)
                    if (Nacional.Equals("on"))
                        nuevo.Nacional = true;
                    else
                        nuevo.Nacional = false;
                else
                    nuevo.Nacional = false;

                if (Internacional != null)
                    if (Internacional.Equals("on"))
                        nuevo.Internacional = true;
                    else
                        nuevo.Internacional = false;
                else
                    nuevo.Internacional = false;

                if (ModelState.IsValid)
                { }
                if (nuevo.Nom_Coverage != "" && nuevo.Nom_Coverage != null)
                {
                    cobertura.NuevaCobertura(nuevo);
                    TempData["mensajecorrecto"] = "correcto";
                    TempData["mensajeerror"] = null;
                    Session["mensale"] = MensajesXynthesis.Nuevo;
                    Session["codigo"] = "1";

                }
                else
                {
                    TempData["mensajecorrecto"] = null;
                    TempData["mensajeerror"] = "Error";
                    Session["mensale"] = MensajesXynthesis.NoProcesa;
                    Session["codigo"] = "0";
                }
                //return RedirectToAction("NuevaCobertura");
                return RedirectToAction("ListaCoberturas");
            }
            catch (Exception)
            {
                Session["mensale"] = MensajesXynthesis.ErrDesconocido;
                Session["codigo"] = "0";
                TempData["mensajeerror"] = "error";
                return RedirectToAction("ListaCoberturas");
            }
        }

        public ActionResult EditarCobertura(int id)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            try
            {
                xy_coverage editarCobertura = xyt.xy_coverage.Find(id);
                return View(editarCobertura);
            }
            catch (Exception error)
            {
                lg.EscribaLog("EDITAR SUCURSALES", "Action:EditarCobertura " + error.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        public ActionResult EditarCobertura(xy_coverage update, int Ide_Coverage, String Nom_Coverage, string Movil, string Nacional, string Internacional)
        {


            //bool exists = (from nomb in xyt.xy_coverage
            //               where nomb.Nom_Coverage == Nom_Coverage
            //               select nomb).Any();

            int exists_ = (from nomb in xyt.xy_coverage
                           where nomb.Nom_Coverage == Nom_Coverage && nomb.Ide_Coverage != Ide_Coverage
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
                xy_coverage actualizaCobertura = xyt.xy_coverage.Find(update.Ide_Coverage);

                actualizaCobertura.Nom_Coverage = update.Nom_Coverage;

                if (Movil != null)
                    if (Movil.Equals("on"))
                        actualizaCobertura.Movil = true;
                    else
                        actualizaCobertura.Movil = false;
                else
                    actualizaCobertura.Movil = false;

                if (Nacional != null)
                    if (Nacional.Equals("on"))
                        actualizaCobertura.Nacional = true;
                    else
                        actualizaCobertura.Nacional = false;
                else
                    actualizaCobertura.Nacional = false;

                if (Internacional != null)
                    if (Internacional.Equals("on"))
                        actualizaCobertura.Internacional = true;
                    else
                        actualizaCobertura.Internacional = false;
                else
                    actualizaCobertura.Internacional = false;

                if (ModelState.IsValid)
                { }


                if (Nom_Coverage != "")
                {
                    cobertura.EditarCobertura(Ide_Coverage, Nom_Coverage, Movil, Nacional, Internacional);
                    TempData["mensajecorrecto"] = "correcto";
                    TempData["mensajeerror"] = null;
                    Session["mensale"] = MensajesXynthesis.Actualiza;
                    Session["codigo"] = "1";
                }
                else
                {
                    TempData["mensajecorrecto"] = null;
                    TempData["mensajeerror"] = "mensajeerror";
                    Session["mensale"] = MensajesXynthesis.NoProcesa;
                    Session["codigo"] = "0";
                }

                return RedirectToAction("ListaCoberturas");
                //return RedirectToAction("EditarCobertura");
            }
            catch (Exception)
            {
                Session["mensale"] = MensajesXynthesis.ErrDesconocido;
                Session["codigo"] = "0";
                TempData["mensajeerror"] = "error";
                return RedirectToAction("ListaCoberturas");
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
        public ActionResult EliminarRegistro(int id, int ?page)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            try
            {
                var vali = (from s in xyt.xy_rates where s.Ide_Coverage == id select s).ToList();
                if (vali.Count == 0)
                {
                    var res = (from s in xyt.xy_operators where s.Ide_Coverage == id select s).ToList();
                    if (res.Count == 0)
                    {
                        // sino hay tarifas permitir eliminarlas
                        cobertura.EliminarCobertura(id);
                        ViewBag.mensale = MensajesXynthesis.Elimina;
                        Session["mensale"] = MensajesXynthesis.Elimina;
                        Session["codigo"] = "1";
                        //TempData["mensajecorrecto"] = "correcto";
                        //TempData["mensajeerror"] = null;
                    }
                    else
                    {
                        ViewBag.mensale = MensajesXynthesis.NoProcesa;
                        Session["mensale"] = MensajesXynthesis.NoProcesa;
                        Session["codigo"] = "0";
                        //TempData["mensajecorrecto"] = null;
                        //TempData["mensajeerror"] = "mensajeerror";

                    }
                }
                else
                {
                    ViewBag.mensale = MensajesXynthesis.NoProcesa;
                    Session["mensale"] = MensajesXynthesis.NoProcesa;
                    Session["codigo"] = "0";
                    //TempData["mensajecorrecto"] = null;
                    //TempData["mensajeerror"] = "mensajeerror";
                }
                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageIndex = 1;
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                return Json(new { success = true });
                //return View("ListaCoberturas", cobertura.listacoberturas().ToList().ToPagedList(pageIndex, pageSize));
            }
            catch (Exception ex)
            {
                Session["mensale"] = MensajesXynthesis.ErrDesconocido;
                Session["codigo"] = "0";
                lg.EscribaLog("ELIMINAR COBERTURA", "Action:EliminarCobertura " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
         
        }

        public ActionResult EliminarCobertura(int? id, bool? saveChangesError = false)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            xy_coverage coverage = xyt.xy_coverage.Find(id);
            if (coverage == null)
            {
                return HttpNotFound();
            }
            return View(coverage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarCobertura(int id)
        {            
            try
            {
                var vali = (from s in xyt.xy_rates where s.Ide_Coverage == id  select s).ToList();
                if (vali.Count == 0)
                {
                    var res = (from s in xyt.xy_operators where s.Ide_Coverage == id select s).ToList();
                    if (res.Count == 0)
                    {
                        // sino hay tarifas permitir eliminarlas
                        cobertura.EliminarCobertura(id);
                        TempData["mensajecorrecto"] = "correcto";
                        TempData["mensajeerror"] = null;
                        Session["mensale"] = MensajesXynthesis.Elimina;
                        Session["codigo"] = "1";
                    }
                    else
                    {
                        Session["mensale"] = MensajesXynthesis.NoProcesa;
                        Session["codigo"] = "0";
                        TempData["mensajecorrecto"] = null;
                        TempData["mensajeerror"] = "mensajeerror";
                    }
                }
                else
                {
                    TempData["mensajecorrecto"] = null;
                    TempData["mensajeerror"] = "mensajeerror";
                    Session["mensale"] = MensajesXynthesis.NoProcesa;
                    Session["codigo"] = "0";
                }
            }
            catch (Exception ex)
            {
                lg.EscribaLog("ELIMINAR COBERTURA", "Action:EliminarCobertura " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
            return RedirectToAction("ListaCoberturas");
        }
    }
}