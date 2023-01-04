using routing_server.ProxyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using routing_server.ProxyService;


namespace routing_server
{
    public  class JCDManager
    {
        public string response;
        public string response2;
        public List<Station> everystations = new List<Station>();
        public List<Contract> everycontracts = new List<Contract>();

        
        

        
        public JCDManager()
        {

            
            BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
            basicHttpBinding.MaxReceivedMessageSize = 99999900;
            EndpointAddress jCDProxyEndpoint = new EndpointAddress("http://localhost:8091/proxy_server/ProxyService/");
            ProxyServiceClient proxy = new ProxyServiceClient(basicHttpBinding, jCDProxyEndpoint);

            everycontracts = proxy.GetContracts().ToList();
            string response2 = APIManager.JCDCallApi("stations").Result;
            everystations = JsonSerializer.Deserialize<List<Station>>(response2);


        }

        // True : lorsqu'on veut predre un vélo (we seek for a station full with bikes)
        // False : lorsqu'on veut restituer un vélo (we seek for a station full with stands)
        public Station getClosestStation(Feature feature, List<Station> stations, bool origin)
        {
            Station closestStation = null;
            double closestDistanceFromStation = 0;
            foreach (Station station in everystations)
            {
                double distanceCalculated = ComputeData.Distance(feature.geometry.coordinates, ComputeData.positionToArray(station.position));
                int standsOrBikesAvailability = origin ? station.totalStands.availabilities.bikes : station.totalStands.availabilities.stands;
                if (closestStation == null || (distanceCalculated <= closestDistanceFromStation && standsOrBikesAvailability > 0))
                {
                    closestStation = station;
                    closestDistanceFromStation = distanceCalculated;
                }
            }
            return closestStation;
        }

        public Station getClosestStationFromCoords(double[] coords)
        {
            {
                Station closestStation = null;
                double closestDistanceFromStation = 0;
                foreach (Station station in everystations)
                {
                    double distanceCalculated = ComputeData.Distance(coords, ComputeData.positionToArray(station.position));
                    if (closestStation == null || distanceCalculated <= closestDistanceFromStation)
                    {
                        closestStation = station;
                        closestDistanceFromStation = distanceCalculated;
                    }
                }
                return closestStation;
            }
        }


        //this method is needed when the client wants to use a bike
        public Station getClosestStationWithAvailableBikes(double[] coords)
        {
            {
                {
                    try
                    {
                        Station closestStation = null;
                        double closestDistanceFromStation = 0;
                        foreach (Station station in everystations)
                        {
                            double distanceCalculated = ComputeData.Distance(coords, ComputeData.positionToArray(station.position));
                            if (closestStation == null || (distanceCalculated <= closestDistanceFromStation && station.totalStands.availabilities.bikes > 0))
                            {
                                closestStation = station;
                                closestDistanceFromStation = distanceCalculated;
                            }
                        }
                        return closestStation;
                    }

                    catch (Exception e)
                    {
                        return null;
                    }
                }
            }
        }
        
        //this method is needed when the client wants to return a bike
        public Station getClosestStationWithAvailableStands(double[] coords)
        {
            {
                {
                    try
                    {
                        Station closestStation = null;
                        double closestDistanceFromStation = 0;
                        foreach (Station station in everystations)
                        {
                            double distanceCalculated = ComputeData.Distance(coords, ComputeData.positionToArray(station.position));
                            if (closestStation == null || (distanceCalculated <= closestDistanceFromStation && station.totalStands.availabilities.stands > 0))
                            {
                                closestStation = station;
                                closestDistanceFromStation = distanceCalculated;
                            }
                        }
                        return closestStation;
                    }

                    catch (Exception e)
                    {
                        return null;
                    }
                }
            }
        }

        //given a station give the city if null it means that the city dont have any station
        public string getCityOfStation(Station station)
        {
            APIManager apiManager = new APIManager();
            foreach (Contract contract in everycontracts)
            {
                foreach (Station station1 in apiManager.getAllStationsOfContract(contract.name))
                {
                    if (station1.name == station.name)
                    {
                        return contract.name;
                    }
                }
            }
            return null;
        }

       
        public bool isLocationAndDestinationInAContract(string location ,string destination)
        {
            {
                bool locationInAContract = false;
                bool destinationInAContract = false;
                foreach (Contract contract in everycontracts)
                {
                    if (contract.name == location)
                    {
                        locationInAContract = true;
                    }
                    if (contract.name == destination)
                    {
                        destinationInAContract = true;
                    }
                }
                return locationInAContract && destinationInAContract;
            }
        }


  


    }

    




}

