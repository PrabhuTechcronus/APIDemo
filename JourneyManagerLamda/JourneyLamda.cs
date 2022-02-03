using Core;
using System;
using System.Threading.Tasks;
using UG.Journey.JourneyManager.Client;
using UG.Journey.JourneyManager.Contracts;

namespace JourneyManagerLamda
{
    public class JourneyLamda
    {

        public async Task<string> Journeyfunction()
        {
            try
            {
                GetJourneyInfoRequest getJourneyInfoRequest = new GetJourneyInfoRequest();
                getJourneyInfoRequest.RequestUrl = "";
                getJourneyInfoRequest.FromLocation = "Friesland, Netherlands";
                getJourneyInfoRequest.ToLocation = "Netherlands";
                var JourneyServiceManagerClient = IocManager.Resolve<IJourneyServiceManagerClient>();
                var response = await JourneyServiceManagerClient.GetJourneyInfo(getJourneyInfoRequest);
                return response.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
