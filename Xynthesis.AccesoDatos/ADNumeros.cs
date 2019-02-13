using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Modelo;
using System.Data.Entity;
using Xynthesis.Utilidades;
using Xynthesis.Utilidades.Mensajes;

namespace Xynthesis.AccesoDatos
{
    public class ADNumeros
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();
        public List<xyp_SelNumbers_Result> ObtenerListaNumeros()
        {
            try
            {
                int totalRegis = (from x in xyt.xy_numbers select x).Count();
                return xyt.xyp_SelNumbers(1, totalRegis).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void nuevaNumero(xy_numbers nuevo)
        {
            try
            {
                xyt.xy_numbers.Add(nuevo);
                xyt.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<xyp_SelNumbers_Result> OrdenFiltro(string sortOrder, string searchString)
        {
            List<xyp_SelNumbers_Result> res;
            try
            {
                int totalRegis = (from x in xyt.xy_numbers select x).Count();
                if (!String.IsNullOrEmpty(searchString))
                {
                    res = xyt.xyp_SelNumbers(1, totalRegis).Where(s => s.Nom_Subscriber.ToUpper().Contains(searchString.ToUpper())
                                      || s.Ide_Number.Contains(searchString.ToUpper())
                                      || s.Nom_CostCenter.ToUpper().Contains(searchString.ToUpper())
                                      || s.Nom_City.ToUpper().Contains(searchString.ToUpper())
                                          ).ToList();
                }
                else
                {
                    res = xyt.xyp_SelNumbers(1, totalRegis).ToList();
                }
                if (sortOrder != null)
                {
                    if (sortOrder.Equals("name_desc"))
                        return res.OrderByDescending(s => s.Ide_Number).ToList();
                    else
                        return res.OrderBy(s => s.Ide_Number).ToList();
                }
                else
                    return res.OrderBy(s => s.Ide_Number).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<xyp_SelNumbers_Result> editarNUmero(string id, int? pagey)
        {
            int totalRegis = (from x in xyt.xy_numbers select x).Count();
            var res = xyt.xyp_SelNumbers(1, totalRegis).Where(s => s.Ide_Number.Replace("+", "").Replace(".", "").Replace("*", "") == id.ToString()).ToList();
            return res;
        }

        public List<xyp_SelCountry_Result> listaPais()
        {

            var Paises = xyt.xyp_SelCountry().ToList();
            return Paises;
        }

        public List<xy_cliente> listaClientes()
        {
            return xyt.xy_cliente.ToList(); 
        }

        public List<xyp_SelGeographyByCountry_Result> obtenerDepto(string PaisxDefecto)
        {
            return xyt.xyp_SelGeographyByCountry(PaisxDefecto).ToList();
        }

        public List<xyp_SelGeographyByCity_Result> obtenerCiudad(string PaisxDefecto, string DeptoxDefecto)
        {
            return xyt.xyp_SelGeographyByCity(PaisxDefecto, DeptoxDefecto).ToList();
        }


        public void guardarEdicion(string id, string Tip_Extension, string DdlSuscriptor, string DdlPais, string DdlDepto, string DdlCiudad, string DdlCliente)
        {
            try
            {

                int branch = 0;
                List<xy_cliente> listacli = xyt.xy_cliente.Where(s => s.nombreCliente.Equals(DdlCliente)).ToList();
                int cliente = Convert.ToInt32(listacli[0].Idcliente.ToString());
                if (Tip_Extension != null)
                {
                    var res = xyt.xyp_UpdNumber(id, 1, cliente);
                    branch = -1;
                }
                else
                {
                    var res = xyt.xyp_UpdNumber(id, 0, cliente);
                    branch = 0;
                }

                List<xy_subscriber> lisSus = xyt.xy_subscriber.Where(s => s.Nom_Subscriber.Equals(DdlSuscriptor)).ToList();
                List<xy_geography> lisCiu = xyt.xy_geography.Where(s => s.Nom_City.Equals(DdlCiudad)).ToList();
                List<xy_numbersxsubscriber> listanxs = xyt.xy_numbersxsubscriber.Where(s => s.Ide_Number.Replace("+", "").Replace(".", "").Equals(id)).ToList();

                listanxs[0].Ide_Geography = lisCiu[0].Ide_Geography;
                listanxs[0].Ide_Branch = branch;
                string nroOriginal = listanxs[0].Ide_Number;
                string suscriber = lisSus[0].Ide_Subscriber.ToString();
                string suscriberold = lisSus[0].Ide_Subscriber.ToString();


                Int32 resUpdate = xyt.xyp_UpdNumberBySubscriber(nroOriginal,
                    listanxs[0].Fec_Date,
                    lisSus[0].Ide_Subscriber,
                    lisSus[0].Tip_Subscriber,
                    listanxs[0].Ide_ServerType,
                    branch,
                    listanxs[0].Ide_CostCenter,
                    listanxs[0].Ide_Geography,
                    nroOriginal);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public  List<xy_numbersxsubscriber> buscarNumeroxUsuarioId(string id)
        {
            return xyt.xy_numbersxsubscriber.Where(s => s.Ide_Number.Replace("+", "").Replace(".", "").Replace("*", "").Equals(id)).ToList();
        }

        public xy_numbers buscarNumeroxId(string id)
        {
            try
            {

                return xyt.xy_numbers.Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<xyp_SelNumbers_Result> buscarSelNumeroxId(string id)
        {
            try
            {
                int totalRegis = (from x in xyt.xy_numbers select x).Count();
                return xyt.xyp_SelNumbers(1, totalRegis).Where(s => s.Ide_Number.Replace("+", "").Replace(".", "").Replace("*", "") == id.ToString()).ToList(); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<xy_subscriber> obtenerlistaUsuario()
        {
            try
            {

                return (from s in xyt.xy_subscriber select s).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<xy_subscriber> buscarUsuarioxNom(string nombreSus)
        {
            try
            {

                return xyt.xy_subscriber.Where(s => s.Nom_Subscriber.Equals(nombreSus)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<xy_geography> buscarCiudadxNom(string nombreCiu)
        {
            try
            {

                return xyt.xy_geography.Where(s => s.Nom_City.Equals(nombreCiu)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<xy_cliente> buscarClientexNombre(string nomCliente)
        {
            try
            {

                return xyt.xy_cliente.Where(s => s.nombreCliente.Equals(nomCliente)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<xy_numbersxsubscriber> buscarNUmeroUsuarioxTel(string numero)
        {
            try
            {

                return xyt.xy_numbersxsubscriber.Where(s => s.Ide_Number.Replace("+", "").Replace(".", "").Replace("*", "").Equals(numero)).ToList(); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public xy_subscriber buscarUsuarioxId(long? id)
        {
            try
            {

                return xyt.xy_subscriber.Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Xynthesis.Utilidades.Mensaje EliminarNumero(string id)
        {
            msg = new Mensaje();
            try
            {
                if ((from s in xyt.xy_numbersxsubscriber where s.Ide_Number == id select s).Count() <= 0)
                {
                    int resdel = xyt.xyp_DelNumber(id);
                    msg.codigo = 1;
                    msg.mensaje = MensajesXynthesis.Elimina;
                }
                else
                {
                    msg.codigo = 0;
                    msg.mensaje = MensajesXynthesis.NoProcesa;
                }
                return msg;
            }
            catch (Exception ex)
            {
                msg.codigo = 0;
                msg.mensaje = MensajesXynthesis.ErrDesconocido;
                log.EscribaLog("NUMEROS", "Action:EliminarNumero " + ex.Message, "");
                return msg;
            }
        }


        public int actualizaNUmero(string id, Int32 tipo, int DdlCliente)
        {
            try
            {
                xyt.xyp_UpdNumber(id, tipo, DdlCliente);
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int actualizarNumeroxUsuario(string nroOriginal,
                       DateTime Fec_Date,
                       long Ide_Subscriber,
                       string Tip_Subscriber,
                       Int32 Ide_ServerType,
                       Int32 branch,
                       Int32 Ide_CostCenter,
                       Int32 Ide_Geography,
                       string nroOriginal_)
        {
            try
            {
                Int32 resUpdate = xyt.xyp_UpdNumberBySubscriber(nroOriginal,
                         Fec_Date,
                        Ide_Subscriber,
                        Tip_Subscriber,
                        Ide_ServerType,
                         branch,
                        Ide_CostCenter,
                        Ide_Geography,
                        nroOriginal_);
                return resUpdate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Xynthesis.Utilidades.Mensaje eliminarNumero(string id)
        {
            int resdel = 0;
            msg = new Mensaje();
            try
            {
                if ((from s in xyt.xy_numbersxsubscriber where s.Ide_Number == id select s).Count() <= 0)
                {
                    resdel = xyt.xyp_DelNumber(id);
                    msg.codigo = 1;
                    msg.mensaje = MensajesXynthesis.Elimina;
                }
                else
                {
                    msg.codigo = 0;
                    msg.mensaje = MensajesXynthesis.NoProcesa;
                }
                return msg;
            }
            catch (Exception ex)
            {
                msg.codigo = 0;
                msg.mensaje = MensajesXynthesis.ErrDesconocido;
                log.EscribaLog("NUMEROS", "Action:eliminarNumero " + ex.Message, "");
                return msg;
            }
        }



    }
}
