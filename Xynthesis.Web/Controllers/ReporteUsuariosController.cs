using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xynthesis.Modelo;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using PagedList.Mvc;
using PagedList;
using System.IO;

namespace Xynthesis.Web.Controllers
{
    public class ReporteUsuariosController : Controller
    {
        xynthesisEntities xyt = new xynthesisEntities();
        public ActionResult ListaUsuarios(string paraPaginacion, string filtro, int? page)
        {
            try
            {
                var lista = xyt.xyp_SelUsuarios().ToList();
                if (!String.IsNullOrEmpty(filtro))
                {
                    var res1 = lista.Where(s => s.Nom_Subscriber.Contains(filtro));
                }

                int pageSize = 10;
                int pageNumber = (page ?? 1);

                return View(lista.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public ActionResult Reportes(string opcion)
        {
            switch (opcion)
            {
                case "pdf":

                    return (ReportePDF());

                case "excel":

                    return (ReporteEXCEL());

                case "word":

                    return (ReporteWORD());

                default:
                    return View();

            }
        }

        public ActionResult ReportePDF()
        {
            List<xyp_SelUsuarios_Result> Usuarios = new List<xyp_SelUsuarios_Result>();

            Usuarios = xyt.xyp_SelUsuarios().ToList();

            ReportDocument rd = new ReportDocument();

            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "xyp_SelUsuarios.rpt"));
            rd.SetDataSource(Usuarios);


            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                //=====================Inicio para Exportar a PDF============================================
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "ReporteUsuarios.pdf");
                //=====================Fin para Exportar a PDF===============================================

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult ReporteEXCEL()
        {
            List<xyp_SelUsuarios_Result> Usuarios = new List<xyp_SelUsuarios_Result>();

            Usuarios = xyt.xyp_SelUsuarios().ToList();

            ReportDocument rd = new ReportDocument();

            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "xyp_SelUsuarios.rpt"));
            rd.SetDataSource(Usuarios);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {

                //=====================Inicio para Exportar a Excel==========================================
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/xls", "ReporteUsuarios.xls");
                //=====================Fin para Exportar a Excel=============================================

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult ReporteWORD()
        {
            List<xyp_SelUsuarios_Result> Usuarios = new List<xyp_SelUsuarios_Result>();

            Usuarios = xyt.xyp_SelUsuarios().ToList();

            ReportDocument rd = new ReportDocument();

            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "xyp_SelUsuarios.rpt"));
            rd.SetDataSource(Usuarios);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {

                //=====================Inicio para Exportar a Word===========================================
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.WordForWindows);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/doc", "ReporteUsuarios.doc");
                //=====================Fin para Exportar a Word==============================================
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}