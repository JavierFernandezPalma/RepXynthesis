//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Xynthesis.Modelo
{
    using System;
    using System.Collections.Generic;
    
    public partial class xy_subscriberxcostcenter
    {
        public xy_subscriberxcostcenter()
        {
            this.xy_subscribersbudget = new HashSet<xy_subscribersbudget>();
        }
    
        public int Ide_CostCenter { get; set; }
        public long Ide_Subscriber { get; set; }
    
        public virtual xy_costcenters1 xy_costcenters { get; set; }
        public virtual xy_subscriber1 xy_subscriber { get; set; }
        public virtual ICollection<xy_subscribersbudget> xy_subscribersbudget { get; set; }
    }
}
