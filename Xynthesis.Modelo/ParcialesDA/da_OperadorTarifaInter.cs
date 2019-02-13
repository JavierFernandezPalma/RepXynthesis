using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Xynthesis.Utilidades.Mensajes;

namespace Xynthesis.Modelo
{
    [MetadataType(typeof(da_OperadorTarifaInter))]
    public partial class xy_operador_tarifa_inter
    {
    }
    class da_OperadorTarifaInter
    {
        [Required(ErrorMessageResourceName = "Requerido", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        [Display(Name = "Operador")]
        public Int32 IdOperador { get; set; }

        [Required(ErrorMessageResourceName = "Requerido", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        [Display(Name = "País")]
        public Int32 IdPais { get; set; }

        [Required(ErrorMessageResourceName = "Requerido", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        [Display(Name = "Valor Costo(Sin Iva)")]
        [RegularExpression(@"^[0-9]([\.\,][0-9]{1,2})?$", ErrorMessageResourceName = "numeroInv", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        public Decimal VlrInternaMinsinIva { get; set; }

        [Required(ErrorMessageResourceName = "Requerido", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^[0-9]([\.\,][0-9]{1,2})?$", ErrorMessageResourceName = "numeroInv", ErrorMessageResourceType = typeof(MensajesXynthesis))]
        public Decimal VlrInternaMinconIva { get; set; }
    }
}
