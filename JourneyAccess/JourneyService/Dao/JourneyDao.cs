using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UG.Journey.JourneyAccess.DataAccess.Models;
using UG.Journey.JourneyAccess.DataAccess.Contracts;
using Newtonsoft.Json;

namespace UG.Journey.JourneyAccess.Dao
{
    public interface IJourneyDao
    {
        ICollection<BookingTbl> GetJourneyList(JourneyRequest Request, ValidationResults ValidationResults);
        GetTransportPriceResponse GetTransportModePrice(JourneyRequest Request, ValidationResults ValidationResults);
        CreateJourenyResponse CreateJourney(CreateJourneyRequest Request, ValidationResults ValidationResults);
        string UpdateJourneyLeg(int BookingDetailID, string LegStatus);
        string UpdateJourneyStatus(int BookingID, string JourneyStatus);
        GetJourneyByUserResponse GetJourneyByUser(GetJourneyByUserRequest Request, ValidationResults ValidationResults);
        GetJourneyByUserResponse GetJourneyDetailByJourneyID(GetJourneyByUserRequest Request, ValidationResults ValidationResults);
        GetUserByKrypcAccountIDResponse GetUserByKrypcUsername(string userName);
        GetFungibleTokenResponse GetFungibleToken(string tokenName);
        ValidationResults CreateKrypcToken(string token);
        ValidationResults GetKrypcToken();
        GetMSPByTransportResponse GetMSPByKrypcTransport(int TMode);
        ValidationResults AddPaymentTransactionHistory(PaymentTransactionRequest request);
        GetUserByKrypcAccountIDResponse GetUserByKrypcUserId(int userId);
    }
    public class JourneyDao : IJourneyDao
    {
        #region Declarations

        private UrbanGoContext Db;
        //public JourneyDao()
        //{
        //    Db = new UrbanGoContext();
        //}

