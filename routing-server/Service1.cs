
using routing_server;
using routing_server.ProxyService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace RoutingServer
{

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {


        public List<string> getRoute(string origin, string destination, bool activeMq)
        {
            List<string> route = GetRouteResult(origin, destination);

             if (activeMq)
             {
                 List<string> response = new List<string>();
                 MqController controller = new MqController("tcp://localhost:61616", new Service1());
                 controller.sendMessageToQueue(route);
                 response.Add("Using activeMQ ");
                 return response;
             }
             else
             {
                 return route;
             }

        }




        private List<string> GetRouteResult(string origin, string destination)
        {
            List<string> route = new List<string>();
            APIManager apiManager = new APIManager();
            JCDManager jcdManager = new JCDManager();
            double[] originCoords = apiManager.getCoordsFromAdress(origin);
            double[] destinationCoords = apiManager.getCoordsFromAdress(destination);
            if (originCoords != null && destinationCoords != null)
            {
                Station closestStationToOrigin = jcdManager.getClosestStationWithAvailableBikes(originCoords);
                Station closestStationToDestination = jcdManager.getClosestStationWithAvailableStands(destinationCoords);
                string coordsOriginConnected = ComputeData.coordsToStringSplitWithComma(originCoords);
                string coordsDesConnected = ComputeData.coordsToStringSplitWithComma(destinationCoords);

                double[] closestStationCoordToOrigin = ComputeData.positionToArray(closestStationToOrigin.position);
                    
                double[] closestStationCoordToDestination = ComputeData.positionToArray(closestStationToDestination.position);

             


                List<Segment> itineraryOriginToStation = apiManager.getSegmentsItinerary(coordsOriginConnected,
                    ComputeData.coordsToStringSplitWithComma(closestStationCoordToOrigin), "foot-walking");

                List<Segment> itineraryStationToDestination = apiManager.getSegmentsItinerary(coordsDesConnected,
                    ComputeData.coordsToStringSplitWithComma(closestStationCoordToDestination), "foot-walking");

                List<Segment> itineraryOriginToDestinationByFoot = apiManager.getSegmentsItinerary(coordsOriginConnected,
                    coordsDesConnected, "foot-walking");
                List<Segment> itineraryOriginToDestinationByBike = apiManager.getSegmentsItinerary(coordsOriginConnected,
                    coordsDesConnected, "cycling-regular");



                if (!ComputeData.isAllContractsContainCity(apiManager.getCityFromAdress(origin)) || !ComputeData.isAllContractsContainCity(apiManager.getCityFromAdress(destination)))
                {
                    route.Add("Sorry, we don't have any station in this city, the closest station from your location is in " + jcdManager.getCityOfStation(closestStationToOrigin) + " at : " + closestStationToOrigin.address);
                    return route;
                }
                
                if (ComputeData.isBikeWorthIt(itineraryOriginToStation, itineraryStationToDestination, itineraryOriginToDestinationByBike, itineraryOriginToDestinationByFoot))

                {



                    route.Add("\nBike is worth to use , here we are giving you the itinerary to get the bike \n");
                    route.AddRange(apiManager.getAllInstructions(coordsOriginConnected, ComputeData.coordsToStringSplitWithComma(ComputeData.positionToArray(closestStationToOrigin.position)), "foot-walking"));
                    route.Add("\n**********************Now head to the nearest station to your destination to drop the bike******************\n");

                    route.AddRange(apiManager.getAllInstructions(ComputeData.coordsToStringSplitWithComma(ComputeData.positionToArray(closestStationToOrigin.position)),
                    ComputeData.coordsToStringSplitWithComma(ComputeData.positionToArray(closestStationToDestination.position)), "cycling-regular"));
                    route.Add("\n**********************Now head to your destination*******************************************\n");


                    route.AddRange(apiManager.getAllInstructions(ComputeData.coordsToStringSplitWithComma(ComputeData.positionToArray(closestStationToDestination.position)),
                        ComputeData.coordsToStringSplitWithComma(destinationCoords), "foot-walking"));



                    return route;



                }
                else
                {
                    route.Add("\nBike is not worth to use , here we are giving you the itinerary to get to your destination by foot\n");
                    route.AddRange(apiManager.getAllInstructions(coordsOriginConnected, coordsDesConnected, "foot-walking"));

                    return route;
                }



            }
            else
            {
                route.Add("No itinerary found, please try again...\n");
                return route;
            }


        }


    }





}

