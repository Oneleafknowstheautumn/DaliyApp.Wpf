using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaliyApp.Wpf.HttpClients
{
    /// <summary>
    /// 接受的结果
    /// </summary>
    public class ApiReponses
    {
        /// <summary>
        /// 结果代码
        /// </summary>
        public int ResultCode { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 结果数据
        /// </summary>
        public object ResultData { get; set; }
    }
}