using routing_server.ProxyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace routing_server
{
    public class ComputeData
    {
            
        //calculate distance between two cordinates
        public static double Distance(double[] coordinates, double[] position)
        {
            double x = coordinates[0] - position[0];
            double y = coordinates[1] - position[1];
            return Math.Sqrt(x * x + y * y);
        }

        public static double[] positionToArray(Position position){
            double[] positionArray = new double[2];
            positionArray[0] = position.longitude;
            positionArray[1] = position.latitude;    
            return positionArray;
        }

        public static string replaceCommaWithDot(string str)
        {
            return  str.Replace(',', '.');
        }
        public static bool allContractsContains(string origin, string destination)
        {
            JCDManager jcd = new JCDManager();
            return jcd.everycontracts.Any(x => x.name == origin) && jcd.everycontracts.Any(x => x.name == destination);
        }


        public static bool isBikeWorthIt(List<Segment> originToStation, List<Segment> destinationToEndStation, List<Segment> originToDestinationByBike, List<Segment> originToDestinationByFoot)
        {

            double sumDuration = originToStation[0].duration + destinationToEndStation[0].duration + originToDestinationByBike[0].duration;
          

            if (sumDuration < originToDestinationByFoot[0].duration)
            {
                return true;
            }
            return false; //if the distance is too long (which is not the case usually) this is not optimal

        }

        public static string coordsToStringSplitWithComma(double[] coords)
        {
            if(coords != null)
            {
                return ComputeData.replaceCommaWithDot(coords[0].ToString()) + "," + ComputeData.replaceCommaWithDot(coords[1].ToString());

            }
            return null;
        }

        public static bool isAllContractsContainCity(string city)
        {

            JCDManager jcd = new JCDManager();
            return jcd.everycontracts.Any(x => x.name == city);
        }



    }
}
