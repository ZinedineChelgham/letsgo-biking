using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace proxy_server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IProxyService
    {
        [OperationContract]
        List<Contract> GetContracts();


        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "proxy_server.ContractType".
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

    
    /******************************************************************************************************************************/
    [DataContract]
    public class Contract
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public List<Station> stations { get; set; }

        [DataMember]
        public Position position { get; set; }

    }
    public class Station
    {
        [DataMember]
        public string contractName { get; set; }
        [DataMember]
        public int number { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        
        public Position position { get; set; }
        [DataMember]
        public Stands totalStands { get; set; }

        [DataMember]
        public string address { get; set; }


    }
    public class Position
    {
        [DataMember]
        public double latitude { get; set; }
        [DataMember]
        public double longitude { get; set; }
    }

    public class Avaliability
    {
        [DataMember]
        public int bikes { get; set; }
        [DataMember]
        public int stands { get; set; }
    }
    public class Stands

    {

        [DataMember]
        public Avaliability availabilities { get; set; }
        [DataMember]
        public int capacity { get; set; }
    }
    /******************************************************************************************************************************/

}
