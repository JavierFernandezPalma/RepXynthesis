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
using Xynthesis.Utilidades.Mensajes;

namespace Xynthesis.Web.Controllers
{
    public class NumerosController : Controller
    {

        Utilidades.Constantes cons = new Constantes();
        AccesoDatos.ADNumeros numero = new ADNumeros();
        AccesoDatos.ADUsuarios usua = new ADUsuarios();
        Utilidades.LogXynthesis log = new LogXynthesis();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();

        string identificador;

        // GET: Numeros
        public ActionResult Index(int? page)
        {
            try
            {
                if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
                {
                    return RedirectToAction("Login", "Acceso");
                }
                string MaxRegGrilla = cons.MaxRegGrilla;
                int pageSize = MaxRegGrilla == null ? 8 : Convert.ToInt32(MaxRegGrilla);
                int pageIndex = 1;
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                return View("Index", numero.ObtenerListaNumeros().ToPagedList(pageIndex, pageSize));
            }
            catch (Exception ex)
            {
                log.EscribaLog("NUMEROS", "Action:Index " + ex.Message, Session["Nom_DomainUser"].ToString());
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
                var res = numero.OrdenFiltro(sortOrder, searchString);

                var pagedList1 = res.ToPagedList(pageIndex, pageSize);
                return View("index", pagedList1);
            }
            catch (Exception ex)
            {
                log.EscribaLog("NUMEROS", "Action:OrdenFiltro " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }


        // GET:  xy_subscriber/Create
        public ActionResult NuevoNumero()
        {
            try
            {
                if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
                {
                    return RedirectToAction("Login", "Acceso");
                }
                xy_numbers area = new xy_numbers();
                return View("NuevoNumero", area);

            }
            catch (Exception ex)
            {
                log.EscribaLog("NUMEROS", "Action:Create " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        public ActionResult NuevoNumero(xy_numbers nuevo)
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            try
            {
                numero.nuevaNumero(nuevo);
                return RedirectToAction("Index");
            }
            catch (Exception error)
            {
                ViewBag.Message = MensajesXynthesis.NoProcesa;
                log.EscribaLog("NUMEROS", "Action:Create " + error.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult EditarNumero(string id, int? page)
        {
            identificador = id;
            try
            {
                if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
                {
                    return RedirectToAction("Login", "Acceso");
                }


                int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
                int pageIndex = 1;
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

                var res = numero.editarNUmero(id, page);
                var pagedList = res.ToPagedList(pageIndex, pageSize);

                string SuscriptorxDefecto = pagedList[0].Nom_Subscriber.Trim();
                string ClientexDefecto = "";
                if (pagedList[0].nombreCliente == null)
                    ClientexDefecto = "";
                else
                    ClientexDefecto = pagedList[0].nombreCliente.Trim();

                var clien = numero.listaClientes();
                List<SelectListItem> itemClie = new List<SelectListItem>();
                foreach (Modelo.xy_cliente p in clien)
                {
                    itemClie.Add(new SelectListItem
                    {
                        Text = p.nombreCliente,
                        Selected = (p.nombreCliente == ClientexDefecto ? true : false)
                    });
                }

                List<SelectListItem> itemSuscriptores = new List<SelectListItem>();
                var suscriptores = usua.ObtenerListaUsuarios();
                itemSuscriptores = new List<SelectListItem>();
                foreach (Modelo.xy_subscriber p in suscriptores)
                {
                    if (p.Nom_Subscriber.ToUpper().Trim().Equals(SuscriptorxDefecto.ToUpper().Trim()))

                    itemSuscriptores.Add(new SelectListItem
                    {
                        Text = p.Nom_Subscriber,
                        Selected = true
                    });
                    else
                        itemSuscriptores.Add(new SelectListItem
                        {
                            Text = p.Nom_Subscriber,
                            Selected = false
                        });


                }
                ViewData["listaCliente"] = itemClie;
                ViewData["listaSuscriptores"] = itemSuscriptores;
                return View("EditarNumero", pagedList);

            }
            catch (Exception ex)
            {
                log.EscribaLog("NUMEROS", "Action:Edit " + ex.Message, Session["Nom_DomainUser"].ToString());
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarNumero(string id, string Tip_Extension, string DdlSuscriptor, string DdlCliente)
        {
            identificador = id;


            try
            {
                List<xy_cliente> lisClie = numero.buscarClientexNombre(DdlCliente);
                int branch = 0;
                if (Tip_Extension != null )
                {
                    if ( Tip_Extension.Equals("on"))
                    {
                        var res = numero.actualizaNUmero(id, 1, lisClie[0].Idcliente);
                        branch = -1;
                    }
                }
                else
                {
                    var res = numero.actualizaNUmero(id, 0, lisClie[0].Idcliente);
                    branch = 0;
                }


                List<xy_subscriber> lisSus = numero.buscarUsuarioxNom(DdlSuscriptor);
                List<xy_numbersxsubscriber> listanxs = numero.buscarNUmeroUsuarioxTel(id);
               


                Int32 resUpdate = numero.actualizarNumeroxUsuario(listanxs[0].Ide_Number,
                    listanxs[0].Fec_Date,
                    lisSus[0].Ide_Subscriber,
                    lisSus[0].Tip_Subscriber,
                    listanxs[0].Ide_ServerType,
                    branch,
                    -1,
                    -1,
                    listanxs[0].Ide_Number);

                if (resUpdate > 0)
                {
                    ViewBag.Message = MensajesXynthesis.Actualiza;
                    Session["mensale"] = MensajesXynthesis.Actualiza;
                    Session["codigo"] = "1";
                }
                else
                {
                    ViewBag.Message = MensajesXynthesis.NoProcesa;
                    Session["mensale"] = MensajesXynthesis.NoProcesa;
                    Session["codigo"] = "0";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                log.EscribaLog("NUMEROS", "Action:Edit_post " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Index");
            }
        }

        public IPagedList<xyp_SelNumbers_Result> cargaRegistro(string id, int? page)
        {


            int pageSize = cons.MaxRegGrilla == null ? 8 : Convert.ToInt32(cons.MaxRegGrilla);
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var res = numero.editarNUmero(id, page);
            var pagedList = res.ToPagedList(pageIndex, pageSize);


            SelectList itemClie = new SelectList(numero.listaClientes(), "Idcliente", "nombreCliente","");

            string SuscriptorxDefecto = pagedList[0].Nom_Subscriber.Trim();
            string ClientexDefecto = "";
            if (pagedList[0].nombreCliente == null)
                ClientexDefecto = "";
            else
                ClientexDefecto = pagedList[0].nombreCliente.Trim();

            List<SelectListItem> itemSuscriptores = new List<SelectListItem>();
            var suscriptores = usua.ObtenerListaUsuarios();  //from s in db.xy_subscriber select s;
            itemSuscriptores = new List<SelectListItem>();
            foreach (Modelo.xy_subscriber p in suscriptores)
            {
                itemSuscriptores.Add(new SelectListItem
                {
                    Text = p.Nom_Subscriber,
                    Selected = (p.Nom_Subscriber == SuscriptorxDefecto ? true : false)
                });
            }
            ViewData["listaCliente"] = itemClie;
            ViewData["listaSuscriptores"] = itemSuscriptores;
            ViewBag.mensaje = msg;

            return pagedList;
        }

        public ActionResult EliminarNumero(string id, int? page, string Nom_Country)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            try
            {
                if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
                {
                    return RedirectToAction("Login", "Acceso");
                }
                IPagedList<xyp_SelNumbers_Result> pagedList = cargaRegistro(id, page);
                return View("EliminarNumero", pagedList);
            }
            catch (Exception ex)
            {
                log.EscribaLog("NUMEROS", "Action:EliminarNumero " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Index");
            }

        }

        // POST:  xy_subscriber/Delete/5
        [HttpPost, ActionName("EliminarNumero")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            try
            {
                List<xy_numbersxsubscriber> res = numero.buscarNUmeroUsuarioxTel(id);
                //if (res.Count<=0)
                msg = numero.EliminarNumero(id);

                ViewBag.mensaje = msg;

                if (msg.codigo == 1)
                {
                    var res_ = numero.editarNUmero(id, 1);
                    res_ = new List<xyp_SelNumbers_Result>();
                    res_.Add(new xyp_SelNumbers_Result());
                    var pagedList = res_.ToPagedList(1, 1);
                    List<SelectListItem> itemPais = new List<SelectListItem>();
                    List<SelectListItem> itemDepto = new List<SelectListItem>();
                    List<SelectListItem> itemCiudad = new List<SelectListItem>();
                    List<SelectListItem> itemSuscriptores = new List<SelectListItem>();
                    List<xy_cliente> clie = new List<xy_cliente>();
                    SelectList itemClie = new SelectList( clie, "Idcliente", "nombreCliente", "");

                    ViewData["listaCliente"] = itemClie;
                    ViewData["listaPais"] = itemPais;
                    ViewData["listaDepto"] = itemDepto;
                    ViewData["listaCiudad"] = itemCiudad;
                    ViewData["listaSuscriptores"] = itemSuscriptores;

                    return View("EliminarNumero", pagedList);
                }
                else
                {
                    IPagedList<xyp_SelNumbers_Result> pagedList = cargaRegistro(id, 1);
                    return View("EliminarNumero", pagedList);
                }
                
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = msg;
                log.EscribaLog("NUMEROS", "Action:EliminarNumero " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Index");
            }
        }

        public ActionResult FillDepto(string nombrePais)
        {
            try
            {
                    var Depto = numero.obtenerDepto(nombrePais); 
                    int key = 0;
                    List<SelectListItem> itemDepto = new List<SelectListItem>();
                    foreach (Modelo.xyp_SelGeographyByCountry_Result p in Depto)
                    {
                        key++;
                        itemDepto.Add(new SelectListItem
                        {
                            Text = p.Nom_Province,
                            Value = key.ToString()
                        });
                    }
                    return Json(new SelectList(itemDepto, "Value", "Text"));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult FillCiudad(string nombrePais, string nombreDepto)
        {
            try
            {
                var Ciudades = numero.obtenerCiudad(nombrePais, nombreDepto); 

                List<SelectListItem> itemCiudad = new List<SelectListItem>();
                foreach (Modelo.xyp_SelGeographyByCity_Result p in Ciudades)
                {
                    itemCiudad.Add(new SelectListItem
                    {
                        Text = p.Nom_City
                    });
                }
                return Json(new SelectList(itemCiudad, "Value", "Text"));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult FillSuscriptor(string Suscriptor)
        {
            try
            {
                    var suscriptores =numero.obtenerlistaUsuario(); 

                    List<SelectListItem> itemSuscriptores = new List<SelectListItem>();
                    foreach (Modelo.xy_subscriber p in suscriptores)
                    {
                        itemSuscriptores.Add(new SelectListItem
                        {
                            Text = p.Nom_Subscriber,
                            Selected = (p.Nom_Subscriber == Suscriptor ? true : false)
                        });
                    }
                    return Json(new SelectList(itemSuscriptores, "Value", "Text"));

            }
            catch (Exception ex)
            {
                throw ex;
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
        public ActionResult EliminarRegistro(string id)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            try
            {
                List<xy_numbersxsubscriber> res = numero.buscarNUmeroUsuarioxTel(id);
                //if (res.Count<=0)
                msg = numero.EliminarNumero(id);

                ViewBag.mensaje = msg;

                if (msg.codigo == 1)
                {
                    Session["mensale"] = MensajesXynthesis.Elimina;
                    Session["codigo"] = "1";
                    //var res_ = numero.editarNUmero(id, 1);
                    //res_ = new List<xyp_SelNumbers_Result>();
                    //res_.Add(new xyp_SelNumbers_Result());
                    //var pagedList = res_.ToPagedList(1, 1);
                    //List<SelectListItem> itemPais = new List<SelectListItem>();
                    //List<SelectListItem> itemDepto = new List<SelectListItem>();
                    //List<SelectListItem> itemCiudad = new List<SelectListItem>();
                    //List<SelectListItem> itemSuscriptores = new List<SelectListItem>();
                    //List<xy_cliente> clie = new List<xy_cliente>();
                    //SelectList itemClie = new SelectList(clie, "Idcliente", "nombreCliente", "");

                    //ViewData["listaCliente"] = itemClie;
                    //ViewData["listaPais"] = itemPais;
                    //ViewData["listaDepto"] = itemDepto;
                    //ViewData["listaCiudad"] = itemCiudad;
                    //ViewData["listaSuscriptores"] = itemSuscriptores;


                }
                else
                {
                    //IPagedList<xyp_SelNumbers_Result> pagedList = cargaRegistro(id, 1);
                    Session["mensale"] = MensajesXynthesis.NoProcesa;
                    Session["codigo"] = "0";

                }
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                ViewBag.mensaje = msg;
                Session["mensale"] = MensajesXynthesis.ErrDesconocido;
                Session["codigo"] = "0";
                log.EscribaLog("NUMEROS", "Action:EliminarNumero " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Index");
            }

        }

    }
}