        #endregion Declarations
        public ICollection<BookingTbl> GetJourneyList(JourneyRequest Request, ValidationResults ValidationResults)
        {
            Db = new UrbanGoContext();

            ICollection<BookingTbl> resultList = null;
            try
            {
                ValidationResults = ValidationResults ?? new ValidationResults();

                resultList = Db.BookingTbls.Where(p => p.UserId == Convert.ToInt32(Request.UserId)).ToList();
            }
            catch (Exception ex)
            {
                ValidationResults.AddResult(new ValidationResult
                {
                    ValidationMessage = $"{ex.Message}\n{ex.InnerException?.Message}",
                    ExceptionInformation = $"{ex.StackTrace}\n{ex.InnerException?.StackTrace}"
                });
            }
            return resultList;
        }
        public GetTransportPriceResponse GetTransportModePrice(JourneyRequest Request, ValidationResults ValidationResults)
        {
            Db = new UrbanGoContext();
            GetTransportPriceResponse response = new GetTransportPriceResponse();
            TransportPrice resultList = new TransportPrice();
            try
            {
                ValidationResults = ValidationResults ?? new ValidationResults();

                //resultList = Db.TransportPrices.Where(p => p.TransportMode == Request.TransportMode).ToList().FirstOrDefault();

                var result = (from TP in Db.TransportPrices
                              join T in Db.TransportModeTbls on TP.TransportType equals T.TransportMode
                              where TP.TransportMode == Request.TransportMode
                              select new { TP.TransportMode, TP.TransportType, TP.PricePerKm, T.Id,TP.Co2PerKm }).FirstOrDefault();

                var msp = (from TP in Db.MspTbls
                           join T in Db.TransportModeTbls on TP.TransportMode equals T.Id
                           where T.TransportMode == Request.TransportMode
                           select new { TP.TransportMode, TP.MspName, TP.MspLogo}).FirstOrDefault();

                //foreach (var item in result)
                if(result != null)
                {
                    resultList.Id = result.Id;
                    resultList.PricePerKm = result.PricePerKm;
                    resultList.TransportMode = result.TransportMode;
                    resultList.TransportType = result.TransportType;
                    resultList.Co2PerKm = result.Co2PerKm;
                }

                response.transportPrice = resultList;
                if(msp != null)
                {
                    response.MspName = msp.MspName;
                    response.MspLogo = msp.MspLogo;
                }
            }
            catch (Exception ex)
            {
                ValidationResults.AddResult(new ValidationResult
                {
                    ValidationMessage = $"{ex.Message}\n{ex.InnerException?.Message}",
                    ExceptionInformation = $"{ex.StackTrace}\n{ex.InnerException?.StackTrace}"
                });
            }
            return response;
        }
        public CreateJourenyResponse CreateJourney(CreateJourneyRequest Request, ValidationResults ValidationResults)
        {
            Db = new UrbanGoContext();

            CreateJourenyResponse resultList = new CreateJourenyResponse();
            try
            {
                ValidationResults = ValidationResults ?? new ValidationResults();

                BookingTbl bookingTbl = new BookingTbl();
                //bookingTbl.Id = Request.Journey.Id;
                bookingTbl.UserId = Request.Journey.UserId;
                bookingTbl.LocationFrom = Request.Journey.LocationFrom;
                bookingTbl.LocationTo = Request.Journey.LocationTo;
                bookingTbl.TimeFrom = Request.Journey.TimeFrom;
                bookingTbl.TimeTo = Request.Journey.TimeTo;
                bookingTbl.TotalHours = Request.Journey.TotalHours;
                bookingTbl.TotalPrice = Request.Journey.TotalPrice;
                bookingTbl.CarbonEmission = Request.Journey.CarbonEmission;
                bookingTbl.GreenPoints = Request.Journey.GreenPoints;
                bookingTbl.TopicId = Request.Journey.TopicId;
                bookingTbl.TransactionId = Request.Journey.TransactionId;
                bookingTbl.SequenceNumber = Request.Journey.SequenceNumber;
                bookingTbl.NetworkType = Request.Journey.NetworkType;
                bookingTbl.TransportStatus = "Booked";
                Db.BookingTbls.Add(bookingTbl);
                Db.SaveChanges();

                BookingDetailTbl bookingDetailTbl;
                TripLegsTbl tripLegs;

                for (int i = 0; i <= Request.JourneyDetail.Count - 1; i++)
                {
                    bookingDetailTbl = new BookingDetailTbl();
                    bookingDetailTbl.BookingId = bookingTbl.Id;
                    bookingDetailTbl.TransportMode = Request.JourneyDetail[i].TransportMode;
                    bookingDetailTbl.TransportPassNumber = Request.JourneyDetail[i].TransportPassNumber;
                    bookingDetailTbl.TransportVehicleNo = Request.JourneyDetail[i].TransportVehicleNo;
                    bookingDetailTbl.LocationFrom = Request.JourneyDetail[i].LocationFrom;
                    bookingDetailTbl.LocationTo = Request.JourneyDetail[i].LocationTo;
                    bookingDetailTbl.TransportCity = Request.JourneyDetail[i].TransportCity;
                    bookingDetailTbl.TimeFrom = Request.JourneyDetail[i].TimeFrom;
                    bookingDetailTbl.TimeTo = Request.JourneyDetail[i].TimeTo;
                    bookingDetailTbl.ExpireTime = Convert.ToDateTime(bookingDetailTbl.TimeFrom).AddHours(2);
                    bookingDetailTbl.TotalHours = Request.JourneyDetail[i].TotalHours;
                    bookingDetailTbl.TotalPrice = Request.JourneyDetail[i].TotalPrice;
                    bookingDetailTbl.TransportStatus = Request.JourneyDetail[i].TransportStatus;
                    bookingDetailTbl.CarbonEmission = Request.JourneyDetail[i].CarbonEmission;
                    Db.BookingDetailTbls.Add(bookingDetailTbl);
                    Db.SaveChanges();

                    Request.JourneyDetail[i].BookingId = bookingTbl.Id;
                    Request.JourneyDetail[i].Id = bookingDetailTbl.Id;
                    Request.JourneyDetail[i].ExpireTime = bookingDetailTbl.ExpireTime;

                    tripLegs = new TripLegsTbl();
                    tripLegs.BookingId = bookingTbl.Id;
                    tripLegs.TransportMode = Request.JourneyDetail[i].TransportMode;
                    tripLegs.TransportPassNumber = Request.JourneyDetail[i].TransportPassNumber;
                    tripLegs.TransportVehicleNo = Request.JourneyDetail[i].TransportVehicleNo;
                    tripLegs.LocationFrom = Request.JourneyDetail[i].LocationFrom;
                    tripLegs.LocationTo = Request.JourneyDetail[i].LocationTo;
                    tripLegs.TransportCity = Request.JourneyDetail[i].TransportCity;
                    tripLegs.TimeFrom = Request.JourneyDetail[i].TimeFrom;
                    tripLegs.TimeTo = Request.JourneyDetail[i].TimeTo;
                    tripLegs.ExpireTime = Convert.ToDateTime(bookingDetailTbl.TimeFrom).AddHours(2);
                    tripLegs.TotalHours = Request.JourneyDetail[i].TotalHours;
                    tripLegs.TotalPrice = Request.JourneyDetail[i].TotalPrice;
                    tripLegs.TransportStatus = Request.JourneyDetail[i].TransportStatus;
                    tripLegs.CarbonEmission = Request.JourneyDetail[i].CarbonEmission;
                    Db.TripLegsTbls.Add(tripLegs);
                    Db.SaveChanges();

                }


                ValidationResults.AddResult(new ValidationResult
                {
                    ValidationMessage = $"Successfuly Saved"
                });
                resultList.ValidationResults = ValidationResults;
                resultList.JourneyDetail = Request.JourneyDetail;

            }
            catch (Exception ex)
            {
                ValidationResults.AddResult(new ValidationResult
                {
                    ValidationMessage = $"{ex.Message}\n{ex.InnerException?.Message}",
                    ExceptionInformation = $"{ex.StackTrace}\n{ex.InnerException?.StackTrace}"
                });
            }
            return resultList;
        }
        public string UpdateJourneyLeg(int BookingDetailID, string LegStatus)
        {
            try
            {
                Db = new UrbanGoContext();
                var bookingDetailTbl = Db.BookingDetailTbls.First(g => g.Id == BookingDetailID);
                bookingDetailTbl.TransportStatus = LegStatus;
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                return $"{ex.Message}\n{ex.InnerException?.Message}";
            }
            return "success";
        }

