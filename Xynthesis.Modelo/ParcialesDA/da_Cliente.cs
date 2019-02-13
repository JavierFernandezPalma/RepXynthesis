using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Xynthesis.Modelo;
using Xynthesis.Utilidades.Mensajes;

namespace Xynthesis.Modelo
{
    [MetadataType(typeof(da_Cliente))]
    public partial class xy_cliente
    {

    }
    public class da_Cliente
    {

        [Required(ErrorMessageResourceName = "Requerido", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        [MaxLength(120, ErrorMessageResourceName = "Maxlon", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        [Display(Name = "Nombre del Cliente")]
        public string nombreCliente { get; set; }
    }
}
