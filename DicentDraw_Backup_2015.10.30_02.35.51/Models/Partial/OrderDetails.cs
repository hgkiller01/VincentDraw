using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DicentDraw.Models
{
    [MetadataType(typeof(OrderDetailsMD))]
    public partial class OrderDetails
    {
        public class OrderDetailsMD
        {
            public string DetailID { get; set; }
            [DisplayName("點心數量")]
            public int DessertAmount { get; set; }
            [DisplayName("")]
            public string OrderID { get; set; }
            public string GiftID { get; set; }
            public string DessertID { get; set; }
        }
    }
}