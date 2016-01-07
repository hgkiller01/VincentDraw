using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DicentDraw.Models
{
    [MetadataType(typeof(OrdersMD))]
    public partial class Orders
    {
        public class OrdersMD
        {
            [Required]
            [DisplayName("訂單編號")]
            public string OrderID { get; set; }
            [Required]
            [DisplayName("訂單日期")]
            public System.DateTime OrderDate { get; set; }
            [Required]
            [DisplayName("處理狀態")]
            public int Orderstat { get; set; }
            [DisplayName("送貨地址")]
            [Required(ErrorMessage="{0}為必填")]
            public string DeliveryAddress { get; set; }
            public string Account { get; set; }
        }
    }
}