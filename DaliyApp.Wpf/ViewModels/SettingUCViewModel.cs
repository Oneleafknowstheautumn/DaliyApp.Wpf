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
    public class SettingUCViewModel : BindableBase
    {
        public SettingUCViewModel(IRegionManager regionManager)
        {
            personalMenuList = new List<LeftMenuInfo>();

            CreateLeftMenus();

            _regionManager = regionManager;
            navigateperCmm = new DelegateCommand<LeftMenuInfo>(NavigatePersonal);
        }

        private List<LeftMenuInfo> _personalMenuList;

        public List<LeftMenuInfo> personalMenuList
        {
            get { return _personalMenuList; }
            set
            {
                _personalMenuList = value;
                RaisePropertyChanged();
            }
        }

        private void CreateLeftMenus()
        {
            personalMenuList.Add(new LeftMenuInfo { Icon = "Palette", MenuName = "个性化", ViewName = "PersonalUC" });
            personalMenuList.Add(new LeftMenuInfo { Icon = "Cog", MenuName = "系统设置", ViewName = "SysSetUC" });
            personalMenuList.Add(new LeftMenuInfo { Icon = "Information", MenuName = "关于我们", ViewName = "AboutUsUC" });
        }

        public readonly IRegionManager _regionManager;
        public DelegateCommand<LeftMenuInfo> navigateperCmm { get; set; }

        public void NavigatePersonal(LeftMenuInfo menuInfo)
        {
            _regionManager.Regions["SettingViewRegion"].RequestNavigate(menuInfo.ViewName);
        }
    }
}