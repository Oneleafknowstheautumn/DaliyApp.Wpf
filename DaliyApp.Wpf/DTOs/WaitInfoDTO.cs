using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaliyApp.Wpf.DTOs
{   /// <summary>
    /// 待办事项DTO
    /// </summary>
    public class WaitInfoDTO
    {
        //id
        public int WaitId { get; set; }

        //标题
        public string Tittle { get; set; }

        //内容
        public string Contene { get; set; }

        //是否完成
        public int Status { get; set; }

        public string BackColor
        {
            get
            {
                return Status == 0 ? "#1E90FF" : "#3CB371";
            }
        }
    }
}