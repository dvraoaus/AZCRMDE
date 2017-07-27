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
    ' Rao 04/16/2017 FilingAssemblyMDE
    =======================================================================
	*/


using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Xml.Serialization;
using aoc = Arizona.Courts.Extensions.v20;
using azs = Arizona.Courts.Services.v20;
using ecf = Oasis.LegalXml.CourtFiling.v40.Ecf;
using nc = Niem.NiemCore.v20;
using wmp = Oasis.LegalXml.CourtFiling.v40.WebServiceMessagingProfile;
using amc = Arizona.Courts.ExChanges.v20;


namespace Arizona.Courts.Services.v20
{
    [ServiceBehavior(Name = "FilingAssemblyMDEService", Namespace = "http://schema.azcourts.az.gov/aoc/efiling/ecf/exchange/services/2.0/FilingAssemblyMDEPort"), AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FilingAssemblyMDE : azs.IFilingAssemblyMDE
    {

        public wmp.NotifyFilingReviewCompleteResponse NotifyFilingReviewComplete(wmp.NotifyFilingReviewCompleteRequest websipNotifyFilingReviewCompleteRequest)
        {
            wmp.NotifyFilingReviewCompleteResponse response = new wmp.NotifyFilingReviewCompleteResponse
            {
                
                NotifyFilingReviewCompleteResponseMessageObject = new amc.NotifyFilingReviewCompleteResponseWrapperType
                {
                    NotifyFilingReviewCompleteResponse = new amc.NotifyFilingReviewCompleteResponseType
                    {
                        MessageReceiptMessage = new Oasis.LegalXml.CourtFiling.v40.Message.MessageReceiptMessageType
                        {
                            SendingMDELocationID = new nc.IdentificationType("http:/efsp.other.com/aoc/efiling/FAMDE"),
                            SendingMDEProfileCode = nc.Constants.ECF4_WEBSERVICES_SIP_CODE,
                            CaseCourt = null
                        }

                    }
                }
                /*
                NotifyFilingReviewCompleteResponseMessageObject = new amc.NotifyFilingReviewCompleteResponseType
                {
                    MessageReceiptMessage = new Oasis.LegalXml.CourtFiling.v40.Message.MessageReceiptMessageType
                    {
                        // SendingMDELocationID = new nc.IdentificationType("http:/efsp.other.com/aoc/efiling/FAMDE"),
                        // SendingMDEProfileCode = nc.Constants.ECF4_WEBSERVICES_SIP_CODE,
                        // CaseCourt = null
                    }

                } 
                */

            };

            try
            {
                amc.NotifyFilingReviewCompleteRequestType notifyFilingReviewCompleteRequest = null;
                if (websipNotifyFilingReviewCompleteRequest != null &&
                    websipNotifyFilingReviewCompleteRequest.NotifyFilingReviewCompleteMessageObject != null &&
                    websipNotifyFilingReviewCompleteRequest.NotifyFilingReviewCompleteMessageObject is amc.NotifyFilingReviewCompleteRequestWrapperType
                    )
                {
                        notifyFilingReviewCompleteRequest = (websipNotifyFilingReviewCompleteRequest.NotifyFilingReviewCompleteMessageObject as amc.NotifyFilingReviewCompleteRequestWrapperType).NotifyFilingReviewCompleteRequest;
                    
                }
                string confirmationId = this.SaveNotifyFilingReviewCompleteXML(notifyFilingReviewCompleteRequest);

                if (!string.IsNullOrEmpty(confirmationId))
                {
                    //response.MessageReceiptMessage.DocumentIdentification = new List<nc.IdentificationType>
                    //{
                    //     new nc.IdentificationType(confirmationId) 
                    //};
                    response.MessageReceiptMessage.Error = ecf.EcfHelper.QuerySuccessfull();
                }
                else
                {
                    response.MessageReceiptMessage.Error = ecf.EcfHelper.ErrorList("-9999", "Error Saving NotifyFilingReviewComplete Operation");
                }


            }
            catch (Exception ex)
            {
                throw new FaultException<aoc.OperationExceptionType>
                    (
                        new aoc.OperationExceptionType { Operation = "NotifyFilingReviewComplete", ExceptionDetail = ex.Message },
                        new FaultReason(ex.Message),
                        new FaultCode("OTHER")
                    );

            }
            return response;
        }

        private string SaveNotifyFilingReviewCompleteXML(amc.NotifyFilingReviewCompleteRequestType notifyFilingReviewCompleteRequest)
        {
            string cmsConformationNumber = string.Empty;

            if (notifyFilingReviewCompleteRequest != null &&
                notifyFilingReviewCompleteRequest.ReviewFilingCallbackMessage != null &&
                notifyFilingReviewCompleteRequest.ReviewFilingCallbackMessage.Count > 0 &&
                notifyFilingReviewCompleteRequest.ReviewFilingCallbackMessage[0].DocumentIdentification != null 
                )
            {
                string submissionId = ecf.EcfHelper.GetIdentificationValue(notifyFilingReviewCompleteRequest.ReviewFilingCallbackMessage[0].DocumentIdentification, "SubmissionID");
                if (!string.IsNullOrWhiteSpace(submissionId))
                {
                    string reviewFilingFilesSaveFolder = ConfigurationManager.AppSettings["nfrcFilesSaveFolder"];
                    if (string.IsNullOrWhiteSpace(reviewFilingFilesSaveFolder) || !Directory.Exists(reviewFilingFilesSaveFolder))
                    {
                        reviewFilingFilesSaveFolder = Path.GetTempPath();
                    }
                    string serializedFileName = Path.Combine(reviewFilingFilesSaveFolder, submissionId + "_nfrc.xml");
                    if (File.Exists(serializedFileName)) File.Delete(serializedFileName);
                    using (FileStream fs = new FileStream(serializedFileName, FileMode.CreateNew, FileAccess.Write))
                    {
                            XmlSerializer serializer = new XmlSerializer(typeof(amc.NotifyFilingReviewCompleteRequestType));
                            serializer.Serialize(fs, notifyFilingReviewCompleteRequest);
                            fs.Flush();
                            cmsConformationNumber = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + submissionId.ToString();
                    }
                }
            }
            return cmsConformationNumber;
        }


    }
}
