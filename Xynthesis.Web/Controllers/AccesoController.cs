using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Xynthesis.Modelo;
using Xynthesis.Utilidades;
using Xynthesis.Utilidades.Mensajes;
using System.Web.Helpers;
using System.Web.Configuration;
using Xynthesis.AccesoDatos;

namespace Xynthesis.Web.Controllers
{
    public class AccesoController : Controller
    {

        xynthesisEntities xyt = new xynthesisEntities();
        LogXynthesis lg = new LogXynthesis();
        CifradoClaves cc = new CifradoClaves();
        ADUsuarios usu = new ADUsuarios();
        Mensaje msg = new Mensaje();
        public string Usuari;

        public ActionResult Index()
        {
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(xy_subscriber log, string Nom_DomainUser)
        {
            var LoginDominio = usu.UsuarioDominio(Nom_DomainUser.Trim(), log.Str_Password.Trim());

            if (LoginDominio)
            {
                Session["LoginDominio"] = "ok";
                Session["Usuario"] = Nom_DomainUser;
                return RedirectToAction("Index", "Acceso");
            }

            try
            {
                if (ModelState.IsValid)
                {                   
                    
                        string key = ConfigurationManager.AppSettings["KeyCifradoXynthesis"];
                        string clave = cc.EncryptText(log.Str_Password, key);
                        Modelo.xy_subscriber res = usu.iniSesion(log.Nom_DomainUser, clave);
                        if (res != null)
                        {
                            Session["Nom_DomainUser"] = res.Nom_DomainUser;
                            Session["Nom_Subscriber"] = res.Nom_Subscriber;
                            Session["Ide_Subscriber"] = res.Ide_Subscriber;
                            Session["Id_Rol"] = res.Id_Rol;
                            Usuari = res.Nom_DomainUser;
                            return RedirectToAction("Index", "Acceso");
                        }
                        else
                        {
                            ViewBag.Incorrecto = MensajesXynthesis.NoSession;
                        }
                    }
                    
                
                xyt.Dispose();
                return View(log);

            }
            catch (Exception ex)
            {

                lg.EscribaLog("LOGIN", "Action:Login " + ex.Message, Nom_DomainUser);
                return RedirectToAction("Login", "Acceso");
            }
        }

        [AllowAnonymous]
        public ActionResult CerrarSesion()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Acceso");


        }

