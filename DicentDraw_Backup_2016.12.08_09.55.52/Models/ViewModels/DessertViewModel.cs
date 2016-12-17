using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DicentDraw.Models.ViewModels
{
    public class DessertViewModel
    {
        public string DessertID { get; set; }
        public string DessertName { get; set; }
        public int DessertPrice { get; set; }
        public string DessertKind { get; set; }
        public string DessertIntroduction { get; set; }
        public string DessertImage { get; set; }
        public bool IsOnSale { get; set; }
    }
}