using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Utilidades.Mensajes;

namespace Xynthesis.Modelo
{
    [MetadataType(typeof(da_Coberturas))]
    public partial class xy_coverage
    {
    }
    class da_Coberturas
    {
        [Required(ErrorMessageResourceName = "Requerido", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        [MaxLength(20, ErrorMessageResourceName = "Maxlon", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        [Display(Name = "Cobertura")]
        public string Nom_Coverage { get; set; }
    }
}
