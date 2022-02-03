using Core;
using UG.Journey.JourneyAccess.DataAccess.Contracts;
using UG.Journey.JourneyAccess.Operations;

namespace UG.Journey.JourneyAccess.Client
{
    public interface IJourneyDataAccessClient
    {
        JourneyResponse GetJourneyList(JourneyRequest Request);
        JourneyResponse GetTransportModePrice(JourneyRequest Request);
        CreateJourenyResponse CreateJourney(CreateJourneyRequest request);
        string UpdateJourneyLeg(int BookingDetailID, string LegStatus);
        string UpdateJourneyStatus(int BookingID, string JourneyStatus);
        GetJourneyByUserResponse GetJourneyByUser(GetJourneyByUserRequest Request);
        GetJourneyByUserResponse GetJourneyDetailByJourneyID(GetJourneyByUserRequest Request);

        GetUserByKrypcAccountIDResponse GetUserByKrypcUsername(string userName);
        GetFungibleTokenResponse GetFungibleToken(string tokenName);
        ValidationResults CreateKrypcToken(string token);
        ValidationResults GetKrypcToken();
        GetMSPByTransportResponse GetMSPByKrypcTransport(int TMode);
        ValidationResults AddPaymentTransactionHistory(PaymentTransactionRequest request);

        GetUserByKrypcAccountIDResponse GetUserByKrypcUserId(int userId);
    }
    public class JourneyDataAccessClient : IJourneyDataAccessClient
    {
        public IGetJourneyInfoOperation GetJourneyInfoOperation;

        //public JourneyDataAccessClient()
        //{
        //    GetJourneyInfoOperation = IocManager.Resolve<IGetJourneyInfoOperation>();
        //}

        public JourneyResponse GetJourneyList(JourneyRequest Request)
        {
            GetJourneyInfoOperation = IocManager.Resolve<IGetJourneyInfoOperation>();
            var response = GetJourneyInfoOperation.GetJourneyList(Request);
            return response;
        }

        public JourneyResponse GetTransportModePrice(JourneyRequest Request)
        {
            GetJourneyInfoOperation = IocManager.Resolve<IGetJourneyInfoOperation>();
            var response = GetJourneyInfoOperation.GetTransportModePrice(Request);
            return response;
        }

        public CreateJourenyResponse CreateJourney(CreateJourneyRequest Request)
        {
            GetJourneyInfoOperation = IocManager.Resolve<IGetJourneyInfoOperation>();
            var response = GetJourneyInfoOperation.CreateJourney(Request);
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

        public GetJourneyByUserResponse GetJourneyDetailByJourneyID(GetJourneyByUserRequest Request)
        {
            GetJourneyInfoOperation = IocManager.Resolve<IGetJourneyInfoOperation>();
            var response = GetJourneyInfoOperation.GetJourneyDetailByJourneyID(Request);
            return response;
        }
        public GetUserByKrypcAccountIDResponse GetUserByKrypcUsername(string userName)
        {
            GetJourneyInfoOperation = IocManager.Resolve<IGetJourneyInfoOperation>();
            return GetJourneyInfoOperation.GetUserByKrypcUsername(userName);
        }

        public GetFungibleTokenResponse GetFungibleToken(string tokenName)
        {
            GetJourneyInfoOperation = IocManager.Resolve<IGetJourneyInfoOperation>();
            return GetJourneyInfoOperation.GetFungibleToken(tokenName);
        }

        public ValidationResults CreateKrypcToken(string token)
        {
            GetJourneyInfoOperation = IocManager.Resolve<IGetJourneyInfoOperation>();
            return GetJourneyInfoOperation.CreateKrypcToken(token);
        }

        public ValidationResults GetKrypcToken()
        {
            GetJourneyInfoOperation = IocManager.Resolve<IGetJourneyInfoOperation>();
            return GetJourneyInfoOperation.GetKrypcToken();
        }

        public GetMSPByTransportResponse GetMSPByKrypcTransport(int TMode)
        {
            GetJourneyInfoOperation = IocManager.Resolve<IGetJourneyInfoOperation>();
            return GetJourneyInfoOperation.GetMSPByKrypcTransport(TMode);
        }

        public ValidationResults AddPaymentTransactionHistory(PaymentTransactionRequest request)
        {
            GetJourneyInfoOperation = IocManager.Resolve<IGetJourneyInfoOperation>();
            return GetJourneyInfoOperation.AddPaymentTransactionHistory(request);
        }

        public GetUserByKrypcAccountIDResponse GetUserByKrypcUserId(int userId)
        {
            GetJourneyInfoOperation = IocManager.Resolve<IGetJourneyInfoOperation>();
            return GetJourneyInfoOperation.GetUserByKrypcUserId(userId);
        }
    }
}
