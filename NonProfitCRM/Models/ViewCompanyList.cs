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
    
    public partial class ViewCompanyList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IdentificationNumber { get; set; }
        public string Contact1Name { get; set; }
        public string Contact1Phone { get; set; }
        public string Contact1Email { get; set; }
        public string Contact2Name { get; set; }
        public string Contact2Phone { get; set; }
        public string Contact2Email { get; set; }
        public string Www { get; set; }
        public int CountryId { get; set; }
        public int RegionId { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int StatusId { get; set; }
        public bool Deleted { get; set; }
        public System.DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
    }
}