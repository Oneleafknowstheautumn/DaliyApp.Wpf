using DaliyApp.Wpf.DTOs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaliyApp.Wpf.ViewModels
{
    public class MemorandumUCViewModel : BindableBase
    {
        public MemorandumUCViewModel()
        {
            memorandumlist = new List<MemorandumInfoDTO>();
            memorandumFunc();
            memorandCmm = new DelegateCommand(AddMemorandFunc);
        }

        private List<MemorandumInfoDTO> _memorandumlist;

        public List<MemorandumInfoDTO> memorandumlist
        {
            get { return _memorandumlist; }
            set
            {
                _memorandumlist = value;
                RaisePropertyChanged();
            }
        }

        private void memorandumFunc()
        {
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "12312312312312312", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "12312312312312312", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "12312312312312312", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "12312312312312312", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "12312312312312312", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "12312312312312312", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "12312312312312312", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "12312312312312312", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "12312312312312312", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "12312312312312312", Status = 1 });
            memorandumlist.Add(new MemorandumInfoDTO { Tittle = "马喽日常", Content = "测试", Status = 1 });
        }

        private bool _isAddMemorand;

        public bool isAddMemorand
        {
            get { return _isAddMemorand; }
            set
            {
                _isAddMemorand = value;
                RaisePropertyChanged();
            }
        }

        public void AddMemorandFunc()
        {
            isAddMemorand = true;
        }

        public DelegateCommand memorandCmm { get; set; }
    }
}