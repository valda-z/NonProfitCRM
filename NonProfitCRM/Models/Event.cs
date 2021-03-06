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
    
    public partial class Event
    {
        public Event()
        {
            this.Event2CompanySub = new HashSet<Event2CompanySub>();
            this.Tag2Event = new HashSet<Tag2Event>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime DateOfEvent { get; set; }
        public int CompanyId { get; set; }
        public int NonProfitOrgId { get; set; }
        public int Capacity { get; set; }
        public string ContactCompanyName { get; set; }
        public string ContactCompanyPhone { get; set; }
        public string ContactCompanyEmail { get; set; }
        public string ContactCompanyNote { get; set; }
        public string ContactNonProfitOrgName { get; set; }
        public string ContactNonProfitOrgPhone { get; set; }
        public string ContactNonProfitOrgEmail { get; set; }
        public string ContactNonProfitOrgNote { get; set; }
        public string Note { get; set; }
        public bool Deleted { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime Updated { get; set; }
        public Nullable<System.DateTime> Closed { get; set; }
        public bool Insurance { get; set; }
        public int TypeId { get; set; }
        public bool SelfOrganised { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual NonProfitOrg NonProfitOrg { get; set; }
        public virtual ICollection<Event2CompanySub> Event2CompanySub { get; set; }
        public virtual ICollection<Tag2Event> Tag2Event { get; set; }
        public virtual EventType EventType { get; set; }
    }
}
