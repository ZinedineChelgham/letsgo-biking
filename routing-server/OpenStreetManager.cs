using RoutingServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace routing_server
{

    public class Feature
    {
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
    }
  
    
    public class Geometry
    {
        public string type { get; set; }
        public double[] coordinates { get; set; }
        
    }
    public class Properties
    {
        public Summary summary { get; set; }
        public List<Segment> segments { get; set; }
       

    }
    public class Summary
    {
        public double distance { get; set; }
        public double duration { get; set; }

    }
    public class Segment

    {
        public double distance { get; set; }
        public double duration { get; set; }
        public List<Step> steps { get; set; }
    }
    public class Step
    {
        public double distance { get; set; }
        public double duration { get; set; }
        public string instruction { get; set; }

    }
    public class Coordinates
    {
        
        public double[] coordinates { get; set; }
    
}
}
