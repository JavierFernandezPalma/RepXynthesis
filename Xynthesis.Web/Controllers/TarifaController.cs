using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using Xynthesis.AccesoDatos;
using Xynthesis.Utilidades;
using Xynthesis.Modelo;
using Xynthesis.Utilidades.Mensajes;

namespace Xynthesis.Web.Controllers
{
    public class TarifaController : Controller
    {
        ADTarifa tarifas = new ADTarifa();
        LogXynthesis lg = new LogXynthesis();
        Constantes cons = new Constantes();
        xynthesisEntities xyt = new xynthesisEntities();

        public ActionResult ListaTarifas(int? page)
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
                return View("ListaTarifas", tarifas.ObtenerListaTarifa().ToList().ToPagedList(pageIndex, pageSize));

            }
            catch (Exception ex)
            {
                lg.EscribaLog("LISTA DE TARIFAS", "Action:ListaTarifas " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
            
        }

        public ActionResult NuevaTarifa()
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            //ViewBag.selCoberage = new SelectList(xyt.xy_coverage.Where(g => g.Ide_Coverage == 4).ToList(), "Ide_Coverage", "Nom_Coverage");
            ViewBag.selCoberage = new SelectList(xyt.xy_coverage.Where(g => g.Movil == true).ToList(), "Ide_Coverage", "Nom_Coverage");
            ViewBag.selOperator = new SelectList(xyt.xy_operators.Where(g => g.Nom_Operator != "INDEFINIDO" && g.Ide_Coverage == 4).ToList(), "Ide_Operator", "Nom_Operator");

            var nuevo = new xy_rates();
            return View("NuevaTarifa", nuevo);
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
                var res = tarifas.OrdenFiltro(sortOrder, searchString);

                var pagedList1 = res.ToPagedList(pageIndex, pageSize);
                return View("ListaTarifas", pagedList1);
            }
            catch (Exception ex)
            {
                lg.EscribaLog("ORDEN FILTRO", "Action:OrdenFiltro " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult NuevaTarifa(xy_rates nuevo, string Des_Rate)
        {

            
            bool exists = (from nomb in xyt.xy_rates
                           where nomb.Des_Rate == Des_Rate  
                           select nomb).Any();

            if (exists == true)
            {
                ViewBag.Message = MensajesXynthesis.existeRegi;
                Session["mensale"] = MensajesXynthesis.existeRegi;
                Session["codigo"] = "0";
                return RedirectToAction("NuevaTarifa");
            }


            try
            {
                //public JsonResult NuevaTarifa(xy_rates nuevo)
                if (ModelState.IsValid)
                {
                    tarifas.NuevaTar(nuevo);
                    TempData["mensaje"] = MensajesXynthesis.Nuevo;
                    Session["mensale"] = MensajesXynthesis.Nuevo;
                    Session["codigo"] = "1";
                
                   // return Json(new { success = true });
                }
                return RedirectToAction("ListaTarifas");
                //return Json(nuevo, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Session["mensale"] = MensajesXynthesis.ErrDesconocido;
                Session["codigo"] = "0";
                return RedirectToAction("ListaTarifas");
            }
        }



        public ActionResult EditarTarifa(int id = 0)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            ViewBag.selCoberage = new SelectList(xyt.xy_coverage.Where(g => g.Movil == true).ToList(), "Ide_Coverage", "Nom_Coverage");
            ViewBag.selOperator = new SelectList(xyt.xy_operators.Where(g => g.Nom_Operator != "INDEFINIDO" && g.Ide_Coverage == 4).ToList(), "Ide_Operator", "Nom_Operator");

            var edit = xyt.xy_rates.Find(id);
            if (edit == null)
            {
                return HttpNotFound();
            }
            // return PartialView("EditarTarifa", edit);
            return View("EditarTarifa", edit);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarTarifa(xy_rates update)
        {

            //bool exists = (from nomb in xyt.xy_rates
            //               where nomb.Des_Rate == update.Des_Rate
            //               select nomb).Any();

            int exists_ = (from nomb in xyt.xy_rates
                           where nomb.Des_Rate == update.Des_Rate && nomb.Ide_Rate != update.Ide_Rate
                           select nomb).Count();

            if (exists_ > 0)
            {
                ViewBag.Message = MensajesXynthesis.existeRegi;
                Session["mensale"] = MensajesXynthesis.existeRegi;
                Session["codigo"] = "0";
                return RedirectToAction("EditarTarifa");
            }


            try
            {
                ViewBag.selCoberage = new SelectList(xyt.xy_coverage.Where(g => g.Movil == true).ToList(), "Ide_Coverage", "Nom_Coverage");
                ViewBag.selOperator = new SelectList(xyt.xy_operators.Where(g => g.Nom_Operator != "INDEFINIDO" && g.Ide_Coverage == 4).ToList(), "Ide_Operator", "Nom_Operator");

                tarifas.EditarTarifa(update);
                xyt.SaveChanges();
                @ViewBag.message = MensajesXynthesis.Actualiza;
                @ViewBag.message_ok = "";
                Session["mensale"] = MensajesXynthesis.Actualiza;
                Session["codigo"] = "1";
                return RedirectToAction("ListaTarifas");
               // return View("EditarTarifa", update);

            }
            catch (Exception ex)
            {
                Session["mensale"] = MensajesXynthesis.ErrDesconocido;
                Session["codigo"] = "0";
                @ViewBag.message = MensajesXynthesis.ErrDesconocido;
                @ViewBag.message_ok = "";
                return RedirectToAction("ListaTarifas");
                //return View("EditarTarifa", update);
                throw ex;
            }
        }



        public ActionResult EliminarTarifa(int id = 0)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            ViewBag.selCoberage = new SelectList(xyt.xy_coverage.ToList(), "Ide_Coverage", "Nom_Coverage");
            ViewBag.selOperator = new SelectList(xyt.xy_operators.Where(g => g.Nom_Operator != "INDEFINIDO").ToList(), "Ide_Operator", "Nom_Operator");

            xy_rates eli = xyt.xy_rates.Find(id);
            if (eli == null)
            {
                return HttpNotFound();
            }
            return PartialView("EliminarTarifa", eli);
        }

        [HttpPost, ActionName("EliminarTarifa")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminaTar(int id)
        {
            tarifas.EliminarTarifa(id);
            return Json(new { success = true });
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
                int res =  tarifas.EliminarTarifa(id);
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
                Session["mensale"] = MensajesXynthesis.ErrDesconocido;
                Session["codigo"] = "0";
                return Json(new { success = true });
            }

        }


        //===========================Metodos para mostrar los dropdownlist en Lista =======================================

        public static string DescripcionOperador(int? id) //Metodo para traer la Descripcion de Operador atravez del id
        {
            using (var xyt = new xynthesisEntities())
            {
                var nom = (from s in xyt.xy_operators where s.Ide_Operator == (int?)id select s).ToList();
                if(nom.Count  ==0)
                    return "";
                else
                 return xyt.xy_operators.Find(id).Nom_Operator;

            }
        }

 

     }
}