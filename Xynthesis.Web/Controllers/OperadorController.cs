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
    public class OperadorController : Controller
    {
        Utilidades.Constantes cons = new Constantes();
        AccesoDatos.ADOperador operador = new ADOperador();
        Utilidades.LogXynthesis log = new LogXynthesis();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        xynthesisEntities xyt = new xynthesisEntities();

        // GET: Operador
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
                var xy_operators_ = operador.ObtenerListaOperadores();

                IPagedList<xy_operators> oper = null;
                oper = xy_operators_.ToList().ToPagedList(pageIndex, pageSize);
                return View("index", oper);
            }
            catch (Exception ex)
            {
                log.EscribaLog("OPERADOR", "Action:Index_Get " + ex.Message, Session["Nom_DomainUser"].ToString());
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
                var res = operador.OrdenFiltro(sortOrder, searchString);

                var pagedList1 = res.ToPagedList(pageIndex, pageSize);
                return View("index", pagedList1);
            }
            catch (Exception ex)
            {
                log.EscribaLog("OPERADOR", "Action:OrdenFiltro " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult NuevoOperador()
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            msg = new Mensaje();
            msg.codigo = -1;
            try
            {
                xy_operators oper = new xy_operators();
                ViewBag.mensaje = msg;
                return View(oper);

            }
            catch (Exception ex)
            {
                ViewBag.mensaje = msg;
                throw ex;
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoOperador([Bind(Include = "Ide_Operator,Cod_Operator,Nom_Operator,Ide_Coverage")] xy_operators xy_operators, string Nom_Operator, string Cod_Operator)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            try
            {
                if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
                {
                    return RedirectToAction("Login", "Acceso");
                }
                if (ModelState.IsValid && xy_operators.Nom_Operator!="")
                {
                    bool exists = (from nomb in xyt.xy_operators
                                   where nomb.Nom_Operator == Nom_Operator
                                 select nomb).Any();
                    bool exists2 = (from codOpe in xyt.xy_operators
                                    where codOpe.Cod_Operator == Cod_Operator
                                    select codOpe).Any();


                    if (exists == true || (Cod_Operator != "" && exists2 == true))
                    {
                        ViewBag.Message = MensajesXynthesis.existeRegi;
                        Session["mensale"] = MensajesXynthesis.existeRegi;
                        Session["codigo"] = "0";
                        return RedirectToAction("NuevoOperador");
                    }
                    //msg = operador.nuevoOperador(xy_operators);
                    operador.nuevoOperador(xy_operators);
                    ViewBag.message_ok = MensajesXynthesis.Nuevo;
                    Session["mensale"] = MensajesXynthesis.Nuevo;
                    Session["codigo"] = "1";

                    return RedirectToAction("Index");
                }
                ViewBag.mensaje = msg;
                return View(xy_operators);

            }
            catch (Exception ex)
            {
                ViewBag.mensaje = msg;
                Session["codigo"] = "0";
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                log.EscribaLog("OPERADOR", "Action:Create_Post " + ex.Message, Session["Nom_DomainUser"].ToString());
                return View(xy_operators);
            }
        }

        public ActionResult EditarOperador(int id)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            try
            {
                if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
                {
                    return RedirectToAction("Login", "Acceso");
                }
                if (id.Equals(null))
                {
                    ViewBag.mensaje = msg;
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                xy_operators xy_operators = operador.buscarOperadorxId(id);         //db.xy_operators.Find(id);
                if (xy_operators == null)
                {
                    ViewBag.mensaje = msg;
                    return HttpNotFound();
                }
                ViewBag.mensaje = msg;
                return View(xy_operators);

            }
            catch (Exception ex)
            {

                log.EscribaLog("OPERADOR", "Action:Edit_Get " + ex.Message, Session["Nom_DomainUser"].ToString());
                ViewBag.mensaje = msg;
                return RedirectToAction("Error", "Error");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarOperador([Bind(Include = "Ide_Operator,Cod_Operator,Nom_Operator,Ide_Coverage")] xy_operators xy_operators, string Cod_Operator)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            try
            {
                if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
                {
                    return RedirectToAction("Login", "Acceso");
                }

                int exists_ = (from nomb in xyt.xy_operators
                               where nomb.Nom_Operator == xy_operators.Nom_Operator && nomb.Ide_Operator != xy_operators.Ide_Operator
                               select nomb).Count();

                bool exists2 = (from codOpe in xyt.xy_operators
                                where codOpe.Cod_Operator == Cod_Operator
                                select codOpe).Any();                

                //bool exists = (from nomb in xyt.xy_operators
                //               where nomb.Nom_Operator == xy_operators.Nom_Operator
                //               select nomb).Any();

                //if (exists == true)
                if (exists_ > 0 || (Cod_Operator != "" && exists2 == true))
                {
                    ViewBag.Message = MensajesXynthesis.existeRegi;
                    Session["mensale"] = MensajesXynthesis.existeRegi;
                    Session["codigo"] = "0";
                    return View("EditarOperador");
                }


                if (ModelState.IsValid && xy_operators.Nom_Operator!="")
                {
                    msg = operador.guardarEdicion(xy_operators);
                    ViewBag.mensaje = msg;
                    Session["mensale"] = MensajesXynthesis.Actualiza;
                    Session["codigo"] = "1";
                    return RedirectToAction("Index");
                }
                ViewBag.mensaje = msg;
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                Session["codigo"] = "0";
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ViewBag.mensaje = msg;
                Session["mensale"] = MensajesXynthesis.ErrDesconocido;
                Session["codigo"] = "0";
                log.EscribaLog("OPERADOR", "Action:Edit_Get " + ex.Message, Session["Nom_DomainUser"].ToString());
                return View(xy_operators);
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

            xy_operators oper;
            try
            {
                if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
                {
                    return RedirectToAction("Login", "Acceso");
                }
                msg = operador.EliminarOperador(id);
                ViewBag.mensaje = msg;
                //ViewBag.Message = MensajesXynthesis.Elimina;
                Session["mensale"] = msg.mensaje;
                Session["codigo"] = msg.codigo.ToString();
                if (msg.codigo == 1)
                    oper = new xy_operators();
                else
                {
                    oper = operador.buscarOperadorxId(id);
                }
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                log.EscribaLog("OPERADOR", "Action:DeleteConfirmed " + ex.Message, Session["Nom_DomainUser"].ToString());
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                Session["codigo"] = "0";
                return Json(new { success = false });
            }

            
        }





    }
}