using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Utilidades.Mensajes;


namespace Xynthesis.Modelo
{
    [MetadataType(typeof(da_Usuario))]
    public partial class xy_subscriber
    {
    }
    class da_Usuario
    {

        [MaxLength(20, ErrorMessageResourceName = "Maxlon", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        public string Nom_DomainUser { get; set; }

        [Required(ErrorMessageResourceName = "Requerido", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        [MaxLength(100, ErrorMessage = "Se excedió el máximo de longitud")]
        public string Str_Password { get; set; }


        [MaxLength(80, ErrorMessageResourceName = "Maxlon", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        [Display(Name = "Nombre del Cliente")]
        public string Nom_Subscriber { get; set; }


    }
}
