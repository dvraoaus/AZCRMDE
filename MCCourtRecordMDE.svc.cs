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

        public GetDocumentResponse GetDocument(GetDocumentRequest request)
        {
            azs.GetDocumentResponse response = new azs.GetDocumentResponse();
            try
            {
                string docketId = request != null && request.DocumentQueryMessage != null &&
                                  request.DocumentQueryMessage.DocumentID != null &&
                                  request.DocumentQueryMessage.DocumentID.ID != null &&
                                  !string.IsNullOrWhiteSpace(request.DocumentQueryMessage.DocumentID.ID.Value) ?
                                  request.DocumentQueryMessage.DocumentID.ID.Value :
                                  string.Empty;

                ecf31.DocumentResponseMessageType documentResponseMessage = this.GetDocumentResponse(docketId);
                if (documentResponseMessage != null)
                {
                    response = new azs.GetDocumentResponse { DocumentResponseMessage = documentResponseMessage };
                }
                else if (documentResponseMessage == null)
                {
                    string errorCode = "-10";
                    string errorText = string.Empty;
                    documentResponseMessage = new ecf31.DocumentResponseMessageType
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

                    response = new azs.GetDocumentResponse { DocumentResponseMessage = documentResponseMessage };

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

        public GetDocumentBytesResponse GetDocumentBytes(GetDocumentBytesRequest request)
        {
            azs.GetDocumentBytesResponse response = new azs.GetDocumentBytesResponse();
            try
            {

                string caseTrackingId = request != null &&
                                        !string.IsNullOrWhiteSpace(request.CaseNumber) ?
                                        request.CaseNumber: 
                                        string.Empty;
                string docketeId = request != null && !string.IsNullOrWhiteSpace(request.encodedDocId) ? request.encodedDocId : string.Empty;
                response.GetDocumentBytesResult = this.GetDocument(docketeId);

            }
            catch (Exception ex)
            {
                throw new FaultException<aoc.OperationExceptionType>
                    (
                        new aoc.OperationExceptionType { Operation = "GetDocumentBytes", ExceptionDetail = ex.Message },
                        new FaultReason(ex.Message),
                        new FaultCode("OTHER")
                    );

            }

            return response;


        }

        private byte[]  GetDocument(string docketId)
        {
            byte[] documentByes = null;
            if (!string.IsNullOrEmpty(docketId))
            {

                string pdfFile = GetApplicationPath() + @"\SampleDocuments\" + docketId + ".pdf";
                if (File.Exists(pdfFile))
                {
                    documentByes = File.ReadAllBytes(pdfFile);
                }
            }
            return documentByes;
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



        private ecf31.DocumentResponseMessageType GetDocumentResponse(string docketId)
        {
            ecf31.DocumentResponseMessageType documentResponse = null;
            if (!string.IsNullOrEmpty(docketId))
            {

                string documentXmlFile = GetApplicationPath() + @"\SampleDocuments\" + docketId + ".xml";
                if (File.Exists(documentXmlFile))
                {
                    using (var fs = new FileStream(documentXmlFile, FileMode.Open, FileAccess.Read))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ecf31.DocumentResponseMessageType));
                        documentResponse = serializer.Deserialize(fs) as ecf31.DocumentResponseMessageType;
                    }

                }
            }
            return documentResponse;
        }


    }
}
