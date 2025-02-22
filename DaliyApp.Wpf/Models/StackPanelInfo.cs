using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaliyApp.Wpf.Models
{
    public class StackPanelInfo : BindableBase
    {
        //图标
        public string Icon { get; set; }

        //名称
        public string ItemName { get; set; }

        //结果
        //  public string Result { get; set; }

        private string _result;

        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                RaisePropertyChanged();
            }
        }

        //背景颜色
        public string BackColor { get; set; }

        //视图名
        public string ViewName { get; set; }

        public string Hand
        {
            get
            {
                if (ItemName == "比例")
                {
                    return "";
                }
                else
                {
                    return "Hand";
                }
            }
        }
    }
}