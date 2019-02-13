using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Modelo;
using Xynthesis.Utilidades.Mensajes;
namespace Xynthesis.AccesoDatos
{
  
    public class ADTarifa
    {
        xynthesisEntities xyt = new xynthesisEntities();

        public IQueryable<xy_rates> ObtenerListaTarifa()
        {
            return from s in xyt.xy_rates select s;
        }

        public List<xy_rates> OrdenFiltro(string sortOrder, string searchString)
        {
            List<xy_rates> res;
            try
            {
                int totalRegis = (from x in xyt.xy_rates select x).Count();
                if (!String.IsNullOrEmpty(searchString))
                {
                    res = (from s in xyt.xy_rates where s.Des_Rate.ToUpper().Contains(searchString.ToUpper()) select s).ToList();

                }
                else
                {
                    res = (from s in xyt.xy_rates select s).ToList();
                }
                if (sortOrder != null)
                {
                    if (sortOrder.Equals("name_desc"))
                        return res.OrderByDescending(s => s.Des_Rate).ToList();
                    else
                        return res.OrderBy(s => s.Des_Rate).ToList();
                }
                else
                    return res.OrderBy(s => s.Des_Rate).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void NuevaTar(xy_rates nuevo)
        {
            try
            {
                xyt.xy_rates.Add(nuevo);
                xyt.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EditarTarifa(xy_rates update)
        {
            try
            {
                xy_rates updateRates = xyt.xy_rates.Find(update.Ide_Rate);
                updateRates.Des_Rate = update.Des_Rate;
                updateRates.Ide_Coverage = update.Ide_Coverage;
                updateRates.Ide_Operator = update.Ide_Operator;
                updateRates.Num_Length = update.Num_Length;
                updateRates.Num_Prefix = update.Num_Prefix;
                xyt.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int EliminarTarifa(int id)
        {
            try
            {
                var eli = xyt.xy_rates.Find(id);
                xyt.xy_rates.Remove(eli);
                xyt.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
               
            }
        }



    }
}
