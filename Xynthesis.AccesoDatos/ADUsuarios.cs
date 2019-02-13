using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Modelo;
using System.Data.Entity;
using Xynthesis.Utilidades;
using System.Configuration;
using Xynthesis.Utilidades.Mensajes;
using System.DirectoryServices;

namespace Xynthesis.AccesoDatos
{

    public class ADUsuarios
    {
        xynthesisEntities xyt = new xynthesisEntities();
        Xynthesis.Utilidades.Mensaje msg = new Mensaje();
        Utilidades.LogXynthesis log = new LogXynthesis();
        //Metodo 
        public IQueryable<xy_subscriber> ObtenerListaUsuarios()
        {
            try
            {
                return from s in xyt.xy_subscriber select s;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public xy_subscriber iniSesion(string Nom_DomainUser, string Clave)
        {
            try
            {
                return xyt.xy_subscriber.Where(a => a.Nom_DomainUser.Equals(Nom_DomainUser) && a.Str_Password.Equals(Clave)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void NuevaUsuario(xy_subscriber nuevo)
        {
            
            try
            {
                xyt.xy_subscriber.Add(nuevo);
                xyt.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EditarUsuario(string id, string nombre, int IdSucursal, int Ide_CostCenter, string user, string tipo, int Id_Rol, string email)
        {
            try
            {
                var usuario = new xy_subscriber { Ide_Subscriber = Convert.ToInt32(id) };

                xyt.xy_subscriber.Attach(usuario);
                usuario.Nom_Subscriber = nombre;
                usuario.Nom_DomainUser = user;
                usuario.Tip_Subscriber = tipo;
                usuario.IdSucursal = IdSucursal;
                usuario.Ide_CostCenter = Ide_CostCenter;
                usuario.Id_Rol = Id_Rol;
                usuario.Email = email;

                xyt.Configuration.ValidateOnSaveEnabled = false;

                xyt.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void EliminarUsuario(long id)
        {
            try
            {
                var user = new xy_subscriber { Ide_Subscriber = id };
                xyt.xy_subscriber.Attach(user);
                xyt.xy_subscriber.Remove(user);
                xyt.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public xy_subscriber buscarAreaxId(long? id)
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

        public List<xy_subscriber> OrdenFiltro(string sortOrder, string searchString)
        {
            List<xy_subscriber> res;
            try
            {
                int totalRegis = (from x in xyt.xy_subscriber select x).Count();
                if (!String.IsNullOrEmpty(searchString))
                {
                    res = (from s in xyt.xy_subscriber where s.Nom_Subscriber.ToUpper().Contains(searchString.ToUpper()) select s).ToList();

                }
                else
                {
                    res = (from s in xyt.xy_subscriber select s).ToList();
                }
                if (sortOrder != null)
                {
                    if (sortOrder.Equals("name_desc"))
                        return res.OrderByDescending(s => s.Nom_Subscriber).ToList();
                    else
                        return res.OrderBy(s => s.Nom_Subscriber).ToList();
                }
                else
                    return res.OrderBy(s => s.Nom_Subscriber).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Xynthesis.Utilidades.Mensaje CambioContraseña(xy_subscriber cambio)
        {
            msg = new Mensaje();
            msg.codigo = -1;
            try
            {
                CifradoClaves cc = new CifradoClaves();
                string key = ConfigurationManager.AppSettings["KeyCifradoXynthesis"];
                xy_subscriber cambiaclave = xyt.xy_subscriber.Find(cambio.Ide_Subscriber);
                cambiaclave.Str_Password = cc.EncryptText(cambio.Str_Password, key);
                cambiaclave.Cod_Subscriber = cambio.Cod_Subscriber;
                xyt.SaveChanges();
                msg.codigo = 1;
                msg.mensaje = MensajesXynthesis.Actualiza;
                return msg;
            }
            catch (Exception ex)
            {
                msg.codigo = 0;
                msg.mensaje = MensajesXynthesis.ErrDesconocido;
                log.EscribaLog("USUARIOS", "Action:CambioContraseña " + ex.Message, "");
                return msg;
            }
        }


        public bool UsuarioDominio(string usuario, string clave)
        {
            string path = ConfigurationManager.AppSettings["path"];
            string dominio = ConfigurationManager.AppSettings["dominio"];
            string usu = usuario.Trim();
            string pass = clave.Trim();
            string domUsu = dominio + @"\" + usu;

            bool permiso = ValidarUsuarioDominio(path, domUsu, pass);

            if (permiso)
            {
                return true;
            }else
            {
                return false;
            }
        }

        public bool ValidarUsuarioDominio(string path, string usuario, string clave)
        {
            DirectoryEntry de = new DirectoryEntry(path, usuario, clave, AuthenticationTypes.Secure);

            try
            {
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.FindOne();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }




    }

}
