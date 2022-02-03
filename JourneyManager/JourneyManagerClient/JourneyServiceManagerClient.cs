using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core;
using UG.Journey.JourneyManager.Contracts;
using UG.Journey.JourneyManager.Service.Operations;

namespace UG.Journey.JourneyManager.Client
{
    public interface IJourneyServiceManagerClient
    {
        Task<GetJourneyInfoResponse> GetJourneyInfo(GetJourneyInfoRequest Request);
        CreateJourneyResponse CreateJourney(CreateJourneyRequest Request);
        string UpdateJourneyLeg(int BookingDetailID, string LegStatus);
        string UpdateJourneyStatus(int BookingID, string JourneyStatus);
        GetJourneyByUserResponse GetJourneyByUser(GetJourneyByUserRequest Request);
        GetJourneyByUserResponse GetJourneyDetail(GetJourneyByUserRequest Request);
    }
    public class JourneyServiceManagerClient : IJourneyServiceManagerClient
    {
        #region Declarations

        public IGetJourneyInfoOperation GetJourneyInfoOperation;

        #endregion Declarations

        public Task<GetJourneyInfoResponse> GetJourneyInfo(GetJourneyInfoRequest Request)
        {
            GetJourneyInfoOperation = IocManager.Resolve<IGetJourneyInfoOperation>();
            var response = GetJourneyInfoOperation.GetJourneyInfo(Request);
            return response;
        }

        public CreateJourneyResponse CreateJourney(CreateJourneyRequest Request)
        {
            GetJourneyInfoOperation = IocManager.Resolve<IGetJourneyInfoOperation>();
            var response = GetJourneyInfoOperation.CreateJoureny(Request);
            return response;
        }

        public string UpdateJourneyLeg(int BookingDetailID, string LegStatus)
        {
            GetJourneyInfoOperation = IocManager.Resolve<IGetJourneyInfoOperation>();
            var response = GetJourneyInfoOperation.UpdateJourneyLeg(BookingDetailID, LegStatus);
            return response;
        }
        public string UpdateJourneyStatus(int BookingID, string JourneyStatus)
        {
            GetJourneyInfoOperation = IocManager.Resolve<IGetJourneyInfoOperation>();
            var response = GetJourneyInfoOperation.UpdateJourneyStatus(BookingID, JourneyStatus);
            return response;
        }

        public GetJourneyByUserResponse GetJourneyByUser(GetJourneyByUserRequest Request)
        {
            GetJourneyInfoOperation = IocManager.Resolve<IGetJourneyInfoOperation>();
            var response = GetJourneyInfoOperation.GetJourneyByUser(Request);
            return response;
        }

        public GetJourneyByUserResponse GetJourneyDetail(GetJourneyByUserRequest Request)
        {
            GetJourneyInfoOperation = IocManager.Resolve<IGetJourneyInfoOperation>();
            var response = GetJourneyInfoOperation.GetJourneyDetail(Request);
            return response;
        }

    }
}
