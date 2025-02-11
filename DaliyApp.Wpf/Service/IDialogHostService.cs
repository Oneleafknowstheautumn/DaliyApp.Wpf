using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaliyApp.Wpf.Service
{
    public interface IDialogHostService
    {
        public void OnDialogOpening(IDialogParameters parameters);

        public DelegateCommand SaveCmm { get; set; }

        public DelegateCommand CancelCmm { get; set; }
    }
}