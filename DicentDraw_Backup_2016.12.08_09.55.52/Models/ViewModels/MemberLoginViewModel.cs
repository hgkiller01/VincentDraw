using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DicentDraw.Models.ViewModels
{
    public class MemberLoginViewModel
    {
        [StringLength(12, ErrorMessage = "{0}必需為{1}到{2}個字", MinimumLength = 5)]
        [Required(ErrorMessage = "{0}為必填")]
        [DisplayName("帳號")]
        public string Account { get; set; }
        [Required(ErrorMessage = "{0}為必填")]
        [StringLength(20, ErrorMessage = "{0}不可以超過{0}個字")]
        [DisplayName("密碼")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }
        [DisplayName("確認密碼")]
        [Compare("PassWord")]
        [Required(ErrorMessage = "{0}為必填")]
        [DataType(DataType.Password)]
        public string PassWord2 { get; set; }
    }
}