using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using EFB.Models;

namespace EFB.Controllers.API
{
    public class APIInterface
    {
        private HttpClient HttpClient { get; set; }

        public async Task<ResponseModel> Get<T>(string Endpoint, Dictionary<string, string> Headers){

            this.HttpClient = new HttpClient();

            this.HttpClient.DefaultRequestHeaders.Clear();

            if (Headers != null)
            {
                foreach (var Header in Headers)
                {
                    this.HttpClient.DefaultRequestHeaders.Add(Header.Key, Header.Value);
                }
            }

            if (Form.FormAuthenticator.ValidateEndpoint(Endpoint))
            {
                try
                {
                    var pendingResult = this.HttpClient.GetAsync(Endpoint);

                    var result = await pendingResult;

                    string resultString = result.Content.ReadAsStringAsync().Result;

                    return new ResponseModel{
                        //Sender should be aware of type T becuase of Generic function
                        Result = JsonConvert.DeserializeObject<T>(resultString)
                    };
                }
                catch (System.Exception e)
                {
                    return new ResponseModel{Error = e.Message};
                }

            }
            //Returned in the event No other response has been returned
            return new ResponseModel{Error = "Invalid Endpoint - Please try again later"};
            
        }

        public async Task<ResponseModel> Post<T>(string Endpoint, Dictionary<string, string> Headers, HttpContent Body){

            this.HttpClient = new HttpClient();
            this.HttpClient.DefaultRequestHeaders.Clear();

            if (Headers != null)
            {
                foreach (var Header in Headers)
                {
                    this.HttpClient.DefaultRequestHeaders.Add(Header.Key, Header.Value);
                }
            }

            if (Form.FormAuthenticator.ValidateEndpoint(Endpoint))
            {
                try{//Try statement to catch errors in the process of making the request
                    var pendingResult = this.HttpClient.PostAsync(Endpoint, Body);
                    var result = await pendingResult;
                    string resultString = result.Content.ReadAsStringAsync().Result;

                    return new ResponseModel{
                        //Sender should be aware of type T becuase of Generic function
                        Result = JsonConvert.DeserializeObject<T>(resultString)
                    };
                }catch(System.Exception e){
                    return new ResponseModel{Error = e.Message};
                }
            }
            //Returned in the event No other response has been returned
            return new ResponseModel{Error = "Invalid Endpoint - Please try again later"};

        }
        
        
    }

}