using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DicentDraw.Models.ViewModels
{
    public class MemberDataViewModel : MemberViewModel
    {
        [DisplayName("新密碼")]
        [Required(ErrorMessage = "{0}為必填")]
        [Compare("PassWord2")]
        [StringLength(20, ErrorMessage = "{0}不可以超過{1}個字")]
        [DataType(DataType.Password)]
        public new string PassWord2 { get; set; }
    }
}