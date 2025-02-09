using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaliyApp.Wpf.DTOs
{
    public class AccountInfoDTO
    {
        public string UserNname { get; set; }
        public string UserNo { get; set; }
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}