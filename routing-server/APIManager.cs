using RoutingServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;
using routing_server.ProxyService;

namespace routing_server
{
    public class APIManager
    {
        private static string jcdURL = "https://api.jcdecaux.com/vls/v3/";
        
        private static string jcdAPI = "abcf0a6d14f7397a211046296c73cf0d8c4521be";

        private static string openStreetURL = "https://api.openrouteservice.org/";
        private static string openStreetAPI = "5b3ce3597851110001cf6248cf4d9e82e1274bf48065c32320dde419";

        public static async Task<string> JCDCallApi(string url="", string query="")
        {

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(jcdURL + url + "?apiKey=" + jcdAPI + "&" + query);
            //response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public static async Task<string> openStreetCallApi(string url, string query)
        {

            HttpClient client = new HttpClient();
            string s = openStreetURL + url + "?api_key=" + openStreetAPI + "&" + query;
            Console.WriteLine(s);
            HttpResponseMessage response = await client.GetAsync(openStreetURL + url + "?api_key=" + openStreetAPI + "&" + query);
            //response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<double[]> getCoordinatesFromAdress(string adress)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(openStreetURL + "geocode/search?api_key=" + openStreetAPI + "&text=" + adress);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            //deserialize the response body and convert it to a list of double
            List<double> coordinates = JsonSerializer.Deserialize<List<double>>(responseBody);
            return JsonSerializer.Deserialize <double[]>(responseBody); 


            
        }
       
        /*********************************************************************************** API CALLS THAT WE WILL NEED**********************************************************/
        public double[] getCoordsFromAdress(string adress)
        {
            try
            {
                string response = openStreetCallApi("geocode/search", "text=" + adress).Result;
                JsonElement jsonCoords = JsonDocument.Parse(response).RootElement.GetProperty("features");
                return JsonSerializer.Deserialize<List<Feature>>(jsonCoords)[0].geometry.coordinates;
            }
            
            catch (Exception e)
            {
                return null;
            }
}
        
        public List<Feature> getFeaturesFromAdress(string adress)
        {
            string response = openStreetCallApi("geocode/search", "text=" + adress).Result;
            JsonElement jsonCoords = JsonDocument.Parse(response).RootElement.GetProperty("features");
            return JsonSerializer.Deserialize<List<Feature>>(jsonCoords);
        }
 
 
        public List<Station> getAllStationsOfContract(string contract)
        {
            string response = JCDCallApi("stations", "contract=" + contract).Result;
            return JsonSerializer.Deserialize<List<Station>>(response);
        }




        public List<Segment> getSegmentsItinerary(string start, string end, string transportWay)
        {
            {
                try
                {
                    string response = openStreetCallApi("v2/directions/" + transportWay, "start=" + start + "&end=" + end).Result;
                    JsonElement jsonSegments = JsonDocument.Parse(response).RootElement.GetProperty("features")[0].GetProperty("properties").GetProperty("segments");

                    return JsonSerializer.Deserialize<List<Segment>>(jsonSegments);

                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }

   

        public List<string> getAllInstructions(string start, string end, string transportWay)
        {
            try
            {
                string response = openStreetCallApi("v2/directions/" + transportWay, "start=" + start + "&end=" + end).Result;
                JsonElement jsonCoords = JsonDocument.Parse(response).RootElement.GetProperty("features")[0].GetProperty("properties").GetProperty("segments")[0].GetProperty("steps");
                List<string> instructions = new List<string>();
                foreach (var item in JsonSerializer.Deserialize<List<Step>>(jsonCoords))
                {
                    instructions.Add(item.instruction);
                }
                return instructions;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public double calculateEstimatedDuration(string start, string end, string transportWay)
        {
            string response = openStreetCallApi("v2/directions/" + transportWay, "start=" + start + "&end=" + end).Result;
            JsonElement jsonCoords = JsonDocument.Parse(response).RootElement.GetProperty("features")[0].GetProperty("properties").GetProperty("summary").GetProperty("duration");
            return jsonCoords.GetDouble();
        }

        
        public string getCityFromAdress(string adr)
        {
            {
                string response = openStreetCallApi("geocode/search", "text=" + adr).Result;
                JsonElement jsonCoords = JsonDocument.Parse(response).RootElement.GetProperty("features")[0].GetProperty("properties").GetProperty("locality");
                return jsonCoords.GetString().ToLower();
            }
        }

           
        
        


























    }
}
