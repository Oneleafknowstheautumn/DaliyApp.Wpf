using DaliyApp.Wpf.MsgEvents;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DaliyApp.Wpf.Views
{
    /// <summary>
    /// LoginUC.xaml 的交互逻辑
    /// </summary>
    public partial class LoginUC : UserControl
    {
        private readonly IEventAggregator _eventAggregator;

        public LoginUC(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            //消息订阅
            _eventAggregator = eventAggregator;
            //
            _eventAggregator.GetEvent<MsgEvent>().Subscribe(MsgEventReceived);
        }

        /// <summary>
        /// 消息订阅
        /// </summary>
        /// <param name="obj">接收订阅的消息</param>
        private void MsgEventReceived(string obj)
        {
            RegUserMeg.MessageQueue.Enqueue(obj);
        }
    }
}