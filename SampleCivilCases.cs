/*
	'=======================================================================
	'   Author(s):      
	'   Module/Form:  Sample Civil Cases  
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
    public class SampleCivilCases
    {
        public static aoc.CivilCaseType AZCivilCase
        {
            get
            {
                aoc.OrganizationType aocOrganization = new aoc.OrganizationType
                               (
                                 id: "PTY0003",
                                 eportalOrganizationId: string.Empty,
                                 name: "SURETY ACCEPTANCE CO",
                                 eportalUnitId: string.Empty,
                                 unitName: string.Empty,
                                 eportalSubUnitId: string.Empty,
                                 subUnitName: string.Empty,
                                 address1: "400 East Broadway Boulevard",
                                 address2: string.Empty,
                                 city: "Tucson",
                                 state: "AZ",
                                 zipCode: "85711",
                                 phoneNumber: "520-790-7181",
                                 extension: string.Empty,
                                 emailAddress: string.Empty
                               );
                aocOrganization.OrganizationAugmentation1 = new j.OrganizationAugmentationType();
                aocOrganization.EcfOrganizationAugmentation = new ecf.OrganizationAugmentationType();
                List<ecf.CaseParticipantType> caseParticipant = new List<ecf.CaseParticipantType>
                        {
                            new aoc.CaseParticipantType
                            {
                                 EntityRepresentation = new ecf.PersonType
                                 ( 
                                   id : "PTY0001" ,
                                   prefix: string.Empty,
                                   givenName: "CASSANDRA",
                                   middleName:string.Empty,
                                   surName: "PRICE" ,
                                   suffix: string.Empty ,
                                   eportalUserId: string.Empty 
                                 ) ,
                                 EntityRepresentationType = nc.EntityRepresentationTpes.EcfPerson,
                                 CaseParticipantRoleCode = new nc.TextType("PL")  
                            } ,
                            new aoc.CaseParticipantType
                            {
                                 EntityRepresentation = new ecf.PersonType
                                 ( 
                                   id : "PTY0002" ,
                                   prefix: string.Empty,
                                   givenName: "CHRISTOPHER",
                                   middleName: string.Empty,
                                   surName: "PRICE" ,
                                   suffix: string.Empty ,
                                   eportalUserId: string.Empty 
                                 ) ,
                                 EntityRepresentationType = nc.EntityRepresentationTpes.EcfPerson,
                                 CaseParticipantRoleCode = new nc.TextType("PL")  
                            } ,
                            new aoc.CaseParticipantType
                            {
                                 EntityRepresentation = aocOrganization ,                                  
                                 EntityRepresentationType = nc.EntityRepresentationTpes.AZAOCOrganization,
                                 CaseParticipantRoleCode = new nc.TextType("DE")  
                            } ,

                        };

                return new aoc.CivilCaseType
                {
                    CaseTitleText = new List<nc.TextType> { new nc.TextType("CCPACKING ET AL. Vs. Lamex") },
                    CaseCategoryText = new List<nc.TextType> { new nc.TextType("Civil") },
                    CaseTrackingID = new List<niemxsd.String> { new niemxsd.String("C20117066") },
                    ClassActionIndicator = new niemxsd.Boolean(false),
                    JuryDemandIndicator = new niemxsd.Boolean(true),
                    CaseGeneralCategoryText = new nc.TextType("Civil"),
                    CaseSubCategoryText = new nc.TextType("Default"),
                    CaseAugmentation = new j.CaseAugmentationType
                    {
                        CaseCourt = new List<j.CourtType> { SampleCourts.CaseCourt }
                    },
                    EcfCaseAugmentation = new aoc.CaseAugmentationType
                    {
                        CaseParticipant = caseParticipant
                    }

                };
            }
        }

    }
}