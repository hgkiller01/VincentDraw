using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DicentDraw.Models.ViewModels
{
    public class AddDessertViewModel : DessertViewModel
    {
        [DisplayName("數量")]
        [Range(0,99,ErrorMessage="{0}最大為{2}")]
        [Required(ErrorMessage="{0}為必填")]
        public int DessertAmount { get; set; }
        [DisplayName("送貨地址")]
        public string DeliveryAddress { get; set; }
        public string GiftID { get; set; }
        public int GiftPrice { get; set; }
        public int PieCount { get; set; }
        public int CakeCount { get; set; }
        public int CookieCount { get; set; }
        public string GiftName { get; set; }
    }
}