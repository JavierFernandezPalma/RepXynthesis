using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Utilidades.Mensajes;

namespace Xynthesis.Modelo
{
    [MetadataType(typeof(da_sucursal))]
    public partial class xy_sucursal
    {
    }
    class da_sucursal
    {
        [Required(ErrorMessageResourceName = "Requerido", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        [MaxLength(100, ErrorMessageResourceName = "Maxlon", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        public string NombreSucursal { get; set; }
        public int Ide_Geography { get; set; }
    }
}
