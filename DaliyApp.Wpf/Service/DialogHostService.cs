using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DaliyApp.Wpf.Service
{
    public class DialogHostService
    {
        private readonly IContainerExtension _containerExtension;

        public DialogHostService(IContainerExtension containerExtension)
        {
            _containerExtension = containerExtension;
        }

        public async Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters, string dialogname = "RootDialog")
        {
            if (parameters == null)
            {
                parameters = new DialogParameters();
            }
            //从容器中去除弹出窗口实例
            var content = _containerExtension.Resolve<object>(name);
            //验证实例有效性
            if (!(content is FrameworkElement dialogContent))
            {
                throw new NullReferenceException("对话框内容必须是框架元素");
            }
            if (dialogContent is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
            {
                ViewModelLocator.SetAutoWireViewModel(view, true);
            }
            if (!(dialogContent.DataContext is IDialogHostService viewModel))
            {
                throw new NullReferenceException("A dialog viemodel must implemt the IdialogAware interface");
            }
            DialogOpenedEventHandler eventHandler = (sender, eventArgs) =>
            {
                if (viewModel is IDialogHostService aware)
                {
                    aware.OnDialogOpening(parameters);
                }
                eventArgs.Session.UpdateContent(content);
            };
            return (IDialogResult)await DialogHost.Show(dialogContent, "RootDialog", eventHandler);
        }
    }
}