using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Models
{
    public class UserEmailOptions
    {
        //邮件的发送地址，可以是多个地址
        public List<string> ToEmails { get; set; }
        //邮件的主题
        public string Subject { get; set; }
        //邮件内容
        public string Body { get; set; }
        //邮件中的所有替换字段
        public List<KeyValuePair<string, string>> PlaceHolders { get; set; }
    }
}
