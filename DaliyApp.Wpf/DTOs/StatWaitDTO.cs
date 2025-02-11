using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaliyApp.Wpf.DTOs
{
    public class StatWaitDTO
    {
        public int TotalCount { get; set; }
        public int CompletedCount { get; set; }

        public string FinishPercent { get; set; }
    }
}