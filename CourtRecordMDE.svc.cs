/*
	'=======================================================================
	'   Author(s):      
	'   Module/Form:    
	'   Created Date:   
	'   Description:    
	'
	'   Modification History:
	'=======================================================================
	'   Author(s)       Date        Control/Procedure       Change
	'=======================================================================
    =======================================================================
	*/

using Oasis.LegalXml.CourtFiling.v40.WebServiceMessagingProfile;
using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using ecf=Oasis.LegalXml.CourtFiling.v40.Ecf;
using nc = Niem.NiemCore.v20 ;
using aoc = Arizona.Courts.Extensions.v20;


namespace Arizona.Courts.Services.v20
{
    [ServiceBehavior(Name = "CourtRecordMDEService", Namespace = "urn:oasis:names:tc:legalxml-courtfiling:wsdl:WebServiceMessagingProfile-Definitions-4.0"), AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CourtRecordMDE : ICourtRecordMDE
    {
        
        public RecordFilingResponse RecordFiling(RecordFilingRequest recordFilingRequest)
        {
            RecordFilingResponse response = null;
         
            return response;
        }


        public GetCaseResponse GetCase(GetCaseRequest getCaseRequest)
        {
            GetCaseResponse response = new GetCaseResponse
            {
                CaseResponseMessage = new Oasis.LegalXml.CourtFiling.v40.CaseResponse.CaseResponseMessageType
                {
                    SendingMDELocationID = new  nc.IdentificationType("http:/courts.az.gov/aoc/efiling/CRMDE") ,
                    SendingMDEProfileCode =  nc.Constants.ECF4_WEBSERVICES_SIP_CODE ,
                    CaseCourt = getCaseRequest != null && getCaseRequest.CaseQueryMessage != null ? getCaseRequest.CaseQueryMessage.CaseCourt : null

                }
            };
            try
            {
                response.CaseResponseMessage = new Oasis.LegalXml.CourtFiling.v40.CaseResponse.CaseResponseMessageType
                {
                    Case = SampleCivilCases.AZCivilCase
                };
            }
            catch (Exception ex)
            {
                throw new FaultException<aoc.OperationExceptionType>
                    (
                        new aoc.OperationExceptionType {  Operation = "GetCase",  ExceptionDetail = ex.Message },
                        new FaultReason(ex.Message),
                        new FaultCode("OTHER")
                    );

            }

            return response;

        }

        public GetCaseListResponse GetCaseList(GetCaseListRequest getCaseListRequest)
        {
            GetCaseListResponse response = null;

            response = new GetCaseListResponse { CaseListResponseMessage = new  Oasis.LegalXml.CourtFiling.v40.CaseListResponse.CaseListResponseMessageType()};
            try
            {

                response.CaseListResponseMessage.Error = ecf.EcfHelper.QuerySuccessfull();

            }
            catch (Exception ex)
            {
                throw new FaultException<aoc.OperationExceptionType>
                    (
                        new aoc.OperationExceptionType { Operation = "GetCase", ExceptionDetail = ex.Message },
                        new FaultReason(ex.Message),
                        new FaultCode("OTHER")
                    );

            }
            return response;
        }


        public GetServiceInformationResponse GetServiceInformation(GetServiceInformationRequest getServiceInformationRequest)
        {
            GetServiceInformationResponse response = null;

            response = new GetServiceInformationResponse {  ServiceInformationResponseMessage = new  Oasis.LegalXml.CourtFiling.v40.ServiceResponse.ServiceInformationResponseMessageType() };
            try
            {

                response.ServiceInformationResponseMessage.Error = ecf.EcfHelper.QuerySuccessfull();
            }
            catch (Exception ex)
            {
                throw new FaultException<aoc.OperationExceptionType>
                    (
                        new aoc.OperationExceptionType { Operation = "GetCase", ExceptionDetail = ex.Message },
                        new FaultReason(ex.Message),
                        new FaultCode("OTHER")
                    );

            }
            return response;
        }


        public GetDocumentResponse GetDocument(GetDocumentRequest getDocumentRequest)
        {
            GetDocumentResponse response = new GetDocumentResponse
            {
                DocumentResponseMessage = new  Oasis.LegalXml.CourtFiling.v40.DocumentResponse.DocumentResponseMessageType
                {
                    SendingMDELocationID = new nc.IdentificationType("http:/courts.az.gov/aoc/efiling/CRMDE"),
                    SendingMDEProfileCode = nc.Constants.ECF4_WEBSERVICES_SIP_CODE,
                    CaseCourt = getDocumentRequest != null && getDocumentRequest.DocumentQueryMessage != null ? getDocumentRequest.DocumentQueryMessage.CaseCourt : null

                }
            };
            try
            {

                response.DocumentResponseMessage.Document = SampleDocuments.AZDocument;
                response.DocumentResponseMessage.Error = ecf.EcfHelper.QuerySuccessfull();

            }
            catch (Exception ex)
            {
                throw new FaultException<aoc.OperationExceptionType>
                    (
                        new aoc.OperationExceptionType { Operation = "GetDocument", ExceptionDetail = ex.Message },
                        new FaultReason(ex.Message),
                        new FaultCode("OTHER")
                    );

            }
            return response;
        }

    }
}
