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
    [MetadataType(typeof(da_Operador))]
    public partial class xy_operators
    {

    }
    public class da_Operador
    {

        [MaxLength(5, ErrorMessageResourceName = "Maxlon", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        [Display(Name = "Código del Operador")]
        [RegularExpression(@"[0-9]{1,5}", ErrorMessage = "El campo tiene que ser numérico")]
        public string Cod_Operator { get; set; }



        [RegularExpression(@"^[0-9]([\.\,][0-9]{1,2})?$", ErrorMessageResourceName = "numeroInv", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        public decimal vlr_Cost { get; set; }

        [Required(ErrorMessageResourceName = "Requerido", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        [MaxLength(40, ErrorMessageResourceName = "Maxlon", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        [Display(Name = "Nombre del Operador")]
        public string Nom_Operator { get; set; }


    }

}
