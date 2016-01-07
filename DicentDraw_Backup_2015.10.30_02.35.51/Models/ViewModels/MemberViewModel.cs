using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DicentDraw.Models.ViewModels
{
    public class MemberViewModel
    {
        [StringLength(12,ErrorMessage="{0}必需為{2}到{1}個字",MinimumLength=5)]
        [Required(ErrorMessage="{0}為必填")]
        [DisplayName("帳號")]
        public string Account { get; set; }
        [StringLength(10,ErrorMessage="{0}不可以超過{1}個字")]
        [Required(ErrorMessage = "{0}為必填")]
        [DisplayName("姓名")]
        public string Name { get; set; }
        [StringLength(100, ErrorMessage = "{0}不可以超過{1}個字")]
        [Required(ErrorMessage = "{0}為必填")]
        [DisplayName("地址")]
        public string Adress { get; set; }
        [Required(ErrorMessage = "{0}為必填")]
        [StringLength(10,ErrorMessage="電話號碼最多10碼")]
        [DisplayName("電話")]
        public string Telphone { get; set; }
        [Required(ErrorMessage = "{0}為必填")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "{0}不可以超過{1}個字")]
        [DisplayName("E-Mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0}為必填")]
        [StringLength(20, ErrorMessage = "{0}不可以超過{1}個字")]
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