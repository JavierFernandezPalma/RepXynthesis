using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Xynthesis.Utilidades.Mensajes;

namespace Xynthesis.Modelo
{

    [MetadataType(typeof(da_Areas))]
    public partial class xy_costcenters 
    {

    }
    class da_Areas
    {

        [Required(ErrorMessageResourceName ="Requerido", ErrorMessageResourceType =typeof(MensajesXynthesis))]
        [MaxLength(100, ErrorMessageResourceName = "Maxlon", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        [Display(Name = "Nombre del Área")]
        public string Nom_CostCenter { get; set;}

    }
}
