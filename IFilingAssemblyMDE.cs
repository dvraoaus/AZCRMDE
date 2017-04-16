/*
	'=======================================================================
	'   Author(s):      D V Rao
	'   Module/Form:    IFilingAssemblyMDE.cs
	'   Created Date:   
	'   Description:    AZ FAMDE Service
	'
	'   Modification History:
	'=======================================================================
	'   Author(s)       Date        Control/Procedure       Change
	'=======================================================================
    '   Rao             04/16/2017 Creation
	'=======================================================================
	*/

using System.ServiceModel;
using wmp = Oasis.LegalXml.CourtFiling.v40.WebServiceMessagingProfile;


namespace Arizona.Courts.Services.v20
{
    [ServiceContract(Namespace = "http://schema.azcourts.az.gov/aoc/efiling/ecf/exchange/services/2.0/FilingAssemblyMDEPort", Name = "FilingAssemblyMDEPort")]
    [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
    public interface IFilingAssemblyMDE
    {
        [OperationContract(Action = "http://schema.azcourts.az.gov/aoc/efiling/ecf/exchange/services/2.0/FilingAssemblyMDEPort/NotifyFilingReviewComplete")]
        wmp.NotifyFilingReviewCompleteResponse NotifyFilingReviewComplete(wmp.NotifyFilingReviewCompleteRequest notifyFilingReviewCompleteRequest);

    }
}
