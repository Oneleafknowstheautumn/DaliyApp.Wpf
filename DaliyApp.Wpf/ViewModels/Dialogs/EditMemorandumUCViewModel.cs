using DaliyApp.Wpf.DTOs;
using DaliyApp.Wpf.Service;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DaliyApp.Wpf.ViewModels.Dialogs
{
    public class EditMemorandumUCViewModel : IDialogHostService
    {
        public EditMemorandumUCViewModel()
        {
            SaveCmm = new DelegateCommand(SaveDialog);
            CancelCmm = new DelegateCommand(CloseDialog);
        }

        public DelegateCommand SaveCmm { get; set; }
        public DelegateCommand CancelCmm { get; set; }

        public void OnDialogOpening(IDialogParameters parameters)
        {
            editmemorandum = parameters.GetValue<MemorandumInfoDTO>("OldeMemorandumInfo");
        }

        public MemorandumInfoDTO editmemorandum { get; set; } = new MemorandumInfoDTO();

        private void CloseDialog()
        {
            if (DialogHost.IsDialogOpen("RootDialog"))
            {
                DialogHost.Close("RootDialog", new DialogResult(ButtonResult.Cancel));
            }
        }

        private void SaveDialog()
        {
            if (string.IsNullOrEmpty(editmemorandum.Title) || string.IsNullOrEmpty(editmemorandum.Content))
            {
                MessageBox.Show("输入信息不全");
            }
            if (DialogHost.IsDialogOpen("RootDialog"))
            {
                DialogParameters paras = new DialogParameters();
                paras.Add("EditMemorandumInfo", editmemorandum);
                DialogHost.Close("RootDialog", new DialogResult(ButtonResult.OK, paras));
            }
        }
    }
}