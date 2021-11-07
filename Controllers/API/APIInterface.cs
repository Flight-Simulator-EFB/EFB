using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace EFB.Controllers.API
{
    public class APIInterface
    {
        private HttpClient HttpClient { get; set; }

        public async Task<T> Get<T>(string Endpoint, Dictionary<string, string> Headers){

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
                var pendingResult = this.HttpClient.GetAsync(Endpoint);

                var result = await pendingResult;

                string resultString = result.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<T>(resultString);

            }else{
                
                T empty = default(T);

                return empty;

            }
            
        }

        public async Task<T> Post<T>(string Endpoint, Dictionary<string, string> Headers, HttpContent Body){

            this.HttpClient = new HttpClient();

            //this.HttpClient.DefaultRequestHeaders.Clear();

            if (Headers != null)
            {
                foreach (var Header in Headers)
                {
                    this.HttpClient.DefaultRequestHeaders.Add(Header.Key, Header.Value);
                }
            }

            if (Form.FormAuthenticator.ValidateEndpoint(Endpoint))
            {
                var pendingResult = this.HttpClient.PostAsync(Endpoint, Body);

                var result = await pendingResult;

                string resultString = result.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<T>(resultString);

            }else{
                
                T empty = default(T);

                return empty;

            }
            

        }
        
        
    }
}