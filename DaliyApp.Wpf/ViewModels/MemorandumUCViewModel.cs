using DaliyApp.Wpf.DTOs;
using DaliyApp.Wpf.HttpClients;
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
        public readonly HttpRestClient _httpClients;

        public MemorandumUCViewModel(HttpRestClient httpRestClient)
        {
            _httpClients = httpRestClient;
            memorandumlist = new List<MemorandumInfoDTO>();

            memorandCmm = new DelegateCommand(AddMemorandFunc);
            searchCmm = new DelegateCommand(memorandumFunc);
            memorandumFunc();
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

        public DelegateCommand searchCmm { get; set; }
        public string searchTitle { get; set; }
        public int selectedIndex { get; set; }

        private void memorandumFunc()
        {
            int? stu = null;
            if (selectedIndex == 1)
            {
                stu = 0;
            }
            if (selectedIndex == 2)
            {
                stu = 1;
            }
            ApiRequest request = new ApiRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"Memorandum/SearchMemorandum?{searchTitle}&&{stu}";
            ApiReponses apiReponses = _httpClients.Excute(request);
            if (apiReponses.ResultCode == 1)
            {
                //将json字符串转换为对象
                memorandumlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MemorandumInfoDTO>>(apiReponses.ResultData.ToString());
            }
            else
            {
                memorandumlist = new List<MemorandumInfoDTO>();
            }
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