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
    
    public partial class xy_budgetbycommunication
    {
        public int Ide_Budget { get; set; }
        public int Ide_ServerType { get; set; }
        public long Vlr_BudgetValue { get; set; }
        public int Num_BudgetPercent { get; set; }
    
        public virtual xy_communicationserver xy_communicationserver { get; set; }
        public virtual xy_companybudget xy_companybudget { get; set; }
    }
}
