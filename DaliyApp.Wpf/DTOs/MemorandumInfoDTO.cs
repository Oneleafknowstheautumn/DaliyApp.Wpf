using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaliyApp.Wpf.DTOs
{
    public class MemorandumInfoDTO
    {
        //id
        public int MemorandumID { get; set; }

        //标题
        public string Title { get; set; }

        //内容
        public string Content { get; set; }

        //是否完成
        public int Status { get; set; }
    }
}