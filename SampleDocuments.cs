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
                string pdfDocumdent = GetApplicationPath()  +  @"\sample.pdf" ;
                    byte[] pdfSample = File.ReadAllBytes(pdfDocumdent) ;

                    return new aoc.DocumentType
                    {
                        DocumentApplicationName = new List<nc.ApplicationNameType> { new nc.ApplicationNameType("application/pdf") },
                        DocumentCategoryName = new nc.TextType("Civil Cover Sheet"),
                        DocumentDescriptionText = new List<nc.TextType> { new nc.TextType("Certificate of Service") },
                        DocumentFiledDate = new List<nc.DateType> { new nc.DateType(new DateTime(2010, 08, 06)) },
                        DocumentStatus = new List<nc.StatusType>
                             { 
                                 new nc.StatusType
                                 {
                                      StatusText = new List<nc.TextType>{ new nc.TextType("accepted")} ,
                                      StatusDate = new List<nc.DateType> { new nc.DateType(new DateTime(2010,08,06))}  ,
                                 }
                             },
                        DocumentMetadata = new aoc.DocumentMetadataType
                        {
                            RegisterActionDescriptionText = new nc.TextType("COVER")
                        },
                        DocumentRendition = new List<ecf.DocumentRenditionType>
                            {
                                new ecf.DocumentRenditionType
                                {
                                     DocumentFileControlID = new List<niemxsd.String>{ new niemxsd.String("C20140006") } ,
                                     DocumentIdentification = new  List<nc.IdentificationType>{ new nc.IdentificationType("133551" , "DOCUMENTID")} ,
                                     DocumentRenditionMetadata = new ecf.DocumentRenditionMetadataType
                                     {
                                          DocumentAttachment = new List<ecf.DocumentAttachmentType>
                                          {
                                              new ecf.DocumentAttachmentType
                                              {
                                                    BinaryDescriptionText  = new List<nc.TextType>{ new nc.TextType("application/pdf")} ,
                                                    BinarySizeValue = new List<nc.NonNegativeDecimalType>{ new nc.NonNegativeDecimalType(pdfSample.Length)},
                                                    AttachmentSequenceID = new niemxsd.String("1"),
                                                    BinaryObjects  = new List<niemxsd.Base64Binary>
                                                    {
                                                         new niemxsd.Base64Binary(pdfSample)
                                                    }

                                              }
                                          }
                                     }
                                }
                            }
                    };
            }
        }
    } // Class

} // Name Space