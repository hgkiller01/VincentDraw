using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DicentDraw.Models
{
    [MetadataType(typeof(MessageMD))]
    public partial class Message
    {
        public class MessageMD
        {
            public int MessageID { get; set; }
            [DisplayName("姓名")]
            [Required(ErrorMessage="{0}必填")]
            [StringLength(20,ErrorMessage="{0}最多為{1}個字")]
            public string Name { get; set; }
            [Required(ErrorMessage="{0}必填")]
            [DisplayName("E-Mail")]
            [StringLength(50,ErrorMessage="{0}最多為{1}個字")]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            [DisplayName("意見內容")]
            [DataType(DataType.MultilineText)]
            [Required(ErrorMessage="請輸入意見內容")]
            public string MessageInfo { get; set; }
        }

    }
}