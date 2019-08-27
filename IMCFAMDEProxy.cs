/*
	'=======================================================================
	'   Author(s):     
	'   Module/Form:    IMCFAMDEProxy.cs
	'   Created Date:   
	'   Description:    Maricoa FAMDE Proxy for handling call back from MC FRMDE
	'
	'   Modification History:
	'=======================================================================
	'   Author(s)       Date        Control/Procedure       Change
	'=======================================================================
	'=======================================================================
	*/

using System.ServiceModel;
using ecf31 = Oasis.LegalXml.v31.CourtFiling;

namespace Arizona.Courts.Services.v20
{


    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://com.wiznet/filingassemblymde/types", ConfigurationName = "Arizona.Courts.Services.v20.IMCFAMDEProxy")]
    public interface IMCFAMDEProxy
    {

        // CODEGEN: Generating message contract since the operation notifyFilingReviewComplete is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
       notifyFilingReviewCompleteResponse notifyFilingReviewComplete(notifyFilingReviewCompleteRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.clerkofcourt.maricopa.gov/GetDocument", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        System.Xml.XmlNode GetDocument(System.Xml.XmlNode DocumentRequestXml);

    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class notifyFilingReviewCompleteRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://com.wiznet/filingassemblymde/types", Order = 0)]
        public object notifyFilingReviewComplete;

        public notifyFilingReviewCompleteRequest()
        {
        }

        public notifyFilingReviewCompleteRequest(object notifyFilingReviewComplete)
        {
            this.notifyFilingReviewComplete = notifyFilingReviewComplete;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class notifyFilingReviewCompleteResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://com.wiznet/filingassemblymde/types", Order = 0)]
        public object notifyFilingReviewCompleteReturn;

        public notifyFilingReviewCompleteResponse()
        {
        }

        public notifyFilingReviewCompleteResponse(object notifyFilingReviewCompleteReturn)
        {
            this.notifyFilingReviewCompleteReturn = notifyFilingReviewCompleteReturn;
        }
    }

}