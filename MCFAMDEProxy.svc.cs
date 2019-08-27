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
using aoc = Arizona.Courts.Extensions.v20;
using azs = Arizona.Courts.Services.v20;

namespace Arizona.Courts.Services.v20
{
    [ServiceBehavior(Name = "MCFAMDEProxyService", Namespace = "http://www.clerkofcourt.maricopa.gov"), AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MCFAMDEProxy : azs.IMCFAMDEProxy
    {

        public azs.GetCaseResponse GetCase(azs.GetCaseRequest getCaseRequest)
        {
            return new GetCaseResponse();
        }

        public azs.notifyFilingReviewCompleteResponse notifyFilingReviewComplete(azs.notifyFilingReviewCompleteRequest request)
        {
            azs.notifyFilingReviewCompleteResponse response = new azs.notifyFilingReviewCompleteResponse();
            try
            {
                if (request != null && request.notifyFilingReviewComplete != null)
                {

                }

                response = new notifyFilingReviewCompleteResponse { notifyFilingReviewCompleteReturn = "TODO" };

                

            }
            catch (Exception ex)
            {
                throw new FaultException<aoc.OperationExceptionType>
                    (
                        new aoc.OperationExceptionType {  Operation = "notifyFilingReviewComplete",  ExceptionDetail = ex.Message },
                        new FaultReason(ex.Message),
                        new FaultCode("OTHER")
                    );

            }

            return response;

        }

        public System.Xml.XmlNode GetDocument(System.Xml.XmlNode DocumentRequestXml)
        {
            System.Xml.XmlNode response = null;

            return response;
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


    }
}
