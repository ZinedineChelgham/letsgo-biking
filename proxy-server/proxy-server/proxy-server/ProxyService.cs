using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.ServiceModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace proxy_server
{
   public class ProxyService : IProxyService
    {

        private string jcURL = "http://api.jcdecaux.com/vls/v1";
        private string jcApiKey = "abcf0a6d14f7397a211046296c73cf0d8c4521be";
        private static List<Contract> contracts = new List<Contract>();
        private static DateTime contractsValidTime = DateTime.Now;
        private static Dictionary<string, DateTime> cacheStations = new Dictionary<string, DateTime>();


        //city name to lowercase

        public List<Contract> GetContracts()
        {
            if (contractsValidTime <= DateTime.Now)
            {
                contracts = GetContractsFromAPI();
                contractsValidTime = DateTime.Now.AddDays(1);
            }
           
            
            foreach (Contract c in contracts)
            {
                if (cacheStations.ContainsKey(c.name))
                {
                    if (cacheStations[c.name] <= DateTime.Now)
                    {
                       
                        try
                        {
                            c.stations = GetStationsFromAPI(c.name);
                            cacheStations[c.name] = DateTime.Now.AddMinutes(1);
                        }
                        catch (Exception) { }
                    }
                }
                else
                {
                    c.stations = GetStationsFromAPI(c.name);
                    cacheStations[c.name] = DateTime.Now.AddMinutes(1);
                }
               
            }

            return new List<Contract>(contracts);
        }





        private List<Contract> GetContractsFromAPI()
        {

            string urlQuery = String.Concat(jcURL, "/contracts?apiKey=", jcApiKey);
            string res = ApiCall(urlQuery).Result;
            return JsonSerializer.Deserialize<List<Contract>>(res);

        }

        private List<Station> GetStationsFromAPI(string contractName)
        {
            string urlQuery = String.Concat(jcURL, "/stations?contract=", contractName, "&apiKey=", jcApiKey);
            string res = ApiCall(urlQuery).Result;
            return JsonSerializer.Deserialize<List<Station>>(res);
        }

        private async Task<string> ApiCall(string urlQuery)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(urlQuery);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }



        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

       
    }
}
