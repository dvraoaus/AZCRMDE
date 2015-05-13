/*
	'=======================================================================
	'   Author(s):      
	'   Module/Form:  Sample Documents
	'   Created Date:   
	'   Description:    
	'
	'   Modification History:
	'=======================================================================
	'   Author(s)       Date        Control/Procedure       Change
	'=======================================================================
    =======================================================================
	*/

using System.Collections.Generic;
using aoc = Arizona.Courts.Extensions.v20;
using ecf = Oasis.LegalXml.CourtFiling.v40.Ecf;
using j = Niem.Domains.Jxdm.v40;
using nc = Niem.NiemCore.v20;
using niemxsd = Niem.Proxy.xsd.v20;
using System;
using System.IO;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web.Hosting;


namespace Arizona.Courts.Services.v20
{
    public class SampleDocuments
    {
        public static string GetApplicationPath()
        {
            string applicationPath = String.Empty;
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

            return applicationPath;
        }

        public static aoc.DocumentType AZDocument
        {
            get
            {
                string applicationPath = GetApplicationPath();
                string pdfDocumdent = GetApplicationPath() + @"\sample.pdf";

                return new aoc.DocumentType
                (
                   documentGuid : string.Empty ,
                   documentSequenceId: "1",
                   documentCategoryId: "MT",
                   documentCategoryName: "Motions",
                   documentTitle: "Motion for attorney fees",
                   documentDescription: "Motion for plaintiff 'CCPacking' attorney fees",
                   filingAttorneyId: null,
                   filingPartyIds: null,
                   parentDocumentId: string.Empty,
                   mimeType: "application/pdf",
                   documentRendition: File.ReadAllBytes(pdfDocumdent),
                   localFileName: string.Empty,
                   numberOfPages:0 
                );
            }
        }

    } // Class

} // Name Space