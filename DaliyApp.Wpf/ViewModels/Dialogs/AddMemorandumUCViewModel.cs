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
    internal class AddMemorandumUCViewModel : IDialogHostService
    {
        public AddMemorandumUCViewModel()
        {
            SaveCmm = new DelegateCommand(SaveDialog);
            CancelCmm = new DelegateCommand(CloseDialog);
        }

        public DelegateCommand SaveCmm { get; set; }
        public DelegateCommand CancelCmm { get; set; }

        public void OnDialogOpening(IDialogParameters parameters)
        {
        }

        public MemorandumInfoDTO memorandum { get; set; } = new MemorandumInfoDTO();

        private void SaveDialog()
        {
            if (string.IsNullOrEmpty(memorandum.Title) || string.IsNullOrEmpty(memorandum.Content))
            {
                MessageBox.Show("信息不全");
            }
            if (DialogHost.IsDialogOpen("RootDialog"))
            {
                DialogParameters paras = new DialogParameters();
                paras.Add("AddMemorandumInfo", memorandum);
                DialogHost.Close("RootDialog", new DialogResult(ButtonResult.OK, paras));
            }
        }

        //关闭对话框
        private void CloseDialog()
        {
            if (DialogHost.IsDialogOpen("RootDialog"))
            {
                DialogHost.Close("RootDialog", new DialogResult(ButtonResult.Cancel));
            }
        }
    }
}