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
using wmp = Oasis.LegalXml.CourtFiling.v40.WebServiceMessagingProfile;


namespace Arizona.Courts.Services.v20
{
    [ServiceContract(Namespace = "http://schema.azcourts.az.gov/aoc/efiling/ecf/exchange/services/2.0/CRMDEServic", Name = "CourtRecordMDEPort")]
    [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
    public interface ICourtRecordMDE
    {
        [OperationContract()]
        wmp.RecordFilingResponse RecordFiling(wmp.RecordFilingRequest recordFilingRequest);

        [OperationContract(Action = "http://schema.azcourts.az.gov/aoc/efiling/ecf/exchange/services/2.0/CRMDEService/GetCase")]
        wmp.GetCaseResponse GetCase(wmp.GetCaseRequest getCaseRequest);

        [OperationContract()]
        wmp.GetCaseListResponse GetCaseList(wmp.GetCaseListRequest getCaseListRequest);

        [OperationContract()]
        wmp.GetServiceInformationResponse GetServiceInformation(wmp.GetServiceInformationRequest getServiceInformationRequest);

        [OperationContract(Action = "http://schema.azcourts.az.gov/aoc/efiling/ecf/exchange/services/2.0/CRMDEService/GetDocument")]
        wmp.GetDocumentResponse GetDocument(wmp.GetDocumentRequest getDocumentRequest);

    }
}
