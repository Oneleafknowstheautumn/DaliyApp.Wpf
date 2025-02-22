using DaliyApp.Wpf.DTOs;
using DaliyApp.Wpf.MsgEvents;
using DaliyApp.Wpf.Service;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///添加待办事项
namespace DaliyApp.Wpf.ViewModels.Dialogs
{
    public class AddWaitUCViewModel : IDialogHostService
    {
        private readonly IEventAggregator _eventAggregator;

        public AddWaitUCViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            CancelCmm = new DelegateCommand(CloseDialog);
            SaveCmm = new DelegateCommand(SaveDialog);
        }

        public DelegateCommand SaveCmm { get; set; }
        public DelegateCommand CancelCmm { get; set; }

        public void OnDialogOpening(IDialogParameters parameters)
        {
        }

        private void CloseDialog()
        {
            if (DialogHost.IsDialogOpen("RootDialog"))
            {
                DialogHost.Close("RootDialog", new DialogResult(ButtonResult.Cancel));
            }
        }

        public WaitInfoDTO WaitInfo { get; set; } = new WaitInfoDTO();

        private void SaveDialog()
        {
            if (string.IsNullOrEmpty(WaitInfo.Tittle) || string.IsNullOrEmpty(WaitInfo.Contene))
            {
                _eventAggregator.GetEvent<MsgEvent>().Publish("信息不全");
            }
            if (DialogHost.IsDialogOpen("RootDialog"))
            {
                DialogParameters paras = new DialogParameters();
                paras.Add("AddWaitInfo", WaitInfo);
                DialogHost.Close("RootDialog", new DialogResult(ButtonResult.OK, paras));
            }
        }
    }
}