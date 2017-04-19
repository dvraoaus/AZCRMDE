using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using aoc = Arizona.Courts.Extensions.v20;
using System.ServiceModel;
using System.Configuration;
using System.IO;
using ecf = Oasis.LegalXml.CourtFiling.v40.Ecf;
using wse2 = Microsoft.Web.Services2;
using System.Xml;
using System.Xml.Serialization;
using ecf31 = Oasis.LegalXml.v31.CourtFiling;

namespace Arizona.Courts.Services.v20
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://www.clerkofcourt.maricopa.gov")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MCFilingReviewMDE :  System.Web.Services.WebService
    {

        [WebMethod]
        public string Initialize(string Filename , bool Overwrite)
        {
            string instanceManagerId = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(Filename))
                {
                    string documentsFolder = Path.Combine(GetSaveFolder(), "Documents");
                    if (!Directory.Exists(documentsFolder))
                    {
                        Directory.CreateDirectory(documentsFolder);
                    }
                    string savedFileName = documentsFolder + @"\" + Filename;
                    if (Overwrite && File.Exists(savedFileName))
                    {
                        File.Delete(savedFileName);
                    }
                    // Create an empty file
                    using (File.Create(savedFileName)) { }
                    instanceManagerId = Filename;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<aoc.OperationExceptionType>
                    (
                        new aoc.OperationExceptionType { Operation = "Initialize", ExceptionDetail = ex.Message },
                        new FaultReason(ex.Message),
                        new FaultCode("OTHER")
                    );

            }
            return instanceManagerId;
        }

        private string GetSaveFolder()
        {
            string saveFolder = ConfigurationManager.AppSettings["mcfrmdeSaveFolder"];
            if (string.IsNullOrWhiteSpace(saveFolder) || !Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }
            return saveFolder;
        }

        [WebMethod]
        public void AppendChunk(string InstanceManagerId, long offset , long bufferSize)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(InstanceManagerId) && bufferSize > 0)
                {

                    string savedFileName = Path.Combine(GetSaveFolder(), "Documents") + @"\" + InstanceManagerId;
                    if (File.Exists(savedFileName))
                    {
                        byte[] chunk = null;
                        wse2.SoapContext conext = wse2.RequestSoapContext.Current;
                        if (conext != null && conext.Attachments != null && conext.Attachments.Count > 0 && conext.Attachments[0].Stream != null )
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                conext.Attachments[0].Stream.CopyTo(ms);
                                chunk = ms.ToArray();
                            }
                        }
                        if (chunk != null && chunk.Length > 0)
                        {
                            using (var fs = new FileStream(savedFileName, FileMode.Append))
                            {
                                fs.Write(chunk, 0, chunk.Length);
                                fs.Flush();
                                fs.Close();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new FaultException<aoc.OperationExceptionType>
                    (
                        new aoc.OperationExceptionType { Operation = "AppendChunk", ExceptionDetail = ex.Message },
                        new FaultReason(ex.Message),
                        new FaultCode("OTHER")
                    );

            }
            

        }

        [WebMethod]
        public void RemoveInstanceManager(string InstanceManagerId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(InstanceManagerId) )
                {
                    // Append to File 
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<aoc.OperationExceptionType>
                    (
                        new aoc.OperationExceptionType { Operation = "RemoveInstanceManager", ExceptionDetail = ex.Message },
                        new FaultReason(ex.Message),
                        new FaultCode("OTHER")
                    );

            }
        }

        [WebMethod]
        public string ReviewFiling(string ReviewFilingRequest)
        {
            string reviewFilingResponse = string.Empty;
            try
            {
                
                if (!string.IsNullOrWhiteSpace(ReviewFilingRequest))
                {
                    string submissionID = GetSubmissionNumber(ReviewFilingRequest);
                    string filingMessagesFolder = Path.Combine(GetSaveFolder(), "FilingMessages");
                    if (!Directory.Exists(filingMessagesFolder))
                    {
                        Directory.CreateDirectory(filingMessagesFolder);
                    }

                    string savedFileName = filingMessagesFolder + @"\" + submissionID + ".xml";
                    if (File.Exists(savedFileName))
                    {
                        File.Delete(savedFileName);
                    }
                    File.WriteAllText(savedFileName, ReviewFilingRequest);

                }
            }
            catch (Exception ex)
            {
                throw new FaultException<aoc.OperationExceptionType>
                    (
                        new aoc.OperationExceptionType { Operation = "ReviewFiling", ExceptionDetail = ex.Message },
                        new FaultReason(ex.Message),
                        new FaultCode("OTHER")
                    );

            }
            return reviewFilingResponse;
        }

        private string GetSubmissionNumber(string ReviewFilingRequest)
        {
            string submissionNumber = string.Empty;
            XmlSerializer coreFilingMessageSerializer = null;
            XmlSerializerNamespaces coreFilingMessageNamespaces = null;
            ecf31.CoreFilingMessageType coreFilingMessage = null;
            if (coreFilingMessageSerializer == null)
            {
                coreFilingMessageNamespaces = new XmlSerializerNamespaces();
                coreFilingMessageNamespaces.Add("message", "urn:oasis:names:tc:legalxml-courtfiling:schema:xsd:MessageTypes-3.0");
                coreFilingMessageNamespaces.Add("jxdm", "http://www.it.ojp.gov/jxdm/3.0.3");
                coreFilingMessageNamespaces.Add("common", "urn:oasis:names:tc:legalxml-courtfiling:schema:xsd:CommonTypes-3.0");
                coreFilingMessageNamespaces.Add("urn", "urn:oasis:names:tc:legalxml-courtfiling:schema:xsd:CoreFilingMessage-3.0");
                coreFilingMessageSerializer = new XmlSerializer(typeof(ecf31.CoreFilingMessageType));
            }
            using (TextReader reader = new StringReader(ReviewFilingRequest))
            {
                coreFilingMessage = coreFilingMessageSerializer.Deserialize(reader) as ecf31.CoreFilingMessageType;
            }
            if (coreFilingMessage != null && coreFilingMessage.FilingID != null && coreFilingMessage.FilingID.ID != null && !string.IsNullOrWhiteSpace(coreFilingMessage.FilingID.ID.Value))
            {
                submissionNumber = coreFilingMessage.FilingID.ID.Value;
            }
            if (string.IsNullOrWhiteSpace(submissionNumber)) submissionNumber = ecf.EcfHelper.UUID; 
            return submissionNumber;
        }
    }
}
