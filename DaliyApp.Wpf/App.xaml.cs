using DaliyApp.Wpf.HttpClients;
using DaliyApp.Wpf.ViewModels;
using DaliyApp.Wpf.Views;
using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using RestSharp;
using System.Configuration;
using System.Data;
using System.Windows;

namespace DaliyApp.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        /// <summary>
        /// 设置启动窗口
        /// </summary>
        /// <returns></returns>
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWin>();
        }

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //登陆窗口
            containerRegistry.RegisterDialog<LoginUC, LoginUCViewModel>();

            //api请求
            containerRegistry.GetContainer().Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "weburl"));
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnInitialized()
        {
            var dailog = Container.Resolve<IDialogService>();
            //显示登陆窗口
            dailog.ShowDialog("LoginUC",
            callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Environment.Exit(0);
                    return;
                }
                base.OnInitialized();
            });
        }
    }
}