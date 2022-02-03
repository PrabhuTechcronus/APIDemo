using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Core;
using UG.Journey.JourneyManager.Client;
using UG.Journey.JourneyManager.Contracts;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace JourneyTestLambda
{
    public class JourneyLamda
    {

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
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
