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
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Hosting;
using System.Xml.Serialization;
using amc = Arizona.Courts.ExChanges.v20;
using aoc = Arizona.Courts.Extensions.v20;
using caseResponse = Oasis.LegalXml.CourtFiling.v40.CaseResponse;
using ecf = Oasis.LegalXml.CourtFiling.v40.Ecf;
using nc = Niem.NiemCore.v20;
using wmp = Oasis.LegalXml.CourtFiling.v40.WebServiceMessagingProfile;
using azs = Arizona.Courts.Services.v20;


namespace Arizona.Courts.Services.v20
{
    [ServiceBehavior(Name = "CourtRecordMDEService", Namespace = "urn:oasis:names:tc:legalxml-courtfiling:wsdl:WebServiceMessagingProfile-Definitions-4.0"), AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CourtRecordMDE : azs.ICourtRecordMDE
    {
        
        public wmp.RecordFilingResponse RecordFiling(wmp.RecordFilingRequest recordFilingRequest)
        {
            wmp.RecordFilingResponse response = null;
         
            return response;
        }


        public wmp.GetCaseResponse GetCase(wmp.GetCaseRequest getCaseRequest)
        {
            wmp.GetCaseResponse response = new wmp.GetCaseResponse();
            try
            {
                string caseTrackingId = getCaseRequest != null && getCaseRequest.CaseQueryMessage != null && getCaseRequest.CaseQueryMessage.CaseTrackingID != null && !string.IsNullOrEmpty(getCaseRequest.CaseQueryMessage.CaseTrackingID.Value) ? getCaseRequest.CaseQueryMessage.CaseTrackingID.Value : string.Empty;
                aoc.CivilCaseType civilCase = this.GetCase(caseTrackingId);
                if (civilCase != null)
                {
                    response = new wmp.GetCaseResponse
                    (
                        getCaseResponse: new amc.GetCaseResponseType
                        {
                            CaseResponseMessage = new caseResponse.CaseResponseMessageType
                            {
                                Case = civilCase,
                                Error = ecf.EcfHelper.OperationSuccessfull(),
                                CaseCourt = getCaseRequest != null && getCaseRequest.CaseQueryMessage != null && getCaseRequest.CaseQueryMessage.CaseCourt != null ? getCaseRequest.CaseQueryMessage.CaseCourt : SampleCourts.CaseCourt,
                                SendingMDELocationID = new nc.IdentificationType("http://courts.az.gov/eFiling/MockCRMDE"),
                                SendingMDEProfileCode = "urn:oasis:names:tc:legalxml-courtfiling:schema:xsd:WebServicesProfile-2.0"
                            }

                        }
                    );
                }
                else if ( civilCase == null)
                {
                    string errorCode = "-10";
                    string errorText = string.Empty  ;
                    if ( !string.IsNullOrEmpty(caseTrackingId) && caseTrackingId.EndsWith(amc.PolicyConstants.GETCASE_ERRORCODE_CAN_NOT_ACCESS_CCI))
                    {
                        errorCode = amc.PolicyConstants.GETCASE_ERRORCODE_CAN_NOT_ACCESS_CCI ;
                        errorText = "Unable to access CCI" ;
                    }
                    else if ( !string.IsNullOrEmpty(caseTrackingId) && caseTrackingId.EndsWith(amc.PolicyConstants.GETCASE_ERRORCODE_CAN_NOT_FIND_CASE_IN_CCI.Substring(1)))
                    {
                        errorCode = amc.PolicyConstants.GETCASE_ERRORCODE_CAN_NOT_FIND_CASE_IN_CCI ;
                        errorText = string.Format("Case #  {0} not found.!!!!", caseTrackingId);
                    }
                    else if (!string.IsNullOrEmpty(caseTrackingId) && caseTrackingId.EndsWith(amc.PolicyConstants.GETCASE_ERRORCODE_SEALED_CASE.Substring(1)))
                    {
                        errorCode = amc.PolicyConstants.GETCASE_ERRORCODE_SEALED_CASE;
                        errorText = string.Format("Case #  {0} is a sealed case.!!!!", caseTrackingId);
                    }
                    else if (!string.IsNullOrEmpty(caseTrackingId) && caseTrackingId.EndsWith(amc.PolicyConstants.GETCASE_ERRORCODE_RESTRICTED_CASE.Substring(1)))
                    {
                        errorCode = amc.PolicyConstants.GETCASE_ERRORCODE_RESTRICTED_CASE;
                        errorText = string.Format("Case #  {0} is restricted.!!!!", caseTrackingId);
                    }
                    else if (!string.IsNullOrEmpty(caseTrackingId) && caseTrackingId.EndsWith(amc.PolicyConstants.GETCASE_ERRORCODE_CONSOLIDATED_CASE.Substring(1)))
                    {
                        errorCode = amc.PolicyConstants.GETCASE_ERRORCODE_CONSOLIDATED_CASE;
                        errorText = string.Format("Case #  {0} is consolidated.!!!!", caseTrackingId);
                    }
                    else if (!string.IsNullOrEmpty(caseTrackingId) && caseTrackingId.EndsWith(amc.PolicyConstants.GETCASE_ERRORCODE_TRANSFERRED_CASE.Substring(1)))
                    {
                        errorCode = amc.PolicyConstants.GETCASE_ERRORCODE_TRANSFERRED_CASE;
                        errorText = string.Format("Case #  {0} is transferred.!!!!", caseTrackingId);
                    }
                    else if (!string.IsNullOrEmpty(caseTrackingId) && caseTrackingId.EndsWith(amc.PolicyConstants.GETCASE_ERRORCODE_XML_BAD_NARKUP.Substring(1)))
                    {
                        errorCode = amc.PolicyConstants.GETCASE_ERRORCODE_XML_BAD_NARKUP;
                        errorText = string.Format("XML Bad Morkup.!!!!", caseTrackingId);
                    }
                    else if (!string.IsNullOrEmpty(caseTrackingId) && caseTrackingId.EndsWith(amc.PolicyConstants.GETCASE_ERRORCODE_XML_NOT_FORMED.Substring(1)))
                    {
                        errorCode = amc.PolicyConstants.GETCASE_ERRORCODE_XML_NOT_FORMED;
                        errorText = string.Format("XML Not Well formed.!!!!", caseTrackingId);
                    }
                    
                    response = new wmp.GetCaseResponse
                    (
                        getCaseResponse: new amc.GetCaseResponseType
                        {
                            CaseResponseMessage = new caseResponse.CaseResponseMessageType
                            {
                                Case = null,
                                Error = ecf.EcfHelper.ErrorList(errorCode ,  errorText ),
                                CaseCourt = getCaseRequest != null && getCaseRequest.CaseQueryMessage != null && getCaseRequest.CaseQueryMessage.CaseCourt != null ? getCaseRequest.CaseQueryMessage.CaseCourt : SampleCourts.CaseCourt,
                                SendingMDELocationID = new nc.IdentificationType("http://courts.az.gov/eFiling/MockCRMDE"),
                                SendingMDEProfileCode = "urn:oasis:names:tc:legalxml-courtfiling:schema:xsd:WebServicesProfile-2.0"
                            }

                        }
                    );

                }

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

        private aoc.CivilCaseType GetCase(string caseTrackingId)
        {
            aoc.CivilCaseType civilCase = null;
            if (!string.IsNullOrEmpty(caseTrackingId))
            {

                string caseXmlFile = GetApplicationPath() + @"\SampleCases\" + caseTrackingId + ".xml";
                if (File.Exists(caseXmlFile))
                {
                    using (var fs = new FileStream(caseXmlFile, FileMode.Open , FileAccess.Read ))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(aoc.CivilCaseType));
                        civilCase = serializer.Deserialize(fs) as aoc.CivilCaseType;
                    }

                }
            }
            return civilCase;
        }

        private string GetApplicationPath()
        {
            string applicationPath = String.Empty;
            try
            {
                if (HttpContext.Current != null)
                {
                    applicationPath = HttpContext.Current.Server.MapPath(".");
                }
                else
                {
                    if (OperationContext.Current != null)
                    {
                        applicationPath = HostingEnvironment.ApplicationPhysicalPath;
                    }
                    else
                    {
                        string applicationCodeBase = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
                        applicationPath = Path.GetDirectoryName(applicationCodeBase.Replace(@"file:///", ""));

                    }
                }

            }
            catch
            {
            }
            return applicationPath;
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
