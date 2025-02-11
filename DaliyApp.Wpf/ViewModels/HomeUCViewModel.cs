using DaliyApp.Wpf.DTOs;
using DaliyApp.Wpf.HttpClients;
using DaliyApp.Wpf.Models;
using DaliyApp.Wpf.Service;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DaliyApp.Wpf.ViewModels
{
    public class HomeUCViewModel : BindableBase, INavigationAware
    {
        //http客户端
        public readonly HttpRestClient _httpClients;

        //对话服务
        public readonly DialogHostService _dialogService;

        public HomeUCViewModel(HttpRestClient httpClients, DialogHostService dialogService)
        {
            _httpClients = httpClients;

            stackPanelList = new List<StackPanelInfo>();
            watiInfoList = new List<WaitInfoDTO>();
            memorandaList = new List<MemorandumInfoDTO>();

            CreateStackPanel();
            //待办信息
            ToDoInfolist();
            //备忘录信息
            MemorandaInfoList();
            //添加待办事项
            OpenAddWaitCmm = new DelegateCommand(OpenAddWaitFunc);
            _dialogService = dialogService;

        }

        /// <summary>
        /// 首页模块
        /// </summary>
        private List<StackPanelInfo> _stackPanelList;

        public List<StackPanelInfo> stackPanelList
        {
            get { return _stackPanelList; }
            set
            {
                _stackPanelList = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void CreateStackPanel()
        {
            stackPanelList.Add(new StackPanelInfo { Icon = "ClockFast", ItemName = "汇总", BackColor = "#FF0CA0FF", Result = "9", ViewName = "WaitUC" });
            stackPanelList.Add(new StackPanelInfo { Icon = "ClockCheckOutline", ItemName = "已完成", BackColor = "#FF1ECA3A", Result = "9", ViewName = "WaitUC" });
            stackPanelList.Add(new StackPanelInfo { Icon = "ChartLineVariant", ItemName = "比例", BackColor = "#FF02C6DC", Result = "90%", ViewName = "WaitUC" });
            stackPanelList.Add(new StackPanelInfo { Icon = "PlaylisStar", ItemName = "备忘录", BackColor = "#FFFFA000", Result = "9", ViewName = "WaitUC" });
        }

        #region 获取待办信息

        public List<WaitInfoDTO> _watiInfoList;

        public List<WaitInfoDTO> watiInfoList
        {
            get { return _watiInfoList; }
            set
            {
                _watiInfoList = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 获取待办事项信息
        /// </summary>
        public void ToDoInfolist()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.Route = "Wait/WaitList";
            apiRequest.ContentType = "application/json";
            ApiReponses apiReponses = _httpClients.Excute(apiRequest);
            if (apiReponses.ResultCode == 1)
            {
                watiInfoList = JsonConvert.DeserializeObject<List<WaitInfoDTO>>(apiReponses.ResultData.ToString());
                RefershWaitStat();
            }
            else
            {
                watiInfoList = new List<WaitInfoDTO>();
            }
        }

        public void RefershWaitList()
        {
            for (int i = 0; i < watiInfoList.Count; i++)
            {
                //stackPanelList[0].Result = statWait.TotalCount.ToString();
                watiInfoList[i].Tittle = watiInfoList[i].Tittle.ToString();
                watiInfoList[i].Contene = watiInfoList[i].Contene.ToString();
            }
        }

        #endregion 获取待办信息

        private List<MemorandumInfoDTO> _memorandaList;

        public List<MemorandumInfoDTO> memorandaList
        {
            get { return _memorandaList; }
            set
            {
                _memorandaList = value;
                RaisePropertyChanged();
            }
        }

        public void MemorandaInfoList()
        {
            memorandaList.Add(new MemorandumInfoDTO { Tittle = "马喽会议", Content = "测试", Status = 1 });
            memorandaList.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "12312312312312312", Status = 1 });
            memorandaList.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
        }

        private string _logininfo;

        public string LoginInfo
        {
            get { return _logininfo; }
            set
            {
                _logininfo = value;
                RaisePropertyChanged();
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("LoginName"))
            {
                DateTime time = DateTime.Now;
                string[] week = ["星期天", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"];
                string name = navigationContext.Parameters.GetValue<string>("LoginName");
                LoginInfo = $"你好,{name},今天是{time.ToString("d")}，{week[(int)time.DayOfWeek]}";
                GetStaWait();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #region 获取待办信息

        private StatWaitDTO statWait { get; set; } = new StatWaitDTO();

        /// <summary>
        /// 获取待办信息
        /// </summary>
        public void GetStaWait()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Route = "Wait/StatWait";
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.ContentType = "application/json";
            ApiReponses reponses = _httpClients.Excute(apiRequest);
            if (reponses.ResultCode == 1)
            {
                statWait = JsonConvert.DeserializeObject<StatWaitDTO>(reponses.ResultData.ToString());//反序列化
                RefershWaitStat();
            }
        }

        /// <summary>
        /// 更新待办信息
        /// </summary>
        public void RefershWaitStat()
        {
            stackPanelList[0].Result = statWait.TotalCount.ToString();
            stackPanelList[1].Result = statWait.CompletedCount.ToString();
            stackPanelList[2].Result = statWait.FinishPercent;
        }

        #endregion 获取待办信息

        #region 添加待办事项

        public DelegateCommand OpenAddWaitCmm { get; set; }

        public async void OpenAddWaitFunc()
        {
            var resutl = await _dialogService.ShowDialog("AddWaitUC", null);
            if (resutl.Result == ButtonResult.OK)
            {
                //接受数据
                if (resutl.Parameters.ContainsKey("AddWaitInfo"))
                {
                    var addWait = resutl.Parameters.GetValue<WaitInfoDTO>("AddWaitInfo");
                    //调用api
                    ApiRequest apiRequest = new ApiRequest();
                    apiRequest.Method = RestSharp.Method.POST;
                    apiRequest.ContentType = "application/json";
                    apiRequest.Route = "Wait/AddWait";
                    apiRequest.Parameters = addWait;
                    ApiReponses apiReponses = _httpClients.Excute(apiRequest);
                    if (apiReponses.ResultCode == 1)
                    {
                        MessageBox.Show(apiReponses.Message);
                        GetStaWait();
                    }
                    else
                    {
                        MessageBox.Show(apiReponses.Message, "Error");
                    }
                }
            }
        }

        #endregion 添加待办事项
    }
}