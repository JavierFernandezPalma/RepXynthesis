using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xynthesis.Modelo;
using Xynthesis.Utilidades;
using System.Web.Security;
using Xynthesis.AccesoDatos;
using System.Data.Entity;
using Xynthesis.Utilidades.Mensajes;

namespace Xynthesis.Web.Controllers
{
    public class RetarificacionController : Controller
    {
        xynthesisEntities xyt = new xynthesisEntities();
        LogXynthesis lg = new LogXynthesis();
        ADRetarificacion ret = new ADRetarificacion();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();

        // GET: Retarificacion
        public ActionResult Retarificar()
        {
            msg = new Mensaje();
            msg.codigo = -1;
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            ViewBag.mensaje = msg;
            return View();
        }

        [HttpPost]
        public ActionResult Retarificar(string FechaInicial, string FechaFinal)
        {

            msg = new Mensaje();
            msg.codigo = -1;
            Boolean error = false;
            try
            {
                try
                {
                    ViewBag.fechaini = Convert.ToDateTime(FechaInicial).ToString("dd-MM-yyyy") + " A";
                    ViewBag.fechafin = Convert.ToDateTime(FechaFinal).ToString("dd-MM-yyyy");


                }
                catch (Exception ex)
                {
                    error = true;
                    //msg.mensaje = MensajesXynthesis.errFechas;
                    Session["mensale"] = MensajesXynthesis.errFechas;
                    Session["codigo"] = "0";
                    ViewBag.mensaje = msg;
                    ViewBag.mensje = "";
                }
                if (!error)
                {
                    msg = ret.retarificar(FechaInicial, FechaFinal);
                    ViewBag.mensaje = msg;
                    ViewBag.mensje = "";
                    Session["mensale"] = MensajesXynthesis.proRetar;
                    Session["codigo"] = "1";
                }
                return View("Retarificar");
            }
            catch (Exception ex)
            {
                Session["codigo"] = "0";
                Session["mensale"] = MensajesXynthesis.falloRetari;
                //msg.mensaje = MensajesXynthesis.falloRetari;
                //ViewBag.mensaje = msg;
                //ViewBag.mensje = "";

                lg.EscribaLog("RETARIFICAR", "Action:Retarificar " + ex.Message, Session["Nom_DomainUser"].ToString());
                return View("Retarificar");
                //return RedirectToAction("Error", "Error");
            }

        }
    }
}