        public string UpdateJourneyStatus(int BookingID, string JourneyStatus)
        {
            try
            {
                Db = new UrbanGoContext();
                var bookingTbl = Db.BookingTbls.First(g => g.Id == BookingID);
                bookingTbl.TransportStatus = JourneyStatus;
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                return $"{ex.Message}\n{ex.InnerException?.Message}";
            }
            return "success";
        }

        public GetJourneyByUserResponse GetJourneyByUser(GetJourneyByUserRequest Request, ValidationResults ValidationResults)
        {
            Db = new UrbanGoContext();

            GetJourneyByUserResponse resultList = new GetJourneyByUserResponse();

            try
            {
                if (Request.FromDate != null)
                    resultList.Journey = JsonConvert.DeserializeObject<List<Booking>>(JsonConvert.SerializeObject(Db.BookingTbls.Where(g => g.UserId == Request.UserID && g.TimeFrom.Value.Date >= Request.FromDate && g.TimeFrom.Value.Date <= Request.ToDate).OrderByDescending(c => c.Id).ToList()));
                else
                    resultList.Journey = JsonConvert.DeserializeObject<List<Booking>>(JsonConvert.SerializeObject(Db.BookingTbls.Where(g => g.UserId == Request.UserID).OrderByDescending(c => c.Id).ToList()));
              
                if (resultList.Journey != null)
                {
                    //resultList.Journey[i].mspdetail = JsonConvert.DeserializeObject<List<msp>>(JsonConvert.SerializeObject((from detail in mspResponse
                    //                                                                                                        group detail by new { detail.type, detail.mspname } into data
                    //                                                                                                        select new
                    //                                                                                                        {
                    //                                                                                                            mspname = data.Key.mspname,
                    //                                                                                                            type = data.Key.type,
                    //                                                                                                            carbonembission = data.Sum(c => c.carbonembission)
                    //                                                                                                        }).ToList()));

                    if (Request.FromDate != null)
                    {
                        var mspResponse = (from msp in Db.MspTbls
                                           join bd in Db.BookingDetailTbls on msp.TransportMode equals bd.TransportMode
                                           join tm in Db.TransportModeTbls on msp.TransportMode equals tm.Id
                                           join bm in Db.BookingTbls on bd.BookingId equals bm.Id
                                           where bm.UserId == Request.UserID
                                           && bm.TimeFrom.Value.Date >= Request.FromDate && bm.TimeFrom.Value.Date <= Request.ToDate
                                           select new
                                           {
                                               mspname = msp.MspName,
                                               type = tm.TransportMode,
                                               carbonembission = bd.CarbonEmission,
                                               bookingId = bm.Id
                                           }).ToList();
                        if (mspResponse != null)
                        {
                            for (int i = 0; i < resultList.Journey.Count; i++)
                            {
                                resultList.Journey[i].mspdetail = JsonConvert.DeserializeObject<List<msp>>(JsonConvert.SerializeObject(mspResponse.Where(c => c.bookingId == resultList.Journey[i].Id)));
                            }
                        }
                    }
                    else
                    {
                        var mspResponse = (from msp in Db.MspTbls
                                           join bd in Db.BookingDetailTbls on msp.TransportMode equals bd.TransportMode
                                           join tm in Db.TransportModeTbls on msp.TransportMode equals tm.Id
                                           join bm in Db.BookingTbls on bd.BookingId equals bm.Id
                                           where bm.UserId == Request.UserID
                                           select new
                                           {
                                               mspname = msp.MspName,
                                               type = tm.TransportMode,
                                               carbonembission = bd.CarbonEmission,
                                               bookingId = bm.Id
                                           }).ToList();
                        if (mspResponse != null)
                        {
                            for (int i = 0; i < resultList.Journey.Count; i++)
                            {
                                resultList.Journey[i].mspdetail = JsonConvert.DeserializeObject<List<msp>>(JsonConvert.SerializeObject(mspResponse.Where(c => c.bookingId == resultList.Journey[i].Id)));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ValidationResults.AddResult(new ValidationResult
                {
                    ValidationMessage = $"{ex.Message}\n{ex.InnerException?.Message}",
                    ExceptionInformation = $"{ex.StackTrace}\n{ex.InnerException?.StackTrace}"
                });
            }
            return resultList;
        }

        public GetJourneyByUserResponse GetJourneyDetailByJourneyID(GetJourneyByUserRequest Request, ValidationResults ValidationResults)
        {
            Db = new UrbanGoContext();

            GetJourneyByUserResponse resultList = new GetJourneyByUserResponse();

            try
            {
                var result = (from BD in Db.BookingDetailTbls
                              join T in Db.TransportModeTbls on BD.TransportMode equals T.Id
                              let TransportModeID = T.Id
                              join msp in Db.MspTbls on BD.TransportMode equals msp.TransportMode
                              select new { BD.Id, BD.BookingId, T.TransportMode, BD.TransportPassNumber, BD.TransportVehicleNo, BD.LocationFrom, BD.LocationTo, BD.TimeFrom, BD.TimeTo, BD.TransportCity, BD.TotalHours, BD.TotalPrice, BD.TransportStatus, TransportModeID, BD.CarbonEmission, msp.MspName, msp.MspLogo })
                               .Where(g => g.BookingId == Request.BookingID).ToList();

                resultList.JourneyDetail = JsonConvert.DeserializeObject<List<BookingDetail>>(JsonConvert.SerializeObject(result));

                resultList.Journey = JsonConvert.DeserializeObject<List<Booking>>(JsonConvert.SerializeObject(Db.BookingTbls.Where(p => p.Id == Convert.ToInt32(Request.BookingID)).ToList()));

                if (resultList.Journey != null)
                {
                    var mspResponse = (from msp in Db.MspTbls
                                       join bd in Db.BookingDetailTbls on msp.TransportMode equals bd.TransportMode
                                       join tm in Db.TransportModeTbls on msp.TransportMode equals tm.Id
                                       join bm in Db.BookingTbls on bd.BookingId equals bm.Id
                                       where bm.Id == Convert.ToInt32(Request.BookingID)
                                       select new
                                       {
                                           mspname = msp.MspName,
                                           type = tm.TransportMode,
                                           carbonembission = bd.CarbonEmission,
                                           bookingId = bm.Id
                                       }).ToList();
                    if (mspResponse != null)
                    {
                        for (int i = 0; i < resultList.Journey.Count; i++)
                        {
                            resultList.Journey[i].mspdetail = JsonConvert.DeserializeObject<List<msp>>(JsonConvert.SerializeObject(mspResponse.Where(c => c.bookingId == resultList.Journey[i].Id)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ValidationResults.AddResult(new ValidationResult
                {
                    ValidationMessage = $"{ex.Message}\n{ex.InnerException?.Message}",
                    ExceptionInformation = $"{ex.StackTrace}\n{ex.InnerException?.StackTrace}"
                });
            }
            return resultList;
        }

        public GetUserByKrypcAccountIDResponse GetUserByKrypcUserId(int userId)
        {
            Db = new UrbanGoContext();
            GetUserByKrypcAccountIDResponse user = new GetUserByKrypcAccountIDResponse();
            try
            {
                var response = Db.UserTbls.Where(c => c.UserId == userId).FirstOrDefault();
                user.user = response;
                if (user != null)
                {
                    user = GetUserBalance(user);
                }
            }
            catch (Exception ex)
            {
                user.validationResults.AddResult(new ValidationResult
                {
                    ValidationMessage = ex.Message
                });
            }
            return user;
        }

        private GetUserByKrypcAccountIDResponse GetUserBalance(GetUserByKrypcAccountIDResponse user)
        {
            var points = Db.PaymentTransactionTbls.Where(x => x.TransactionId == null && x.ReceiverAccountId != null && (x.ReceiverAccountId == user.user.AccountId || x.SenderAcountId == user.user.AccountId)).ToList();
            foreach (var item in points)
            {
                if (item.ReceiverAccountId == user.user.AccountId)
                {
                    if (item.TokenId == 1)
                    {
                        user.user.GreenPoint += item.TransactionAmount;
                    }
                    else
                    {
                        user.user.WalletAmount += item.TransactionAmount;
                    }
                }
                else if (item.SenderAcountId == user.user.AccountId)
                {
                    if (item.TokenId == 1)
                    {
                        user.user.GreenPoint -= item.TransactionAmount;
                    }
                    else
                    {
                        user.user.WalletAmount -= item.TransactionAmount;
                    }
                }
            }
            if (user.user.WalletAmount < 0)
                user.user.WalletAmount = 0;
            if (user.user.GreenPoint < 0)
                user.user.GreenPoint = 0;

            return user;
        }

        public GetUserByKrypcAccountIDResponse GetUserByKrypcUsername(string userName)
        {
            Db = new UrbanGoContext();
            GetUserByKrypcAccountIDResponse user = new GetUserByKrypcAccountIDResponse();
            try
            {
                var response = Db.UserTbls.Where(c => c.UserName == userName).FirstOrDefault();
                user.user = response;
                if (user != null)
                {
                    user = GetUserBalance(user);
                }
            }
            catch (Exception ex)
            {
                user.validationResults.AddResult(new ValidationResult
                {
                    ValidationMessage = ex.Message
                });
            }
            return user;
        }
        public GetMSPByTransportResponse GetMSPByKrypcTransport(int TMode)
        {
            Db = new UrbanGoContext();
            GetMSPByTransportResponse getMSP = new GetMSPByTransportResponse();
            try
            {
                var response = Db.MspTbls.Where(c => c.TransportMode == TMode).FirstOrDefault();
                getMSP.msp = response;
            }
            catch (Exception ex)
            {
                getMSP.validationResults.AddResult(new ValidationResult
                {
                    ValidationMessage = ex.Message
                });
            }
            return getMSP;
        }
        public GetFungibleTokenResponse GetFungibleToken(string tokenName)
        {
            Db = new UrbanGoContext();
            GetFungibleTokenResponse getFungibleToken = new GetFungibleTokenResponse();
            try
            {
                var response = Db.FungibleTokenTbls.Where(c => c.TokenName == tokenName).FirstOrDefault();
                getFungibleToken.fungibleToken = response;
            }
            catch (Exception ex)
            {
                getFungibleToken.validationResult.AddResult(new ValidationResult
                {
                    ValidationCode = 500,
                    ValidationMessage = ex.StackTrace.ToString()
                });
            }
            return getFungibleToken;
        }
        public ValidationResults CreateKrypcToken(string token)
        {
            Db = new UrbanGoContext();
            ValidationResults validationResult = new ValidationResults();
            try
            {
                KrypcTokenTbl krypcToken = new KrypcTokenTbl();
                krypcToken = Db.KrypcTokenTbls.Where(c => c.Id == 1).FirstOrDefault();
                if (krypcToken == null)
                {
                    krypcToken = new KrypcTokenTbl();
                    krypcToken.Id = 1;
                    krypcToken.Token = token;
                    Db.KrypcTokenTbls.Add(krypcToken);
                    Db.SaveChanges();
                }
                else
                {
                    krypcToken.Token = token;
                    Db.Entry(krypcToken).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    Db.SaveChanges();
                }
                validationResult.AddResult(new ValidationResult
                {
                    ValidationCode = 200,
                    ValidationMessage = "success"
                });
            }
            catch (Exception ex)
            {
                validationResult.AddResult(new ValidationResult
                {
                    ValidationCode = 500,
                    ValidationMessage = ex.StackTrace.ToString()
                });
            }
            return validationResult;
        }
        public ValidationResults GetKrypcToken()
        {
            Db = new UrbanGoContext();
            ValidationResults validationResult = new ValidationResults();
            try
            {
                var response = Db.KrypcTokenTbls.Where(c => c.Id == 1).FirstOrDefault();
                if (response == null)
                {
                    validationResult.AddResult(new ValidationResult
                    {
                        ValidationCode = 400,
                        ValidationMessage = "Token not found"
                    });
                }
                else
                {
                    validationResult.AddResult(new ValidationResult
                    {
                        ValidationCode = 200,
                        ValidationMessage = response.Token
                    });
                }
            }
            catch (Exception ex)
            {
                validationResult.AddResult(new ValidationResult
                {
                    ValidationCode = 500,
                    ValidationMessage = ex.StackTrace.ToString()
                });
            }
            return validationResult;
        }
        public ValidationResults AddPaymentTransactionHistory(PaymentTransactionRequest request)
        {
            ValidationResults validationResults = new ValidationResults();
            Db = new UrbanGoContext();
            try
            {
                PaymentTransactionTbl historyTbl = new PaymentTransactionTbl();
                historyTbl = JsonConvert.DeserializeObject<PaymentTransactionTbl>(JsonConvert.SerializeObject(request));
                Db.PaymentTransactionTbls.Add(historyTbl);
                Db.SaveChanges();

                    UserTbl user = new UserTbl();
                    user = Db.UserTbls.Where(c => c.UserId == request.UserId).FirstOrDefault();
                //if (user != null)
                //{
                //    if (request.TransactionType == "Trip" || request.TransactionType == "Money Deducted")
                //    {
                //        user.WalletAmount = (user.WalletAmount == null ? 0 : user.WalletAmount) - request.TransactionAmount;
                //    }
                //    else if (request.TransactionType == "Top Up" || request.TransactionType == "Money Added")
                //    {
                //        user.WalletAmount = (user.WalletAmount == null ? 0 : user.WalletAmount) + request.TransactionAmount;
                //    }
                //    else if (request.TransactionType == "Reward")
                //    {
                //        user.GreenPoint = (user.GreenPoint == null ? 0 : user.GreenPoint) + request.TransactionAmount;
                //    }
                //    Db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                //    Db.SaveChanges();
                //}
                validationResults.AddResult(new ValidationResult
                {
                    ValidationCode = 200,
                    ValidationMessage = "success"
                });
            }
            catch (Exception ex)
            {
                validationResults.AddResult(new ValidationResult
                {
                    ValidationCode = 500,
                    ValidationMessage = ex.StackTrace.ToString()
                });
            }
            return validationResults;
        }

    }
}
