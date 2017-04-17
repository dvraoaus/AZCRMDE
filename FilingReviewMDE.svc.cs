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
    '   Rao 01/28/2017 Removed unused usings
    =======================================================================
	*/

using Oasis.LegalXml.CourtFiling.v40.WebServiceMessagingProfile;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Xml.Serialization;
using aoc = Arizona.Courts.Extensions.v20;
using core = Oasis.LegalXml.CourtFiling.v40.Core;
using ecf = Oasis.LegalXml.CourtFiling.v40.Ecf;
using nc = Niem.NiemCore.v20;
using wmp = Oasis.LegalXml.CourtFiling.v40.WebServiceMessagingProfile;
namespace Arizona.Courts.Services.v20
{
    [ServiceBehavior(Name = "FilingReviewMDEService", Namespace = "urn:oasis:names:tc:legalxml-courtfiling:wsdl:WebServiceMessagingProfile-Definitions-4.0"), AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FilingReviewMDE : wmp.IFilingReviewMDE
    {

        public ReviewFilingResponse ReviewFiling(wmp.ReviewFilingRequest reviewFilingRequest)
        {
            ReviewFilingResponse response = new ReviewFilingResponse
            {
                 MessageReceiptMessage = new Oasis.LegalXml.CourtFiling.v40.Message.MessageReceiptMessageType
                 {
                    SendingMDELocationID = new  nc.IdentificationType("http:/courts.az.gov/aoc/efiling/CRMDE") ,
                    SendingMDEProfileCode =  nc.Constants.ECF4_WEBSERVICES_SIP_CODE ,
                    CaseCourt = reviewFilingRequest != null && 
                                reviewFilingRequest.ReviewFilingRequestMessage != null && 
                                reviewFilingRequest.ReviewFilingRequestMessage.CoreFilingMessage != null ?
                                ecf. EcfHelper.GetCourt(reviewFilingRequest.ReviewFilingRequestMessage.CoreFilingMessage.Case) :
                                null

                 }

            } ;

            try
            {

                string confirmationId = this.SaveReviewFilingXML(reviewFilingRequest);

                if (!string.IsNullOrEmpty(confirmationId))
                {
                    response.MessageReceiptMessage.DocumentIdentification = new List<nc.IdentificationType>
                    {
                         new nc.IdentificationType(confirmationId) 
                    };
                    response.MessageReceiptMessage.Error = ecf.EcfHelper.QuerySuccessfull();
                }
                else
                {
                    response.MessageReceiptMessage.Error = ecf.EcfHelper.ErrorList("-9999", "Error Saving Record Filing Operation");
                }


            }
            catch (Exception ex)
            {
                throw new FaultException<aoc.OperationExceptionType>
                    (
                        new aoc.OperationExceptionType { Operation = "Review Filing", ExceptionDetail = ex.Message },
                        new FaultReason(ex.Message),
                        new FaultCode("OTHER")
                    );

            }
            return response;
        }

        private string SaveReviewFilingXML(wmp.ReviewFilingRequest reviewFilingRequest)
        {
            string cmsConformationNumber = string.Empty;
            core.CoreFilingMessageType coreFilingMessage = reviewFilingRequest != null ? reviewFilingRequest.CoreFilingMessage : null;
            aoc.CoreFilingMessageType filingMessage = coreFilingMessage != null && coreFilingMessage is aoc.CoreFilingMessageType ? coreFilingMessage as aoc.CoreFilingMessageType : null;

            if (filingMessage != null)
            {
                string submissionId = ecf.EcfHelper.GetIdentificationValue(filingMessage.DocumentIdentification, "SubmissionID");
                if (!string.IsNullOrWhiteSpace(submissionId))
                {
                    string reviewFilingFilesSaveFolder = ConfigurationManager.AppSettings["reviewFilingFilesSaveFolder"];
                    if (string.IsNullOrWhiteSpace(reviewFilingFilesSaveFolder) || !Directory.Exists(reviewFilingFilesSaveFolder))
                    {
                        reviewFilingFilesSaveFolder = Path.GetTempPath();
                    }
                    string serializedFileName = Path.Combine(reviewFilingFilesSaveFolder, submissionId + ".xml");
                    if (File.Exists(serializedFileName)) File.Delete(serializedFileName);
                    using (FileStream fs = new FileStream(serializedFileName, FileMode.CreateNew, FileAccess.Write))
                    {
                            XmlSerializer serializer = new XmlSerializer(typeof(wmp.ReviewFilingRequest));
                            serializer.Serialize(fs, reviewFilingRequest);
                            fs.Flush();
                            cmsConformationNumber = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + submissionId.ToString();
                    }
                }
            }
            return cmsConformationNumber;
        }


        public NotifyDocketingCompleteResponse NotifyDocketingComplete(NotifyDocketingCompleteRequest notifyDocketingCompleteRequest)
        {
            NotifyDocketingCompleteResponse response = new NotifyDocketingCompleteResponse
            {
                MessageReceiptMessage = new Oasis.LegalXml.CourtFiling.v40.Message.MessageReceiptMessageType
                {

                }
            };
            return response;
        }

        public NotifyFilingStatusChangeResponse NotifyFilingStatusChange(NotifyFilingStatusChangeRequest notifyFilingStatusChangeRequest)
        {
            NotifyFilingStatusChangeResponse response = new NotifyFilingStatusChangeResponse { };
            return response;
        }

        public GetFilingStatusResponse GetFilingStatus(GetFilingStatusRequest getFilingStatusRequest)
        {
            GetFilingStatusResponse response = new GetFilingStatusResponse { };
            return response;
        }

        /*
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
         */

    }
}
