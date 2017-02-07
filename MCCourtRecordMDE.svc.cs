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
using azs = Arizona.Courts.Services.v20;
using ecf31 = Oasis.LegalXml.v31.CourtFiling;
using aoc = Arizona.Courts.Extensions.v20;

namespace Arizona.Courts.Services.v20
{
    [ServiceBehavior(Name = "CourtRecordMDEService", Namespace = "http://www.clerkofcourt.maricopa.gov"), AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MCCourtRecordMDE : azs.IMCCourtRecordMDE
    {

        public System.Xml.XmlNode GetDocument(System.Xml.XmlNode DocumentRequestXml)
        {
            System.Xml.XmlNode response = null;

            return response;
        }


        public azs.GetCaseResponse GetCase(azs.GetCaseRequest getCaseRequest)
        {
            azs.GetCaseResponse response = new azs.GetCaseResponse();
            try
            {

                string caseTrackingId = getCaseRequest != null &&
                                        getCaseRequest.CaseQueryMessage != null &&
                                        getCaseRequest.CaseQueryMessage.CaseTrackingID != null &&
                                        getCaseRequest.CaseQueryMessage.CaseTrackingID.ID != null &&
                                        !string.IsNullOrWhiteSpace(getCaseRequest.CaseQueryMessage.CaseTrackingID.ID.Value) ?
                                        getCaseRequest.CaseQueryMessage.CaseTrackingID.ID.Value :
                                        string.Empty;

                ecf31.CaseResponseMessageType caseResponseMessage = this.GetCase(caseTrackingId);
                if (caseResponseMessage != null)
                {
                    response = new azs.GetCaseResponse { CaseResponseMessage = caseResponseMessage };
                }
                else if (caseResponseMessage == null)
                {
                    string errorCode = "-10";
                    string errorText = string.Empty  ;
                    caseResponseMessage = new ecf31.CaseResponseMessageType
                    {
                        Error = new System.Collections.Generic.List<ecf31.ErrorType>
                        {
                            new ecf31.ErrorType
                            {
                                ErrorCode = new ecf31.PolicyDefinedCodeTextType { Value = errorCode },
                                ErrorText = new Gjxdm.TextType { Value = errorText }
                            }
                         }
                    };

                    response = new azs.GetCaseResponse { CaseResponseMessage = caseResponseMessage };

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

        private ecf31.CaseResponseMessageType GetCase(string caseTrackingId)
        {
            ecf31.CaseResponseMessageType caseResponse = null;
            if (!string.IsNullOrEmpty(caseTrackingId))
            {

                string caseXmlFile = GetApplicationPath() + @"\SampleCases\" + caseTrackingId + ".xml";
                if (File.Exists(caseXmlFile))
                {
                    using (var fs = new FileStream(caseXmlFile, FileMode.Open , FileAccess.Read ))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ecf31.CaseResponseMessageType));
                        caseResponse = serializer.Deserialize(fs) as ecf31.CaseResponseMessageType;
                    }

                }
            }
            return caseResponse;
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


        /*


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
                throw new FaultException<aoc.OperationExceptionType>
                    (
                        new aoc.OperationExceptionType { Operation = "GetDocument", ExceptionDetail = ex.Message },
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
        */
    }
}
