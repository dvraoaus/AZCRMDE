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
using amc20 = Arizona.Courts.ExChanges.v20;
using aoc20 = Arizona.Courts.Extensions.v20;
using amc21 = Arizona.Courts.ExChanges.v21;
using aoc21 = Arizona.Courts.Extensions.v21;

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
                bool use21Version = getCaseRequest != null && getCaseRequest.GetCaseRequestMessageObject != null && getCaseRequest.GetCaseRequestMessageObject is amc21.GetCaseRequestWrapperType;
                nc.CaseType civilCase = use21Version  ? 
                                        this.Get21Case(caseTrackingId) :
                                        this.Get20Case(caseTrackingId);
                if (civilCase != null)
                {
                    if (use21Version)
                    {
                        response = new wmp.GetCaseResponse
                        (
                            getCaseResponse: new amc21.GetCaseResponseType
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
                    else
                    {
                            response = new wmp.GetCaseResponse
                            (
                                getCaseResponse: new amc20.GetCaseResponseType
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
                }
                else if ( civilCase == null)
                {
                    string errorCode = "-10";
                    string errorText = string.Empty  ;
                    if ( !string.IsNullOrEmpty(caseTrackingId) && caseTrackingId.EndsWith(amc20.PolicyConstants.GETCASE_ERRORCODE_CAN_NOT_ACCESS_CCI))
                    {
                        errorCode = amc20.PolicyConstants.GETCASE_ERRORCODE_CAN_NOT_ACCESS_CCI ;
                        errorText = "Unable to access CCI" ;
                    }
                    else if ( !string.IsNullOrEmpty(caseTrackingId) && caseTrackingId.EndsWith(amc20.PolicyConstants.GETCASE_ERRORCODE_CAN_NOT_FIND_CASE_IN_CCI.Substring(1)))
                    {
                        errorCode = amc20.PolicyConstants.GETCASE_ERRORCODE_CAN_NOT_FIND_CASE_IN_CCI ;
                        errorText = string.Format("Case #  {0} not found.!!!!", caseTrackingId);
                    }
                    else if (!string.IsNullOrEmpty(caseTrackingId) && caseTrackingId.EndsWith(amc20.PolicyConstants.GETCASE_ERRORCODE_SEALED_CASE.Substring(1)))
                    {
                        errorCode = amc20.PolicyConstants.GETCASE_ERRORCODE_SEALED_CASE;
                        errorText = string.Format("Case #  {0} is a sealed case.!!!!", caseTrackingId);
                    }
                    else if (!string.IsNullOrEmpty(caseTrackingId) && caseTrackingId.EndsWith(amc20.PolicyConstants.GETCASE_ERRORCODE_RESTRICTED_CASE.Substring(1)))
                    {
                        errorCode = amc20.PolicyConstants.GETCASE_ERRORCODE_RESTRICTED_CASE;
                        errorText = string.Format("Case #  {0} is restricted.!!!!", caseTrackingId);
                    }
                    else if (!string.IsNullOrEmpty(caseTrackingId) && caseTrackingId.EndsWith(amc20.PolicyConstants.GETCASE_ERRORCODE_CONSOLIDATED_CASE.Substring(1)))
                    {
                        errorCode = amc20.PolicyConstants.GETCASE_ERRORCODE_CONSOLIDATED_CASE;
                        errorText = string.Format("Case #  {0} is consolidated.!!!!", caseTrackingId);
                    }
                    else if (!string.IsNullOrEmpty(caseTrackingId) && caseTrackingId.EndsWith(amc20.PolicyConstants.GETCASE_ERRORCODE_TRANSFERRED_CASE.Substring(1)))
                    {
                        errorCode = amc20.PolicyConstants.GETCASE_ERRORCODE_TRANSFERRED_CASE;
                        errorText = string.Format("Case #  {0} is transferred.!!!!", caseTrackingId);
                    }
                    else if (!string.IsNullOrEmpty(caseTrackingId) && caseTrackingId.EndsWith(amc20.PolicyConstants.GETCASE_ERRORCODE_XML_BAD_NARKUP.Substring(1)))
                    {
                        errorCode = amc20.PolicyConstants.GETCASE_ERRORCODE_XML_BAD_NARKUP;
                        errorText = string.Format("XML Bad Morkup.!!!!", caseTrackingId);
                    }
                    else if (!string.IsNullOrEmpty(caseTrackingId) && caseTrackingId.EndsWith(amc20.PolicyConstants.GETCASE_ERRORCODE_XML_NOT_FORMED.Substring(1)))
                    {
                        errorCode = amc20.PolicyConstants.GETCASE_ERRORCODE_XML_NOT_FORMED;
                        errorText = string.Format("XML Not Well formed.!!!!", caseTrackingId);
                    }
                    else if (!string.IsNullOrEmpty(caseTrackingId) && caseTrackingId.EndsWith(amc20.PolicyConstants.GETCASE_ERRORCODE_DELETED_CASE.Substring(1)))
                    {
                        errorCode = amc20.PolicyConstants.GETCASE_ERRORCODE_DELETED_CASE;
                        errorText = string.Format("Deleted");
                    }
                    else if (!string.IsNullOrEmpty(caseTrackingId) && caseTrackingId.EndsWith(amc20.PolicyConstants.GETCASE_ERRORCODE_CLOSED_CASE.Substring(1)))
                    {
                        errorCode = amc20.PolicyConstants.GETCASE_ERRORCODE_CLOSED_CASE;
                        errorText = string.Format("Closed");
                    }

                    response = new wmp.GetCaseResponse
                    (
                        getCaseResponse: new amc20.GetCaseResponseType
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
                throw new FaultException<aoc20.OperationExceptionType>
                    (
                        new aoc20.OperationExceptionType {  Operation = "GetCase",  ExceptionDetail = ex.Message },
                        new FaultReason(ex.Message),
                        new FaultCode("OTHER")
                    );

            }

            return response;

        }

        private nc.CaseType Get20Case(string caseTrackingId)
        {
            aoc20.CivilCaseType civilCase = null;
            if (!string.IsNullOrEmpty(caseTrackingId))
            {

                string caseXmlFile = GetApplicationPath() + @"\SampleCases\" + caseTrackingId + ".xml";
                if (File.Exists(caseXmlFile))
                {
                    using (var fs = new FileStream(caseXmlFile, FileMode.Open , FileAccess.Read ))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(aoc20.CivilCaseType));
                        civilCase = serializer.Deserialize(fs) as aoc20.CivilCaseType;
                    }

                }
            }
            return civilCase;
        }

        private nc.CaseType Get21Case(string caseTrackingId)
        {
            aoc21.CivilCaseType civilCase = null;
            if (!string.IsNullOrEmpty(caseTrackingId))
            {

                string caseXmlFile = GetApplicationPath() + @"\SampleCases\" + caseTrackingId + ".xml";
                if (File.Exists(caseXmlFile))
                {
                    using (var fs = new FileStream(caseXmlFile, FileMode.Open, FileAccess.Read))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(aoc21.CivilCaseType));
                        civilCase = serializer.Deserialize(fs) as aoc21.CivilCaseType;
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
                throw new FaultException<aoc20.OperationExceptionType>
                    (
                        new aoc20.OperationExceptionType { Operation = "GetCase", ExceptionDetail = ex.Message },
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
                throw new FaultException<aoc20.OperationExceptionType>
                    (
                        new aoc20.OperationExceptionType { Operation = "GetCase", ExceptionDetail = ex.Message },
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
                string caseTrackingId = getDocumentRequest != null && getDocumentRequest.DocumentQueryMessage != null && getDocumentRequest.DocumentQueryMessage.CaseTrackingID != null && !string.IsNullOrEmpty(getDocumentRequest.DocumentQueryMessage.CaseTrackingID.Value) ? getDocumentRequest.DocumentQueryMessage.CaseTrackingID.Value : string.Empty;
                string docketId = getDocumentRequest != null && getDocumentRequest.DocumentQueryMessage != null && getDocumentRequest.DocumentQueryMessage.CaseDocketID != null && !string.IsNullOrEmpty(getDocumentRequest.DocumentQueryMessage.CaseDocketID.Value) ? getDocumentRequest.DocumentQueryMessage.CaseDocketID.Value : string.Empty;

                wmp.GetDocumentResponse fetchResponse = this.GetDocument(docketId);
                if (fetchResponse != null)
                {
                    response = fetchResponse;
                }
                else
                {
                    response.DocumentResponseMessage.Document = null;
                    response.DocumentResponseMessage.Error = new System.Collections.Generic.List<ecf.ErrorType>
                    {
                        new ecf.ErrorType { ErrorCode = new nc.TextType("-31") , ErrorText = new nc.TextType( "Other Errror")}
                    } ;
                }

            }
            catch (Exception ex)
            {
                throw new FaultException<aoc20.OperationExceptionType>
                    (
                        new aoc20.OperationExceptionType { Operation = "GetDocument", ExceptionDetail = ex.Message },
                        new FaultReason(ex.Message),
                        new FaultCode("OTHER")
                    );

            }
            return response;
        }

        private wmp.GetDocumentResponse GetDocument(string docketId)
        {
            wmp.GetDocumentResponse response = null;
            if (!string.IsNullOrEmpty(docketId))
            {

                string caseXmlFile = GetApplicationPath() + @"\SampleDocuments\" + docketId + ".xml";
                if (File.Exists(caseXmlFile))
                {
                    using (var fs = new FileStream(caseXmlFile, FileMode.Open, FileAccess.Read))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(wmp.GetDocumentResponse));
                        response = serializer.Deserialize(fs) as wmp.GetDocumentResponse;
                    }

                }
            }
            return response;
        }

    }
}
