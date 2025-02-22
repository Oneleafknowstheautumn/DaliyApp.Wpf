using DaliyApp.Wpf.DTOs;
using DaliyApp.Wpf.HttpClients;
using DaliyApp.Wpf.Models;
using DaliyApp.Wpf.MsgEvents;
using DaliyApp.Wpf.Service;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
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

        //事件聚合器
        public readonly IEventAggregator _eventAggrega;

        //对话服务
        public readonly DialogHostService _dialogService;

        public HomeUCViewModel(HttpRestClient httpClients, DialogHostService dialogService, IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _httpClients = httpClients; //http客户端
            _eventAggrega = eventAggregator; //事件聚合器
            _dialogService = dialogService; //对话服务
            _regionManager = regionManager; //区域管理器
            stackPanelList = new List<StackPanelInfo>();
            watiInfoList = new List<WaitInfoDTO>();
            memorandaList = new List<MemorandumInfoDTO>();
            //创建首页模块
            CreateStackPanel();
            //待办信息
            ToDoInfolist();
            //备忘录信息
            MemorandaInfoList();
            //备忘录统计
            MemorandumCount();
            //添加待办事项
            OpenAddWaitCmm = new DelegateCommand(OpenAddWaitFunc);
            //添加备忘录
            AddMemorandumCmm = new DelegateCommand(AddMemorandumFunc);
            //修改待办状态
            chageWaitCmm = new DelegateCommand<WaitInfoDTO>(ChageWaitFunc);
            //编辑待办事项
            showEditWaitCmm = new DelegateCommand<WaitInfoDTO>(ShowEditWaitFunc);
            //打开页面命令
            OpenPageCmm = new DelegateCommand<StackPanelInfo>(OpenPageFunc);
            showEditMemorandumCmm = new DelegateCommand<MemorandumInfoDTO>(ShowEditMemorandumFunc);
            chageMemorandCmm = new DelegateCommand<MemorandumInfoDTO>(ChageMemorandFunc);
        }

        #region 首页模块

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

        public void CreateStackPanel()
        {
            stackPanelList.Add(new StackPanelInfo { Icon = "ClockFast", ItemName = "汇总", BackColor = "#FF0CA0FF", Result = "9", ViewName = "WaitUC" });
            stackPanelList.Add(new StackPanelInfo { Icon = "ClockCheckOutline", ItemName = "已完成", BackColor = "#FF1ECA3A", Result = "9", ViewName = "WaitUC" });
            stackPanelList.Add(new StackPanelInfo { Icon = "ChartLineVariant", ItemName = "比例", BackColor = "#FF02C6DC", Result = "90%", ViewName = "WaitUC" });
            stackPanelList.Add(new StackPanelInfo { Icon = "PlaylisStar", ItemName = "备忘录", BackColor = "#FFFFA000", Result = "9", ViewName = "WaitUC" });
        }

        #endregion 首页模块

        #region 页面跳转

        //打开页面命令
        public DelegateCommand<StackPanelInfo> OpenPageCmm { get; set; }

        //区域管理
        public readonly IRegionManager _regionManager;

        //打开页面
        public void OpenPageFunc(StackPanelInfo stackPanel)
        {
            if (stackPanel.ItemName == "已完成")
            {
                NavigationParameters pairs = new NavigationParameters();
                pairs.Add("selectIndex", 2);
                _regionManager.Regions["MainViewRegion"].RequestNavigate(stackPanel.ViewName, pairs);
            }
            else
            {
                //导航到指定页面
                _regionManager.Regions["MainViewRegion"].RequestNavigate(stackPanel.ViewName);
            }
        }

        #endregion 页面跳转

        #region 获取登陆信息

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

        #endregion 获取登陆信息

        #region 获取统计数据

        private StatWaitDTO statWait { get; set; } = new StatWaitDTO();

        /// <summary>
        /// 获取待办统计数据
        /// </summary>
        public void GetStaWait()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Route = "Wait/StatWait";
            apiRequest.Method = RestSharp.Method.GET;
            ApiReponses reponses = _httpClients.Excute(apiRequest);
            if (reponses.ResultCode == 1)
            {
                statWait = JsonConvert.DeserializeObject<StatWaitDTO>(reponses.ResultData.ToString());//反序列化
                RefershWaitStat();
            }
        }

        public void MemorandumCount()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.Route = "Memorandum/GetMemorandumCount";
            ApiReponses apiReponses = _httpClients.Excute(apiRequest);
            if (apiReponses.ResultCode == 1)
            {
                stackPanelList[3].Result = apiReponses.ResultData.ToString();
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

        #endregion 获取统计数据

        #region 获取待办信息列表

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
                GetStaWait();
                RefershWaitStat();
            }
            else
            {
                watiInfoList = new List<WaitInfoDTO>();
            }
        }

        #endregion 获取待办信息列表

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
                        ToDoInfolist();
                    }
                    else
                    {
                        MessageBox.Show(apiReponses.Message, "Error");
                    }
                }
            }
        }

        #endregion 添加待办事项

        #region 修改待办状态

        public DelegateCommand<WaitInfoDTO> chageWaitCmm { get; set; }

        public void ChageWaitFunc(WaitInfoDTO waitInfo)
        {
            ApiRequest request = new ApiRequest();
            request.Method = RestSharp.Method.PUT;
            request.Route = "Wait/UpdateWaitStatus";
            request.Parameters = waitInfo;
            ApiReponses apiReponses = _httpClients.Excute(request);
            if (apiReponses.ResultCode == 1)
            {
                //_eventAggrega.GetEvent<MsgEvent>().Publish(apiReponses.Message);
                MessageBox.Show("修改成功");
                ToDoInfolist();
            }
            else
            {
                MessageBox.Show(apiReponses.Message);
                //_eventAggrega.GetEvent<MsgEvent>().Publish(apiReponses.Message);
            }
        }

        #endregion 修改待办状态

        #region 编辑待办事项

        public DelegateCommand<WaitInfoDTO> showEditWaitCmm { get; set; }

        public async void ShowEditWaitFunc(WaitInfoDTO waitInfo)
        {
            DialogParameters pairs = new DialogParameters();
            pairs.Add("OldeWaitInfo", waitInfo);
            var resutl = await _dialogService.ShowDialog("EditWaitUC", pairs);
            if (resutl.Result == ButtonResult.OK)
            {
                if (resutl.Parameters.ContainsKey("EditWaitInfo"))
                {
                    var editWait = resutl.Parameters.GetValue<WaitInfoDTO>("EditWaitInfo");
                    ApiRequest request = new ApiRequest();
                    request.Method = RestSharp.Method.PUT;
                    request.Route = "Wait/EditWait";
                    request.Parameters = editWait;
                    ApiReponses reponses = _httpClients.Excute(request);
                    if (reponses.ResultCode == 1)
                    {
                        //_eventAggrega.GetEvent<MsgEvent>().Publish(reponses.Message);
                        MessageBox.Show("修改成功");
                    }
                    else
                    {
                        MessageBox.Show(reponses.Message, "Error");
                    }
                }
            }
        }

        #endregion 编辑待办事项

        #region 备忘录信息列表

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
            ApiRequest request = new ApiRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = "Memorandum/GetAllMemorandum";
            ApiReponses apiReponses = _httpClients.Excute(request);
            if (apiReponses.ResultCode == 1)
            {
                memorandaList = JsonConvert.DeserializeObject<List<MemorandumInfoDTO>>(apiReponses.ResultData.ToString());
                MemorandumCount();
            }
            else
            {
                memorandaList = new List<MemorandumInfoDTO>();
            }
        }

        #endregion 备忘录信息列表

        #region 修改备忘录状态

        public DelegateCommand<MemorandumInfoDTO> chageMemorandCmm { get; set; }

        public void ChageMemorandFunc(MemorandumInfoDTO memorandum)
        {
            ApiRequest request = new ApiRequest();
            request.Method = RestSharp.Method.PUT;
            request.Route = "Memorandum/UpStatus";
            request.Parameters = memorandum;
            ApiReponses apiReponses = _httpClients.Excute(request);
            if (apiReponses.ResultCode == 1)
            {
                MessageBox.Show("修改成功");
                MemorandaInfoList();
            }
            else
            {
                MessageBox.Show("修改失败");
            }
        }

        #endregion 修改备忘录状态

        #region 添加备忘录

        public DelegateCommand AddMemorandumCmm { get; set; }

        public async void AddMemorandumFunc()
        {
            var resutl = await _dialogService.ShowDialog("AddMemorandumUC", null);
            if (resutl.Result == ButtonResult.OK)
            {
                if (resutl.Parameters.ContainsKey("AddMemorandumInfo"))
                {
                    var addMemorandum = resutl.Parameters.GetValue<MemorandumInfoDTO>("AddMemorandumInfo");
                    ApiRequest request = new ApiRequest();
                    request.Method = RestSharp.Method.POST;
                    request.Route = "Memorandum/AddMemorandum";
                    request.Parameters = addMemorandum;
                    ApiReponses apiReponses = _httpClients.Excute(request);
                    if (apiReponses.ResultCode == 1)
                    {
                        MessageBox.Show("添加成功");
                        MemorandaInfoList();
                    }
                    else
                    {
                        MessageBox.Show(apiReponses.Message);
                    }
                }
            }
        }

        #endregion 添加备忘录

        #region 编辑备忘录

        public DelegateCommand<MemorandumInfoDTO> showEditMemorandumCmm { get; set; }

        public async void ShowEditMemorandumFunc(MemorandumInfoDTO memorandumInfo)
        {
            DialogParameters pairs = new DialogParameters();
            pairs.Add("OldeMemorandumInfo", memorandumInfo);
            var newrestful = await _dialogService.ShowDialog("EditMemorandumUC", pairs);
            if (newrestful.Result == ButtonResult.OK)
            {
                var getrestful = newrestful.Parameters.GetValue<MemorandumInfoDTO>("EditMemorandumInfo");
                ApiRequest request = new ApiRequest();
                request.Method = RestSharp.Method.PUT;
                request.Route = "Memorandum/UpdateMemorandum";
                request.Parameters = getrestful;
                ApiReponses apiReponses = _httpClients.Excute(request);
                if (apiReponses.ResultCode == 1)
                {
                    MessageBox.Show("修改成功");
                    MemorandaInfoList();
                }
                else
                {
                    MessageBox.Show(apiReponses.Message);
                }
            }
        }

        #endregion 编辑备忘录
    }
}