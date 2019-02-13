using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xynthesis.Utilidades.Mensajes;

namespace Xynthesis.Modelo
{
    [MetadataType(typeof(da_Tarifas))]
    public partial class xy_rates
    {

    }
    class da_Tarifas
    {
        [Required(ErrorMessageResourceName = "Requerido", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        [MaxLength(100, ErrorMessageResourceName = "Maxlon", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        public string Des_Rate { get; set; }

        [Required(ErrorMessageResourceName = "Requerido", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        public int Num_Length { get; set; }

        [Range(0, 9999, ErrorMessage = "Valor entre 0 y 9999")]
        [Required(ErrorMessage = "(*) Por favor, el campo o dato es requerido..")]
        [MaxLength(10, ErrorMessageResourceName = "Maxlon", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        public string Num_Prefix { get; set; }


    }
}
