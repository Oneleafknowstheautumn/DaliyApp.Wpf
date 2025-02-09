using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace DaliyApp.Wpf.HttpClients
{
    public class HttpRestClient
    {
        private readonly RestClient _client;//客户端

        private readonly string baseurl = "https://localhost:44345/api/";

        public HttpRestClient()
        {
            _client = new RestClient();
        }

        public ApiReponses Excute(ApiRequest apirequest)
        {
            RestRequest rest = new RestRequest(apirequest.Method);//请求方式
            rest.AddHeader("Content-Type", apirequest.ContentType);//数据类型
            //参数序列号
            if (apirequest.Parameters != null)
            {
                //SerializeObject 序列号 对象->JSON
                rest.AddParameter("param", JsonConvert.SerializeObject(apirequest.Parameters), ParameterType.RequestBody);
            }
            //BaseUrl 基础地址+路由
            _client.BaseUrl = new Uri(baseurl + apirequest.Route);
            var res = _client.Execute(rest); //执行请求
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //DeserializeObject 反序列化 JSON->对象
                return JsonConvert.DeserializeObject<ApiReponses>(res.Content);
            }
            else
            {
                //返回错误信息
                return new ApiReponses { ResultCode = 99, Message = "服务器忙", ResultData = null };
            }
        }
    }
}