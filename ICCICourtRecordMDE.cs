/*
	'=======================================================================
	'   Author(s):     
	'   Module/Form:    ICourtRecordMDE.cs
	'   Created Date:   
	'   Description:    
	'
	'   Modification History:
	'=======================================================================
	'   Author(s)       Date        Control/Procedure       Change
	'=======================================================================
	'=======================================================================
	*/

using System.ServiceModel;
using Oasis.LegalXml.CourtFiling.v40.WebServiceMessagingProfile;


namespace Oasis.LegalXml.CourtFiling.v40.WebServiceMessagingProfile
{
    [ServiceContract(Namespace = "http://schema.azcourts.az.gov/aoc/efiling/ecf/exchange/services/2.0/CRMDEServic", Name = "CourtRecordMDEPort")]
    [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal )]
    public interface ICourtRecordMDE
    {
        [OperationContract() ]
        [FaultContract(typeof(Arizona.Courts.Extensions.v20.OperationExceptionType))]
        RecordFilingResponse RecordFiling(RecordFilingRequest recordFilingRequest);

        [OperationContract( Action="http://schema.azcourts.az.gov/aoc/efiling/ecf/exchange/services/2.0/CRMDEService/GetCase" )]
        [FaultContract(typeof(Arizona.Courts.Extensions.v20.OperationExceptionType))]
        GetCaseResponse GetCase(GetCaseRequest getCaseRequest);

        [OperationContract()]
        [FaultContract(typeof(Arizona.Courts.Extensions.v20.OperationExceptionType))]
        GetCaseListResponse GetCaseList(GetCaseListRequest getCaseListRequest);

        [OperationContract()]
        [FaultContract(typeof(Arizona.Courts.Extensions.v20.OperationExceptionType))]
        GetServiceInformationResponse GetServiceInformation(GetServiceInformationRequest getServiceInformationRequest);

        [OperationContract(Action = "http://schema.azcourts.az.gov/aoc/efiling/ecf/exchange/services/2.0/CRMDEService/GetDocument")]
        [FaultContract(typeof(Arizona.Courts.Extensions.v20.OperationExceptionType))]
        GetDocumentResponse GetDocument(GetDocumentRequest getDocumentRequest);

    }
}
