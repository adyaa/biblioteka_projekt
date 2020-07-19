using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace libraryapp.Models
{
    public class PurchaseTempMV
    {
        public int PurTemID { get; set; }
        //[Required(ErrorMessage = "Proszę wybrać książkę")]
        public int BookID { get; set; }
        //[Required(ErrorMessage = "Proszę wpisać ilość sztuk")]
        public int Qty { get; set; }
        //[Required(ErrorMessage = "Proszę wpisać kwotę zamówienia")]
        public double UnitPrice { get; set; }
    }
}