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

using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using ecf=Oasis.LegalXml.CourtFiling.v40.Ecf;
using nc = Niem.NiemCore.v20 ;
using wmp = Oasis.LegalXml.CourtFiling.v40.WebServiceMessagingProfile;
using aoc = Arizona.Courts.Extensions.v20;
using amc = Arizona.Courts.ExChanges.v20;
using caseResponse = Oasis.LegalXml.CourtFiling.v40.CaseResponse;



namespace Arizona.Courts.Services.v20
{
    [ServiceBehavior(Name = "CourtRecordMDEService", Namespace = "urn:oasis:names:tc:legalxml-courtfiling:wsdl:WebServiceMessagingProfile-Definitions-4.0"), AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CourtRecordMDE : wmp.ICourtRecordMDE
    {
        
        public wmp.RecordFilingResponse RecordFiling(wmp.RecordFilingRequest recordFilingRequest)
        {
            wmp.RecordFilingResponse response = null;
         
            return response;
        }


        public wmp.GetCaseResponse GetCase(wmp.GetCaseRequest getCaseRequest)
        {
            wmp.GetCaseResponse response = new wmp.GetCaseResponse
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
                response.GetCaseResponseObject = new  amc.GetCaseResponseWrapperType
                {
                     GetCaseResponse = new amc.GetCaseResponseType
                     { 
                         CaseResponseMessage  = new  caseResponse.CaseResponseMessageType
                         {
                              Case = SampleCivilCases.AZCivilCase 
                         }
                     }
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

        public wmp.GetCaseListResponse GetCaseList(wmp.GetCaseListRequest getCaseListRequest)
        {
            wmp.GetCaseListResponse response = null;

            response = new wmp.GetCaseListResponse { CaseListResponseMessage = new  Oasis.LegalXml.CourtFiling.v40.CaseListResponse.CaseListResponseMessageType()};
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


        public wmp.GetServiceInformationResponse GetServiceInformation(wmp.GetServiceInformationRequest getServiceInformationRequest)
        {
            wmp.GetServiceInformationResponse response = null;

            response = new wmp.GetServiceInformationResponse {  ServiceInformationResponseMessage = new  Oasis.LegalXml.CourtFiling.v40.ServiceResponse.ServiceInformationResponseMessageType() };
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


        public wmp.GetDocumentResponse GetDocument(wmp.GetDocumentRequest getDocumentRequest)
        {
            wmp.GetDocumentResponse response = new wmp.GetDocumentResponse
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
