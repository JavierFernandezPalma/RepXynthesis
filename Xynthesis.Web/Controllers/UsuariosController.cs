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
using System.Net;
using System.Configuration;
using Xynthesis.Utilidades.Mensajes;
namespace Xynthesis.Web.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: Subscriptor
        ADUsuarios Usuarios = new ADUsuarios();
        LogXynthesis lg = new LogXynthesis();
        xynthesisEntities xyt = new xynthesisEntities();
        Constantes cons = new Constantes();
        Mensaje msg = new Mensaje();

        public ActionResult Index(int? page)
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
                return View("index", Usuarios.ObtenerListaUsuarios().ToList().ToPagedList(pageIndex, pageSize));

            }
            catch (Exception ex)
            {

                lg.EscribaLog("LISTA DE USUARIO", "Action:Index " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult NuevoUsuario()
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            if (Convert.ToInt32(Session["Id_Rol"]) > 1)
            {
                ViewBag.Message = MensajesXynthesis.NoPermisos;
                Session["mensale"] = MensajesXynthesis.NoPermisos;
                Session["codigo"] = "0";
                return RedirectToAction("Index");
            }
            ViewBag.Sucursal = new SelectList(xyt.xy_sucursal.ToList(), "IdSucursal", "NombreSucursal");
            ViewBag.Area = new SelectList(xyt.xy_costcenters.ToList(), "Ide_CostCenter", "Nom_CostCenter");
            ViewBag.Rol = new SelectList(xyt.xy_rol.ToList(), "Ide_Rol", "Nom_Rol");
            try
            {
                List<SelectListItem> item = new List<SelectListItem>();
                item.Add(new SelectListItem
                {
                    Text = "EMPLEADO"
                });
                item.Add(new SelectListItem
                {
                    Text = "CLIENTE"
                });

                ViewData["listaTipo"] = item;
                return View();

            }
            catch (Exception ex)
            {
                lg.EscribaLog("NUEVO USUARIO", "Action:NuevoUsuario " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoUsuario([Bind(Include = "Ide_Subscriber,Nom_Subscriber,IdSucursal, Id_Rol,Ide_CostCenter,Nom_DomainUser,Tip_Subscriber")]  xy_subscriber xy_subscriber, string DdlTipo, int IdSucursal, int Ide_CostCenter, string Str_Password, string Str_Password_, string Nom_Subscriber, string Nom_DomainUser, string Email, int Id_Rol)
        {
            DateTime Hoy = DateTime.Now;
            string fecha_actual = Hoy.ToString("dd-MM-yyyy HH:mm");
            xy_subscriber.FechaCreacion = fecha_actual;
            xy_subscriber.Email = Email;

            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            bool exists = (from nomb in xyt.xy_subscriber
                           where nomb.Nom_DomainUser == Nom_DomainUser || nomb.Nom_Subscriber == Nom_Subscriber || nomb.Email == Email
                           select nomb).Any();

            if (exists == true)
            {
                ViewBag.Message = MensajesXynthesis.CorreoUsuario;
                Session["mensale"] = MensajesXynthesis.CorreoUsuario;
                Session["codigo"] = "0";
                ViewBag.Sucursal = new SelectList(xyt.xy_sucursal.ToList(), "IdSucursal", "NombreSucursal");
                ViewBag.Area = new SelectList(xyt.xy_costcenters.ToList(), "Ide_CostCenter", "Nom_CostCenter");
                ViewBag.Rol = new SelectList(xyt.xy_rol.ToList(), "Ide_Rol", "Nom_Rol");
                List<SelectListItem> item = new List<SelectListItem>();
                item.Add(new SelectListItem
                {
                    Text = "EMPLEADO"
                });
                item.Add(new SelectListItem
                {
                    Text = "CLIENTE"
                });

                ViewData["listaTipo"] = item;
                return View("NuevoUsuario");
            }

            try
            {
                ViewBag.Sucursal = new SelectList(xyt.xy_sucursal.ToList(), "IdSucursal", "NombreSucursal");
                ViewBag.Area = new SelectList(xyt.xy_costcenters.ToList(), "Ide_CostCenter", "Nom_CostCenter");

                List<SelectListItem> item = new List<SelectListItem>();
                item.Add(new SelectListItem
                {
                    Text = "EMPLEADO"
                });
                item.Add(new SelectListItem
                {
                    Text = "CLIENTE"
                });
                string nomUsuario = xy_subscriber.Nom_Subscriber;
                string DomUsuario = xy_subscriber.Nom_DomainUser;
                ViewData["listaTipo"] = item;
                int conteo = (from nxs in xyt.xy_subscriber where nxs.Nom_Subscriber.ToUpper().Equals(nomUsuario.ToUpper()) || nxs.Nom_DomainUser.ToUpper().Equals(DomUsuario.ToUpper()) select nxs).Count();
                if (conteo > 0)
                {
                    ViewBag.Message = MensajesXynthesis.existeRegi;
                    Session["mensale"] = MensajesXynthesis.existeRegi;
                    Session["codigo"] = "0";
                    //Session["usuarioexistente"] = MensajesXynthesis.existeRegi;
                    return RedirectToAction("Index");
                }
                else
                {
                    string key = ConfigurationManager.AppSettings["KeyCifradoXynthesis"];
                    CifradoClaves cif = new CifradoClaves();
                    String Error = "";
                    if (Str_Password != Str_Password_)
                    {
                        Error += MensajesXynthesis.NoPass;
                    }

                    if (Str_Password == Str_Password_)
                    {
                        ViewBag.message = "";
                        xy_subscriber.Cod_Subscriber = xy_subscriber.Nom_Subscriber;
                        xy_subscriber.Str_Password = cif.EncryptText(Str_Password, key);
                        if (DdlTipo == "EMPLEADO")
                            xy_subscriber.Tip_Subscriber = "E";
                        else
                            xy_subscriber.Tip_Subscriber = "C";
                        if (Nom_Subscriber != "")
                        {
                            Usuarios.NuevaUsuario(xy_subscriber);
                            xyt.xyp_RolUser(Id_Rol, Convert.ToInt32(xy_subscriber.Ide_Subscriber));
                            ViewBag.message_ok = MensajesXynthesis.Nuevo;
                            Session["mensale"] = MensajesXynthesis.Nuevo;
                            Session["codigo"] = "1";
                            //Session["nuevousuario"] = MensajesXynthesis.Actualiza;
                        }
                        else
                        {
                            ViewBag.message_ok = "";
                            @ViewBag.message = MensajesXynthesis.NoProcesa;
                            Session["mensale"] = MensajesXynthesis.NoProcesa;
                            Session["codigo"] = "0";
                        }
                        return RedirectToAction("Index");
                    }

                    return View("Index");

                }
            }
            catch (Exception ex)
            {
                Session["codigo"] = "0";
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                lg.EscribaLog("NUEVO USUARIO", "Action:NuevoUsuario " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }

        }

        public ActionResult EditarUsuario(int? id)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            if (Convert.ToInt32(Session["Id_Rol"]) > 1)
            {
                ViewBag.Message = MensajesXynthesis.NoPermisos;
                Session["mensale"] = MensajesXynthesis.NoPermisos;
                Session["codigo"] = "0";
                return RedirectToAction("Index");
            }
            ViewBag.Sucursal = new SelectList(xyt.xy_sucursal.ToList(), "IdSucursal", "NombreSucursal");
            ViewBag.Area = new SelectList(xyt.xy_costcenters.ToList(), "Ide_CostCenter", "Nom_CostCenter");
            ViewBag.Rol = new SelectList(xyt.xy_rol.ToList(), "Ide_Rol", "Nom_Rol");

            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                xy_subscriber xy_subscriber = xyt.xy_subscriber.Find(id);
                if (xy_subscriber == null)
                {
                    return HttpNotFound();
                }
                return View(xy_subscriber);

            }
            catch (Exception ex)
            {
                lg.EscribaLog("EDITAR USUARIO", "Action:EditarUsuario " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuario(xy_subscriber xy_subscriber, string Ide_Subscriber, int Id_Rol, int IdSucursal, int Ide_CostCenter, string Tip_Subscriber, string Str_Password, String Nom_Subscriber, string Nom_DomainUser, string Email)
        {
           
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            int exists_ = (from nomb in xyt.xy_subscriber
                           where nomb.Email == xy_subscriber.Email && nomb.Ide_Subscriber != xy_subscriber.Ide_Subscriber
                           select nomb).Count();

            if (exists_ > 0)
            {
                Session["mensale"] = MensajesXynthesis.CorreoExistente;
                Session["codigo"] = "0";
                return RedirectToAction("EditarUsuario");
            }


            try
            {
                CifradoClaves cc = new CifradoClaves();
                string key = ConfigurationManager.AppSettings["KeyCifradoXynthesis"];
                Int64 idsus = Convert.ToInt64(Ide_Subscriber);
                ViewBag.Sucursal = new SelectList(xyt.xy_sucursal.ToList(), "IdSucursal", "NombreSucursal");
                ViewBag.Area = new SelectList(xyt.xy_costcenters.ToList(), "Ide_CostCenter", "Nom_CostCenter");

                xy_subscriber sus = new xy_subscriber();
                sus = xyt.xy_subscriber.Find(Convert.ToInt32(Ide_Subscriber));
                if (Tip_Subscriber.ToString() == "E")
                {
                    sus.Tip_Subscriber = "E";
                }
                else
                {
                    sus.Tip_Subscriber = "C";
                }
                if (ModelState.IsValid)
                { }
                if (Nom_Subscriber != "")
                {
                    Usuarios.EditarUsuario(Ide_Subscriber, Nom_Subscriber, IdSucursal, Ide_CostCenter, Nom_DomainUser, sus.Tip_Subscriber, Id_Rol, Email);
                    xyt.xyp_UpdRolUser(Id_Rol, Convert.ToInt32(Ide_Subscriber));
                    Session["mensale"] = MensajesXynthesis.Actualiza;
                    Session["codigo"] = "1";
                }
                else
                {
                    Session["mensale"] = MensajesXynthesis.NoProcesa;
                    Session["codigo"] = "0";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Session["mensale"] = MensajesXynthesis.ErrDesconocido;
                Session["codigo"] = "0";
                lg.EscribaLog("EDITAR USUARIO", "Action:EditarUsuario " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }

        }

        public ActionResult EliminarRegistro(long? id)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            try
            {
                if (Convert.ToInt32(Session["Id_Rol"]) > 1)
                {
                    ViewBag.Message = MensajesXynthesis.NoPermisos;
                    Session["mensale"] = MensajesXynthesis.NoPermisos;
                    Session["codigo"] = "0";
                    return RedirectToAction("EliminarRegistro");
                }
                else
                {
                    return PartialView("../PopupDel/EliminarRegistro");
                }
            }
            catch (Exception ex)
            {
                lg.EscribaLog("ELIMINAR USUARIO", "Action:EliminarUsuario " + ex.Message, Session["Nom_DomainUser"].ToString());
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                Session["codigo"] = "0";
                return Json(new { success = false });
                ;
            }
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

            ViewBag.Sucursal = new SelectList(xyt.xy_sucursal.ToList(), "IdSucursal", "NombreSucursal");
            ViewBag.Area = new SelectList(xyt.xy_costcenters.ToList(), "Ide_CostCenter", "Nom_CostCenter");

            try
            {
                ViewBag.Message = "";

                int verif = (from nxs in xyt.xy_numbersxsubscriber where nxs.Ide_Subscriber == id select nxs).Count();
                xy_subscriber xy_subscriber = xyt.xy_subscriber.Find(id);
                if (verif > 0)
                {
                    ViewBag.Message = MensajesXynthesis.NoProcesa;
                    Session["mensale"] = MensajesXynthesis.NoProcesa;
                    Session["codigo"] = "0";
                }
                else
                {

                    int verifica = xyt.xy_subscriber.Where(i => i.Ide_Subscriber == id).Count();
                    if (verifica > 0)
                    {

                        Usuarios.EliminarUsuario(id);
                        xyt.xyp_DelRolUser(Convert.ToInt32(id));
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

                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                lg.EscribaLog("ELIMINAR USUARIO", "Action:EliminarUsuario " + ex.Message, Session["Nom_DomainUser"].ToString());
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                Session["codigo"] = "0";
                return Json(new { success = false });

            }


        }

        public ActionResult EliminarUsuario(long? id)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            ViewBag.Sucursal = new SelectList(xyt.xy_sucursal.ToList(), "IdSucursal", "NombreSucursal");
            ViewBag.Area = new SelectList(xyt.xy_costcenters.ToList(), "Ide_CostCenter", "Nom_CostCenter");

            try
            {
                if (id == null)
                {
                    ViewBag.mensaje = msg;
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                xy_subscriber xy_subscriber = xyt.xy_subscriber.Find(id);
                if (xy_subscriber == null)
                {
                    ViewBag.mensaje = msg;
                    return HttpNotFound();
                }
                ViewBag.mensaje = msg;
                return View(xy_subscriber);
            }
            catch (Exception ex)
            {
                lg.EscribaLog("ELIMINAR USUARIO", "Action:EliminarUsuario " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }

        }


        [HttpPost, ActionName("EliminarUsuario")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {

                return RedirectToAction("Login", "Acceso");
            }

            ViewBag.Sucursal = new SelectList(xyt.xy_sucursal.ToList(), "IdSucursal", "NombreSucursal");
            ViewBag.Area = new SelectList(xyt.xy_costcenters.ToList(), "Ide_CostCenter", "Nom_CostCenter");

            try
            {
                ViewBag.Message = "";

                int verif = (from nxs in xyt.xy_numbersxsubscriber where nxs.Ide_Subscriber == id select nxs).Count();
                xy_subscriber xy_subscriber = xyt.xy_subscriber.Find(id);
                if (verif > 0)
                    ViewBag.Message = MensajesXynthesis.NoProcesa;
                else
                {

                    int verifica = xyt.xy_subscriber.Where(i => i.Ide_Subscriber == id).Count();
                    if (verifica > 0)
                    {

                        Usuarios.EliminarUsuario(id);
                        ViewBag.Message = MensajesXynthesis.Elimina;
                    }
                    else
                    {
                        ViewBag.Message = MensajesXynthesis.NoProcesa;
                    }

                }
                if (ViewBag.Message != "")
                {
                    xy_subscriber = new xy_subscriber();
                    return View("EliminarUsuario", xy_subscriber);
                }
                else
                    return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                lg.EscribaLog("ELIMINAR USUARIO", "Action:EliminarUsuario " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Index");
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
                var res = Usuarios.OrdenFiltro(sortOrder, searchString);

                var pagedList1 = res.ToPagedList(pageIndex, pageSize);
                return View("index", pagedList1);
            }
            catch (Exception ex)
            {
                lg.EscribaLog("ORDEN FILTRO", "Action:OrdenFiltro " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }


        //===========================Metodos para mostrar los dropdownlist en Lista =======================================

        public static string DescripcionArea(int? Id) //Metodo para traer la Descripcion de Area atravez del id
        {
            if (Id < 1)
            {
                return "";
            }

            using (var xyt = new xynthesisEntities())
            {

                xy_costcenters area = xyt.xy_costcenters.Find(Id);
                if (area == null)
                    return "";
                else
                    return area.Nom_CostCenter;
            }
        }
        //===========================Metodos para mostrar los dropdownlist en Lista =======================================

        public static string DescripcionSucursal(int? Id) //Metodo para traer la Descripcion de Sucursal atravez del id
        {
            try
            {
                if (Id < 1)
                {
                    return "";
                }

                using (var xyt = new xynthesisEntities())
                {
                    xy_sucursal suc = xyt.xy_sucursal.Find(Id);
                    if (suc == null)
                        return "";
                    else
                        return suc.NombreSucursal;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        //===========================Metodos para mostrar los dropdownlist en Lista =======================================

        public static string DescripcionRol(int? Id) //Metodo para traer la Descripcion de Rol atravez del id
        {
            using (var xyt = new xynthesisEntities())
            {
                xy_rol suc = xyt.xy_rol.Find(Id);
                if (suc == null)
                    return "";
                else
                    return suc.Nom_Rol;
            }
        }
        //===========================Metodos para mostrar los dropdownlist en Lista =======================================


    }
}