//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Seoul_Stay.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ItemPrice
    {
        public long ID { get; set; }
        public System.Guid GUID { get; set; }
        public long ItemID { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Price { get; set; }
        public long CancellationPolicyID { get; set; }
    
        public virtual CancellationPolicy CancellationPolicy { get; set; }
        public virtual Item Item { get; set; }
    }
}