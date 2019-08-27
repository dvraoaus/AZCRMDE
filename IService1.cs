using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

using System.Text;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here

        // CODEGEN: Generating message contract since the operation notifyFilingReviewComplete is neither RPC nor document wrapped.
        //[System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
        //[System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        // notifyFilingReviewCompleteResponse notifyFilingReviewComplete(notifyFilingReviewCompleteRequest request);

        // [System.ServiceModel.OperationContract(Action = "http://www.clerkofcourt.maricopa.gov/GetDocument", ReplyAction = "*")]
        // [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [OperationContract]
        System.Xml.XmlNode GetDocument(System.Xml.XmlNode DocumentRequestXml);

    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
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
}
