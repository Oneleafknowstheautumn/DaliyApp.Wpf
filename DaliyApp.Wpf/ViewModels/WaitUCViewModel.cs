using DaliyApp.Wpf.DTOs;
using DaliyApp.Wpf.HttpClients;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DaliyApp.Wpf.ViewModels
{
    public class WaitUCViewModel : BindableBase, INavigationAware
    {
        public readonly HttpRestClient _httpClients;

        public WaitUCViewModel(HttpRestClient httpRestClient)
        {
            _httpClients = httpRestClient;
            // waitList = new List<WaitInfoDTO>();
            //添加待办右侧窗口打开
            showAddCmm = new DelegateCommand(showAddWait);
            queryWaitListCmm = new DelegateCommand(QueryWaitInfo);
        }

        //待办事项列表
        private List<WaitInfoDTO> _waitList;

        public List<WaitInfoDTO> waitList
        {
            get { return _waitList; }
            set
            {
                _waitList = value;
                RaisePropertyChanged();
            }
        }

        //查询待办命令
        public DelegateCommand queryWaitListCmm { get; set; }

        public string searchTitle { get; set; }

        //待办条件
        private int _selectIndex;

        public int selectIndex
        {
            get { return _selectIndex; }
            set
            {
                _selectIndex = value;
                RaisePropertyChanged();//属性改变通知前端
            }
        }

        private void QueryWaitInfo()
        {
            waitList = new List<WaitInfoDTO>();
            int? status = null;
            if (selectIndex == 1)
            {
                status = 0;
            }
            if (selectIndex == 2)
            {
                status = 1;
            }
            // var r = searchTitle;
            // var t = selectIndex;
            //查询待办事项
            ApiRequest request = new ApiRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"Wait/QueryWaitList?tittle={searchTitle}&status={status}";
            ApiReponses apiReponses = _httpClients.Excute(request);
            if (apiReponses.ResultCode == 1)
            {
                waitList = JsonConvert.DeserializeObject<List<WaitInfoDTO>>(apiReponses.ResultData.ToString());
            }
            else
            {
                MessageBox.Show(apiReponses.Message);
            }
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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("selectIndex"))
            {
                selectIndex = navigationContext.Parameters.GetValue<int>("selectIndex");
            }
            else
            {
                selectIndex = 0;
            }
            QueryWaitInfo();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}