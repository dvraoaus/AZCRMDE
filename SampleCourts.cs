/*
	'=======================================================================
	'   Author(s):      
	'   Module/Form:  Sample Courts
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

namespace Arizona.Courts.Services.v20
{
    public class SampleCourts
    {
        public static j.CourtType CaseCourt
        {
            get
            {
                return new j.CourtType
                {
                    OrganizationIdentification = new List<nc.IdentificationType> { new nc.IdentificationType("courts.az.gov:1000") },
                    OrganizationLocation = new List<nc.LocationType>
                     {
                         new nc.LocationType
                         {
                              LocationAddress = new List<nc.AddressType>
                              {
                                  new nc.AddressType
                                      (
                                        address1: "110 West Congress Street" ,
                                        address2 : string.Empty ,
                                        city : "Tucson" ,
                                        state: "AZ" ,
                                        zipCode:"85701" ,
                                        countryCode:"US" ,
                                        country: "United States of America"
                                      )
                              }
                         }
                     },
                    CourtName = new List<nc.TextType>
                     {
                         new  nc.TextType("Pima Superior Court")
                     }
                };
            }

        }

    }
}