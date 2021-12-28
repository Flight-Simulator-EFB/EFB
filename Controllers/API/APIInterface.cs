using System.Collections.Generic;
using System.Text;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using EFB.Models;

namespace EFB.Controllers.API
{
    public class APIInterface
    {
        private HttpClient HttpClient { get; set; }

        public async Task<ResponseModel<T>> Get<T>(string Endpoint, Dictionary<string, string> Headers){

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

                    //Assess return value (object or raw string)
                    object response = resultString;

                    if (typeof(T) != typeof(string))
                    {//If the user requests string for return type
                        response = JsonConvert.DeserializeObject<T>(resultString);
                    }

                    return new ResponseModel<T>(){
                        //Sender should be aware of type T becuase of Generic type
                        Result = (T)response
                    };
                }
                catch (System.Exception e)
                {
                    return new ResponseModel<T>{Error = e.Message};
                }

            }
            //Returned in the event No other response has been returned
            return new ResponseModel<T>{Error = "Invalid Endpoint - Please try again later"};
            
        }

        public async Task<ResponseModel<T>> Post<T>(string Endpoint, Dictionary<string, string> Headers, HttpContent Body){

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
                    
                    object response = resultString;

                    if (typeof(T) != typeof(string))
                    {//If the user requests string for return type
                        response = JsonConvert.DeserializeObject<T>(resultString);
                    }

                    return new ResponseModel<T>(){
                        //Sender should be aware of type T becuase of Generic type
                        Result = (T)response
                    };
                }catch(System.Exception e){
                    return new ResponseModel<T>{Error = e.Message};
                }
            }
            //Returned in the event No other response has been returned
            return new ResponseModel<T>{Error = "Invalid Endpoint - Please try again later"};

        }



        public async Task<ResponseModel<T>> Put<T>(string Endpoint, Dictionary<string, string> Headers, HttpContent Body){

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
                    var pendingResult = this.HttpClient.PutAsync(Endpoint, Body);
                    var result = await pendingResult;
                    string resultString = result.Content.ReadAsStringAsync().Result;
                    
                    object response = resultString;

                    if (typeof(T) != typeof(string))
                    {//If the user requests string for return type
                        response = JsonConvert.DeserializeObject<T>(resultString);
                    }

                    return new ResponseModel<T>(){
                        //Sender should be aware of type T becuase of Generic type
                        Result = (T)response
                    };
                }catch(System.Exception e){
                    return new ResponseModel<T>{Error = e.Message};
                }
            }
            //Returned in the event No other response has been returned
            return new ResponseModel<T>{Error = "Invalid Endpoint - Please try again later"};

        }
        
        
    }

}