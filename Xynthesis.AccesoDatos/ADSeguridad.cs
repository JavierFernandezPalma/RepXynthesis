using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Modelo;

namespace Xynthesis.AccesoDatos
{
    public class ADSeguridad
    {
        xynthesisEntities xyt = new xynthesisEntities();

    public xy_subscriber ObtenerUsuario(string usuario)
    {
      return xyt.xy_subscriber.Where(P => P.Nom_DomainUser.ToUpper() == usuario.ToUpper()).FirstOrDefault();
    }

    }
}
