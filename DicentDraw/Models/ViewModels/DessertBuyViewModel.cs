using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DicentDraw.Models.ViewModels
{
    public class DessertBuyViewModel :DessertViewModel
    {
        public int DessertAmount { get; set; }
        public string DeliveryAddress { get; set; }
        public int Orderstat { get; set; }
        public string GiftID { get; set; }
    }
}