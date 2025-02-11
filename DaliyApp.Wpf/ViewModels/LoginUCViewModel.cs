using DaliyApp.Wpf.DTOs;
using DaliyApp.Wpf.HttpClients;
using DaliyApp.Wpf.MsgEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DaliyApp.Wpf.ViewModels
{
    /// <summary>
    /// 登陆窗口模型
    /// </summary>
    public class LoginUCViewModel : BindableBase, IDialogAware
    {
        public readonly HttpRestClient _httpRest;

        public readonly IEventAggregator _eventAggregator;
        public string Title { get; set; } = "我的日常";

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand LoginCmmmand { get; set; }

        public LoginUCViewModel(HttpRestClient httpRest, IEventAggregator eventAggregator)
        {
            //登陆页面
            LoginCmmmand = new DelegateCommand(LginFuc);
            //注册页面
            ShowRegInfoCmm = new DelegateCommand<string>(ShowRegInfo);

            //注册用户
            Regcmm = new DelegateCommand(RegFuc);

            //初始化
            accountInfoDTO = new AccountInfoDTO();

            //http请求
            _httpRest = httpRest;

            _eventAggregator = eventAggregator;
        }

        #region 登陆实现

        /// <summary>
        /// 账号密码
        /// </summary>
        private string _userno;

        public string userno
        {
            get { return _userno; }
            set { _userno = value; }
        }

        private string _pwd;

        public string pwd
        {
            get { return _pwd; }
            set { _pwd = value; }
        }

        /// <summary>
        /// 登陆方法验证
        /// </summary>
        private void LginFuc()
        {
            //验证用户信息
            if (string.IsNullOrEmpty(userno) || string.IsNullOrEmpty(pwd))
            {
                _eventAggregator.GetEvent<MsgEvent>().Publish("用户信息不全");
                return;
            }
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Route = $"Account/LoginResult?userno={userno}&password={pwd}";
            apiRequest.Method = RestSharp.Method.GET;
            //执行请求
            ApiReponses apiReponses = _httpRest.Excute(apiRequest);
            if (apiReponses.ResultCode == 1)
            {
                if (RequestClose != null)
                {
                    //Json反序列画
                    AccountInfoDTO accountInfoDTO = JsonConvert.DeserializeObject<AccountInfoDTO>(apiReponses.ResultData.ToString());
                    DialogParameters pars = new DialogParameters();
                    //获取登陆名
                    pars.Add("LoginName", accountInfoDTO.UserNname);
                    RequestClose(new DialogResult(ButtonResult.OK, pars));
                }
            }
            else
            {
                _eventAggregator.GetEvent<MsgEvent>().Publish(apiReponses.Message);
            }
        }

        #endregion 登陆实现

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        #region 注册实现

        public DelegateCommand Regcmm { get; set; }

        private void RegFuc()
        {
            ///验证用户信息
            if (string.IsNullOrEmpty(accountInfoDTO.UserNname) || string.IsNullOrEmpty(accountInfoDTO.Password)
                || string.IsNullOrEmpty(accountInfoDTO.ConfirmPassword))
            {
                _eventAggregator.GetEvent<MsgEvent>().Publish("用户信息不全");
                //MessageBox.Show("用户信息不全");
                return;
            }

            if (accountInfoDTO.Password != accountInfoDTO.ConfirmPassword)
            {
                _eventAggregator.GetEvent<MsgEvent>().Publish("两次密码不一致");
                //MessageBox.Show("两次密码不一致");
                return;
            }
            //发送请求
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Route = "Account/RegUser";
            apiRequest.Method = RestSharp.Method.POST;
            apiRequest.Parameters = accountInfoDTO;
            //执行请求
            ApiReponses apiReponses = _httpRest.Excute(apiRequest);
            if (apiReponses.ResultCode == 1)
            {
                _eventAggregator.GetEvent<MsgEvent>().Publish(apiReponses.Message);
                //MessageBox.Show(apiReponses.Message);
                selectedindex = 0;
            }
            else
            {
                _eventAggregator.GetEvent<MsgEvent>().Publish(apiReponses.Message);
            }
        }

        private AccountInfoDTO _accountInfoDTO;

        public AccountInfoDTO accountInfoDTO
        {
            get { return _accountInfoDTO; }
            set
            {
                _accountInfoDTO = value;
                RaisePropertyChanged();
            }
        }

        #endregion 注册实现

        #region 显示登陆索引

        /// <summary>
        /// 显示登陆索引
        /// </summary>
        private int _selectedindex;

        public int selectedindex
        {
            get { return _selectedindex; }
            set
            {
                _selectedindex = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand<string> ShowRegInfoCmm { get; set; }

        private void ShowRegInfo(string index)
        {
            selectedindex = Convert.ToInt32(index);
        }

        #endregion 显示登陆索引
    }
}