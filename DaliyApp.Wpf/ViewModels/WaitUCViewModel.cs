using DaliyApp.Wpf.DTOs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaliyApp.Wpf.ViewModels
{
    public class WaitUCViewModel : BindableBase
    {
        public WaitUCViewModel()
        {
            waitList = new List<MemorandumInfoDTO>();
            MemorandumInfo();
            showAddCmm = new DelegateCommand(showAddWait);
        }

        private List<MemorandumInfoDTO> _waitList;

        public List<MemorandumInfoDTO> waitList
        {
            get { return _waitList; }
            set
            {
                _waitList = value;
                RaisePropertyChanged();
            }
        }

        private void MemorandumInfo()
        {
            waitList.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            waitList.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "12312312312312312", Status = 1 });
            waitList.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
        }

        #region 右侧待办打开

        //添加待办右侧窗口打开
        private bool _isshowWait;

        public bool isshowWait
        {
            get { return _isshowWait; }
            set
            {
                _isshowWait = value;
                RaisePropertyChanged();
            }
        }

        public void showAddWait()
        {
            isshowWait = true;
        }

        public DelegateCommand showAddCmm { get; set; }

        #endregion 右侧待办打开
    }
}