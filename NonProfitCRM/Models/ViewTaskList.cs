//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NonProfitCRM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ViewTaskList
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public int EntityId { get; set; }
        public string Entity { get; set; }
        public System.DateTime DueDate { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string AssignedTo { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime Updated { get; set; }
        public string StatusName { get; set; }
        public string StatusColor { get; set; }
        public string EntityName { get; set; }
    }
}
