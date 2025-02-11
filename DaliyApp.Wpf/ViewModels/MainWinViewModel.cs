using DaliyApp.Wpf.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaliyApp.Wpf.ViewModels
{
    public class MainWinViewModel : BindableBase
    {
        public MainWinViewModel(IRegionManager regionManager)
        {
            leftMenuList = new List<LeftMenuInfo>();

            //创建左侧菜单
            CreateLeftMenus();

            _regionManager = regionManager;

            //导航
            navigateCmm = new DelegateCommand<LeftMenuInfo>(Navigate);

            //前进
            goforwardCmm = new DelegateCommand(GoforWard);

            //后退
            gobackCmm = new DelegateCommand(Goback);

            //返回首页按钮
            homeCommand = new DelegateCommand<string>(SetDefaNav);
        }

        public DelegateCommand<string> homeCommand { get; set; }

        #region 左侧菜单

        /// <summary>
        /// 左侧菜单集合
        /// </summary>
        private List<LeftMenuInfo> _leftMenuList;

        public List<LeftMenuInfo> leftMenuList
        {
            get { return _leftMenuList; }
            set
            {
                _leftMenuList = value;

                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 创建左侧菜单
        /// </summary>
        private void CreateLeftMenus()
        {
            leftMenuList.Add(new LeftMenuInfo
            {
                Icon = "Home",
                MenuName = "首页",
                ViewName = "HomeUC"
            });
            leftMenuList.Add(new LeftMenuInfo
            {
                Icon = "NoteBookOutLine",
                MenuName = "代办事项",
                ViewName = "WaitUC"
            });
            leftMenuList.Add(new LeftMenuInfo
            {
                Icon = "NotebookPlus",
                MenuName = "备忘录",
                ViewName = "MemorandumUC"
            });
            leftMenuList.Add(new LeftMenuInfo
            {
                Icon = "Cog",
                MenuName = "设置",
                ViewName = "SettingUC"
            });
        }

        #endregion 左侧菜单

        #region 导航页面跳转

        private readonly IRegionManager _regionManager;

        public DelegateCommand<LeftMenuInfo> navigateCmm { get; set; }

        /// <summary>
        /// 导航
        /// </summary>
        /// <param name="leftMenu">左侧菜单信息</param>
        private void Navigate(LeftMenuInfo leftMenu)
        {
            if (leftMenu == null || string.IsNullOrEmpty(leftMenu.ViewName))
            {
                return;
            }
            _regionManager.Regions["MainViewRegion"].RequestNavigate(leftMenu.ViewName, callback =>
            {
                _journal = callback.Context.NavigationService.Journal; //记录导航足迹
            });
        }

        #endregion 导航页面跳转

        #region 前进后退

        //前进后退实现
        public IRegionNavigationJournal _journal { get; set; }

        public DelegateCommand goforwardCmm { get; set; }//前进
        public DelegateCommand gobackCmm { get; set; }//后退

        private void GoforWard()
        {
            if (_journal != null && _journal.CanGoForward)
            {
                _journal.GoForward();
            }
        }

        private void Goback()
        {
            if (_journal != null && _journal.CanGoBack)
            {
                _journal.GoBack();
            }
        }

        #endregion 前进后退

        //默认导航主页
        public void SetDefaNav(string name)
        {
            NavigationParameters pairs = new NavigationParameters();
            pairs.Add("LoginName", name);
            _regionManager.Regions["MainViewRegion"].RequestNavigate("HomeUC",pairs);
        }
    }
}