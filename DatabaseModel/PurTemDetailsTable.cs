//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class PurTemDetailsTable
    {
        public int PurTemID { get; set; }
        [Required(ErrorMessage = "Prosz� wybra� ksi��k�")]
        public int BookID { get; set; }
        [Required(ErrorMessage = "Prosz� wpisa� ilo�� sztuk")]
        public int Qty { get; set; }
        [Required(ErrorMessage = "Prosz� wpisa� kwot� zam�wienia")]
        public double UnitPrice { get; set; }

        public virtual BookTable BookTable { get; set; }
    }
}