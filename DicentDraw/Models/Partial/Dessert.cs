using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DicentDraw.Models
{
    [MetadataType(typeof(DessertMD))]
    public partial class Dessert
    {
        public class DessertMD
        {
            public string DessertID { get; set; }
            [DisplayName("點心名稱")]
            [Required(ErrorMessage="{0}必填")]
            [StringLength(50,ErrorMessage="{0}最多為{1}個字")]
            public string DessertName { get; set; }
            [DisplayName("單價")]
            [Required(ErrorMessage="{0}必填")]
            public int DessertPrice { get; set; }
            [DisplayName("類別")]
            [Required(ErrorMessage = "{0}必填")]
            public string DessertKind { get; set; }
            [DisplayName("介紹")]
            [Required(ErrorMessage = "{0}必填")]
            [DataType(DataType.MultilineText)]
            public string DessertIntroduction { get; set; }
            [DisplayName("點心圖片")]
            public string DessertImage { get; set; }
            [DisplayName("上架與否")]
            public bool IsOnSale { get; set; }
        }
    }
}