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

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Xml.Serialization;
using aoc20 = Arizona.Courts.Extensions.v20;
using core = Oasis.LegalXml.CourtFiling.v40.Core;
using ecf = Oasis.LegalXml.CourtFiling.v40.Ecf;
using nc = Niem.NiemCore.v20;
using wmp = Oasis.LegalXml.CourtFiling.v40.WebServiceMessagingProfile;
using message = Oasis.LegalXml.CourtFiling.v40.Message;
using amc20 = Arizona.Courts.ExChanges.v20;
using amc21 = Arizona.Courts.ExChanges.v21;
using azs = Arizona.Courts.Services.v20;

namespace Arizona.Courts.Services.v20
{
    [ServiceBehavior(Name = "FilingReviewMDEService", Namespace = "http://schema.azcourts.az.gov/aoc/efiling/ecf/exchange/services/2.0/FilingReviewMDEPort"), AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FilingReviewMDE : azs.IFilingReviewMDE
    {

        public wmp.ReviewFilingResponse ReviewFiling(wmp.ReviewFilingRequest reviewFilingRequest)
        {
            bool use21Version = reviewFilingRequest != null &&
                                reviewFilingRequest.ReviewFilingRequestMessageObject != null &&
                                reviewFilingRequest.ReviewFilingRequestMessageObject is amc21.ReviewFilingRequestWrapperType;

            wmp.ReviewFilingResponse response = null;
            message.MessageReceiptMessageType messageReceipt = new message.MessageReceiptMessageType
            {
                SendingMDELocationID = new nc.IdentificationType("http:/courts.az.gov/aoc/efiling/CRMDE"),
                SendingMDEProfileCode = nc.Constants.ECF4_WEBSERVICES_SIP_CODE,
                CaseCourt = reviewFilingRequest != null &&
                                reviewFilingRequest.ReviewFilingRequestMessage != null &&
                                reviewFilingRequest.ReviewFilingRequestMessage.CoreFilingMessage != null ?
                                ecf.EcfHelper.GetCourt(reviewFilingRequest.ReviewFilingRequestMessage.CoreFilingMessage.Case) :
                                null

            };
            if (use21Version)
            {
                response = new wmp.ReviewFilingResponse(new amc21.ReviewFilingResponseType {  MessageReceiptMessage = messageReceipt });
            }
            else
            {
                response = new wmp.ReviewFilingResponse(new amc20.ReviewFilingResponseType { MessageReceiptMessage = messageReceipt });
            }
            

            try
            {

                string confirmationId = this.SaveReviewFilingXML(reviewFilingRequest);

                if (!string.IsNullOrEmpty(confirmationId))
                {
                    messageReceipt.DocumentIdentification = new List<nc.IdentificationType>
                    {
                         new nc.IdentificationType(confirmationId) 
                    };
                    messageReceipt.Error = ecf.EcfHelper.QuerySuccessfull();
                }
                else
                {
                    messageReceipt.Error = ecf.EcfHelper.ErrorList("-9999", "Error Saving Record Filing Operation");
                }


            }
            catch (Exception ex)
            {
                throw new FaultException<aoc20.OperationExceptionType>
                    (
                        new aoc20.OperationExceptionType { Operation = "Review Filing", ExceptionDetail = ex.Message },
                        new FaultReason(ex.Message),
                        new FaultCode("OTHER")
                    );

            }
            return response;
        }

        private string SaveReviewFilingXML(wmp.ReviewFilingRequest reviewFilingRequest)
        {
            string cmsConformationNumber = string.Empty;
            core.CoreFilingMessageType filingMessage = reviewFilingRequest != null ? reviewFilingRequest.CoreFilingMessage : null;
            

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


        public wmp.NotifyDocketingCompleteResponse NotifyDocketingComplete(wmp.NotifyDocketingCompleteRequest notifyDocketingCompleteRequest)
        {
            wmp.NotifyDocketingCompleteResponse response = new wmp.NotifyDocketingCompleteResponse
            {
                MessageReceiptMessage = new Oasis.LegalXml.CourtFiling.v40.Message.MessageReceiptMessageType
                {

                }
            };
            return response;
        }

        public wmp.NotifyFilingStatusChangeResponse NotifyFilingStatusChange(wmp.NotifyFilingStatusChangeRequest notifyFilingStatusChangeRequest)
        {
            wmp.NotifyFilingStatusChangeResponse response = new wmp.NotifyFilingStatusChangeResponse { };
            return response;
        }

        public wmp.GetFilingStatusResponse GetFilingStatus(wmp.GetFilingStatusRequest getFilingStatusRequest)
        {
            wmp.GetFilingStatusResponse response = new wmp.GetFilingStatusResponse { };
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
