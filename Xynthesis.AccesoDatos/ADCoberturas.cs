using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Modelo;

namespace Xynthesis.AccesoDatos
{
    public class ADCoberturas
    {
        xynthesisEntities xyt = new xynthesisEntities();

        public List<xyp_SelCoverages_Result> OrdenFiltro(string sortOrder, string searchString)
        {
            List<xyp_SelCoverages_Result> res;
            try
            {
                int totalRegis = (from x in xyt.xy_subscriber select x).Count();
                if (!String.IsNullOrEmpty(searchString))
                {
                    res = (from s in xyt.xyp_SelCoverages() where s.Nom_Coverage.ToUpper().Contains(searchString.ToUpper()) select s).ToList();

                }
                else
                {
                    res = (from s in xyt.xyp_SelCoverages() select s).ToList();
                }
                if (sortOrder != null)
                {
                    if (sortOrder.Equals("name_desc"))
                        return res.OrderByDescending(s => s.Nom_Coverage).ToList();
                    else
                        return res.OrderBy(s => s.Nom_Coverage).ToList();
                }
                else
                    return res.OrderBy(s => s.Nom_Coverage).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<xyp_SelCoverages_Result> listacoberturas()
        {
            try
            {
                List<xyp_SelCoverages_Result> lista = new List<xyp_SelCoverages_Result>();
                lista = xyt.xyp_SelCoverages().ToList();
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void NuevaCobertura(xy_coverage nuevo)
        {
            try
            {
                xyt.xy_coverage.Add(nuevo);
                xyt.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EditarCobertura(int id, string nombre, string mov, string nacio, string inter)
        {
            try
            {
                var cobertura = new xy_coverage { Ide_Coverage = id };

                xyt.xy_coverage.Attach(cobertura);
                cobertura.Nom_Coverage = nombre;

                if (mov == "on")
                {
                    cobertura.Movil = true;
                }
                else
                {
                    cobertura.Movil = false;
                }

                if (nacio == "on")
                {
                    cobertura.Nacional = true;
                }
                else
                {
                    cobertura.Nacional = false;
                }

                if (inter == "on")
                {
                    cobertura.Internacional = true;
                }
                else
                {
                    cobertura.Internacional = false;
                }


                xyt.Configuration.ValidateOnSaveEnabled = true;
                xyt.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EliminarCobertura(int id)
        {
            try
            {
                var cobertura = new xy_coverage { Ide_Coverage = id };
                xyt.xy_coverage.Attach(cobertura);
                xyt.xy_coverage.Remove(cobertura);
                xyt.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