        [AllowAnonymous]
        public ActionResult CambioContraseña()
        {
            msg = new Mensaje();
            msg.codigo = -1;
            if (Session["Ide_Subscriber"] == null && Session["LoginDominio"] == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            int id = 0;
            if (Session["Ide_Subscriber"] == null)
            {

            }
            else
            {
                id = Convert.ToInt32(Session["Ide_Subscriber"].ToString());
            }
            try
            {
                xy_subscriber cambiarClave = xyt.xy_subscriber.Find(id);
                if (cambiarClave != null)
                {
                    string key = ConfigurationManager.AppSettings["KeyCifradoXynthesis"];
                    string clave = cc.DecryptText(cambiarClave.Str_Password, key);
                    cambiarClave.Str_Password = clave;
                    ViewBag.mensaje = msg;

                    Session["codigo"] = "1";
                    return View(cambiarClave);
                }
                else
                {
                    Session["codigo"] = "0";
                    Session["mensale"] = MensajesXynthesis.NoProcesa;
                    msg.mensaje = MensajesXynthesis.NoSession;
                    ViewBag.mensaje = msg;
                    return View(new xy_subscriber());
                }


            }
            catch (Exception ex)
            {
                Session["codigo"] = "0";
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                lg.EscribaLog("CAMBIAR CONTRASEÑA", "Action:CambioContraseña " + ex.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("Error", "Error");
            }
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult CambioContraseña(xy_subscriber mod_pass, string ClaveNueva)
        {
            mod_pass.Str_Password = ClaveNueva;
            msg = new Mensaje();
            msg.codigo = -1;
            CifradoClaves cc = new CifradoClaves();
            string key = ConfigurationManager.AppSettings["KeyCifradoXynthesis"];
            try
            {
                msg = usu.CambioContraseña(mod_pass);
                xy_subscriber cambiarClave = xyt.xy_subscriber.Find(mod_pass.Ide_Subscriber);

                string clave = cc.DecryptText(cambiarClave.Str_Password, key);
                cambiarClave.Str_Password = clave;

                ViewBag.mensaje = msg;
                Session["mensale"] = MensajesXynthesis.CambClave;
                Session["codigo"] = "1";
                //return View("CambioContraseña", cambiarClave);
                return RedirectToAction("CambioContraseña");
            }
            catch (Exception error)
            {
                Session["mensale"] = MensajesXynthesis.NoProcesa;
                Session["codigo"] = "0";
                ViewBag.mensaje = msg;
                lg.EscribaLog("ACCESO", "Action:Create " + error.Message, Session["Nom_DomainUser"].ToString());
                return RedirectToAction("CambioContraseña");

            }
        }

        public ActionResult ArchivoAyuda()
        {
            String rutaArchivoAyuda = ConfigurationManager.AppSettings["RutaArchivoAyuda"];
            try
            {
                //byte[] fileBytes = System.IO.File.ReadAllBytes(@"D:\Innovacion\Xynthesis desde 28-03-2018\Xynthesis.Web\ArchivoAyuda\ManualXynthesis.pdf");
                byte[] fileBytes = System.IO.File.ReadAllBytes(rutaArchivoAyuda);
                string fileName = "ArchivoAyuda.pdf";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult OlvidoClave(xy_subscriber cla)
        {
            return View();
        }

        [HttpPost]
        public ActionResult OlvidoClave(string Email)
        {
            if(Email.Trim() == "")
            {
                return RedirectToAction("OlvidoClave");
            }

            bool exists = (from nomb in xyt.xy_subscriber
                           where nomb.Email == Email.Trim()
                           select nomb).Any();

            if (exists == false)
            {
                ViewBag.ConfirmacionEnvio = MensajesXynthesis.CorreoInvalido;
                return View();
            }


            var id = (from sus in xyt.xy_subscriber
                      where sus.Email == Email.Trim()
                      select sus.Ide_Subscriber).FirstOrDefault();
            string key = ConfigurationManager.AppSettings["KeyCifradoXynthesis"];
            string clave = cc.EncryptText(Convert.ToString(id), key);            

            try
            {
                WebMail.SmtpServer = WebConfigurationManager.AppSettings["MailSender.Host"];
                WebMail.SmtpPort = Convert.ToInt32(WebConfigurationManager.AppSettings["MailSender.Port"]);
                WebMail.SmtpUseDefaultCredentials = true;
                WebMail.EnableSsl = true;
                WebMail.UserName = WebConfigurationManager.AppSettings["MailSender.corrreousuario"];
                WebMail.Password = WebConfigurationManager.AppSettings["MailSender.clave"];
                WebMail.From = WebConfigurationManager.AppSettings["MailSender.corrreousuario"];
                //string url = "http://localhost:63702/Acceso/CambioClave?mod=" + clave;
                string url = WebConfigurationManager.AppSettings["url"] + clave;
                //String mensaje = "Para cambiar su clave haga click aquí " + "<a href='http://localhost:63702/Acceso/CambioClave?user=' style='font-weight: bold'> RECUPERAR CONTRASEÑA</a>";
                String mensaje = "Para cambiar su clave haga click aquí " + "<a href="+ url + " style='font-weight: bold'> RECUPERAR CLAVE</a>";

                string asunto = "XYNTHESIS : RECUPERAR CLAVE";

                WebMail.Send(to: Email, subject: asunto, body: mensaje, isBodyHtml: true);
                ViewBag.ConfirmacionEnvio = MensajesXynthesis.ConfirmacionEnvio;
            }
            catch (Exception ex)
            {
                lg.EscribaLog("Cambio de clave","El error es : " + ex.ToString(), "");
                ViewBag.ConfirmacionEnvio = MensajesXynthesis.ConfirmacionEnvioError;

            }
            return View();
        }


        public ActionResult CambioClave()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CambioClave(string mod, string ClaveNueva)
        {
            try
            {
                string id = mod.Trim();
                string key = ConfigurationManager.AppSettings["KeyCifradoXynthesis"];
                string clave = cc.DecryptText(id, key);
                string clavNuev = cc.EncryptText(ClaveNueva, key);

                xyt.xyp_cambiarclave(Convert.ToInt32(clave), clavNuev);
                ViewBag.ConfirmacionCambio = MensajesXynthesis.ConfirmacionCambio;
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                ViewBag.ConfirmacionCambio = MensajesXynthesis.ConfirmacionCambioErrado;
                return RedirectToAction("CambioClave");
            }

            return View();
        }
    }
    
}