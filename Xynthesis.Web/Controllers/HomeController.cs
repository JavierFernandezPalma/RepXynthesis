using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xynthesis.Modelo;
using Xynthesis.AccesoDatos;

namespace Xynthesis.Web.Controllers
{
    public class HomeController : Controller
    {
        xynthesisEntities context = new xynthesisEntities();
        public ActionResult Index()
        {
            ADSeguridad contexto = new ADSeguridad();
            var query = contexto.ObtenerUsuario("andresv");
            return View(query);
        }

        
    }
}