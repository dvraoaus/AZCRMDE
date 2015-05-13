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
                                        address1: "120 S Cortez St" ,
                                        address2 : string.Empty ,
                                        city : "Prescott" ,
                                        state: "AZ" ,
                                        zipCode:"86302" ,
                                        countryCode:"US"
                                  ) 
                                  
                              }
                         }
                     },
                    CourtName = new List<nc.TextType>
                     {
                         new  nc.TextType("Yavapai County Superior Court ")
                     }
                };
            }

        }


    }
}