using UG.Journey.JourneyAccess.DataAccess;
using UG.Journey.JourneyAccess.Dao;
using Core;
using System.Collections.Generic;
using UG.Journey.JourneyAccess.DataAccess.Models;
using UG.Journey.JourneyAccess.DataAccess.Contracts;
using System;

namespace UG.Journey.JourneyAccess.Operations
{
    public interface IGetJourneyInfoOperation
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
    public class GetJourneyInfoOperation : IGetJourneyInfoOperation
    {
        #region Declarations

        private JourneyRequest _Request;
        private JourneyResponse _Response;
        private CreateJourneyRequest createJourneyRequest;
        private CreateJourenyResponse createJourneyResponse;
        private GetJourneyByUserRequest getJourneybyUserRequest;
        private GetJourneyByUserResponse getJourneyByUserResponse;

        public IJourneyDao journeyDao { get; set; }

        //public GetJourneyInfoOperation()
        //{
        //    journeyDao = IocManager.Resolve<IJourneyDao>();
        //}

        #endregion Declarations

        public JourneyResponse GetJourneyList(JourneyRequest Request)
        {
            _Request = Request;
            _Response = new JourneyResponse { ValidationResults = new ValidationResults() };

            journeyDao = IocManager.Resolve<IJourneyDao>();

            try
            {
                if (!_Response.ValidationResults.IsValid) return _Response;
                _Response.JourneyList = (ICollection<BookingTbl>)journeyDao.GetJourneyList(_Request, _Response.ValidationResults);
            }
            catch (Exception ex)
            {

            }
            //assignResponse();

            return _Response;
        }
        public JourneyResponse GetTransportModePrice(JourneyRequest Request) {
            _Request = Request;
            _Response = new JourneyResponse { ValidationResults = new ValidationResults() };

            journeyDao = IocManager.Resolve<IJourneyDao>();
            try
            {
                if (!_Response.ValidationResults.IsValid) return _Response;
                _Response.TransportModePrice = journeyDao.GetTransportModePrice(_Request, _Response.ValidationResults);
            }
            catch (Exception ex)
            {

            }

            return _Response;
        }
        public CreateJourenyResponse CreateJourney(CreateJourneyRequest Request)
        {
            createJourneyRequest = Request;
            createJourneyResponse = new CreateJourenyResponse { ValidationResults = new ValidationResults() };

            journeyDao = IocManager.Resolve<IJourneyDao>();

            if (!createJourneyResponse.ValidationResults.IsValid) return createJourneyResponse;

            createJourneyResponse = journeyDao.CreateJourney(createJourneyRequest, createJourneyResponse.ValidationResults);

            return createJourneyResponse;
        }

        public string UpdateJourneyLeg(int BookingDetailID, string LegStatus)
        {
            journeyDao = IocManager.Resolve<IJourneyDao>();
            return journeyDao.UpdateJourneyLeg(BookingDetailID,LegStatus);
        }
        public string UpdateJourneyStatus(int BookingID, string JourneyStatus)
        {
            journeyDao = IocManager.Resolve<IJourneyDao>();
            return journeyDao.UpdateJourneyStatus(BookingID, JourneyStatus);
        }

        public GetJourneyByUserResponse GetJourneyByUser(GetJourneyByUserRequest Request)
        {
            getJourneybyUserRequest = Request;
            getJourneyByUserResponse = new GetJourneyByUserResponse { ValidationResults = new ValidationResults() };

            journeyDao = IocManager.Resolve<IJourneyDao>();

            if (!getJourneyByUserResponse.ValidationResults.IsValid) return getJourneyByUserResponse;

            getJourneyByUserResponse = journeyDao.GetJourneyByUser(getJourneybyUserRequest, getJourneyByUserResponse.ValidationResults);

            return getJourneyByUserResponse;
        }

        public GetJourneyByUserResponse GetJourneyDetailByJourneyID(GetJourneyByUserRequest Request)
        {
            getJourneybyUserRequest = Request;
            getJourneyByUserResponse = new GetJourneyByUserResponse { ValidationResults = new ValidationResults() };

            journeyDao = IocManager.Resolve<IJourneyDao>();

            if (!getJourneyByUserResponse.ValidationResults.IsValid) return getJourneyByUserResponse;

            getJourneyByUserResponse = journeyDao.GetJourneyDetailByJourneyID(getJourneybyUserRequest, getJourneyByUserResponse.ValidationResults);

            return getJourneyByUserResponse;
        }
        public GetUserByKrypcAccountIDResponse GetUserByKrypcUsername(string userName)
        {
            journeyDao = IocManager.Resolve<IJourneyDao>();
            return journeyDao.GetUserByKrypcUsername(userName);
        }
        public GetFungibleTokenResponse GetFungibleToken(string tokenName)
        {
            journeyDao = IocManager.Resolve<IJourneyDao>();
            return journeyDao.GetFungibleToken(tokenName);
        }

        public ValidationResults CreateKrypcToken(string token)
        {
            journeyDao = IocManager.Resolve<IJourneyDao>();
            return journeyDao.CreateKrypcToken(token);
        }

        public ValidationResults GetKrypcToken()
        {
            journeyDao = IocManager.Resolve<IJourneyDao>();
            return journeyDao.GetKrypcToken();
        }

        public GetMSPByTransportResponse GetMSPByKrypcTransport(int TMode)
        {
            journeyDao = IocManager.Resolve<IJourneyDao>();
            return journeyDao.GetMSPByKrypcTransport(TMode);
        }

        public ValidationResults AddPaymentTransactionHistory(PaymentTransactionRequest request)
        {
            journeyDao = IocManager.Resolve<IJourneyDao>();
            return journeyDao.AddPaymentTransactionHistory(request);
        }

        public GetUserByKrypcAccountIDResponse GetUserByKrypcUserId(int userId)
        {
            journeyDao = IocManager.Resolve<IJourneyDao>();
            return journeyDao.GetUserByKrypcUserId(userId);
        }
    }
}
