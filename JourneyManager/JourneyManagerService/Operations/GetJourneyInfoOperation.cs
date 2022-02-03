using UG.Journey.JourneyAccess;
using UG.Journey.JourneyAccess.DataAccess;
using UG.Journey.JourneyManager.Contracts;
using Core;
using System.Linq;
using UG.Journey.JourneyAccess.DataAccess.Contracts;
using UG.Journey.JourneyAccess.Client;
using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CreateJourneyRequest = UG.Journey.JourneyManager.Contracts.CreateJourneyRequest;
using CreateJourneyResponse = UG.Journey.JourneyManager.Contracts.CreateJourneyResponse;
using GetJourneyByUserResponse = UG.Journey.JourneyManager.Contracts.GetJourneyByUserResponse;
using GetJourneyByUserRequest = UG.Journey.JourneyManager.Contracts.GetJourneyByUserRequest;
using Newtonsoft.Json;
using RestSharp;
using System.Text;

namespace UG.Journey.JourneyManager.Service.Operations
{
    public interface IGetJourneyInfoOperation
    {
        Task<GetJourneyInfoResponse> GetJourneyInfo(GetJourneyInfoRequest Request);
        CreateJourneyResponse CreateJoureny(CreateJourneyRequest Request);
        string UpdateJourneyLeg(int BookingDetailID, string LegStatus);
        string UpdateJourneyStatus(int BookingID, string JourneyStatus);
        GetJourneyByUserResponse GetJourneyByUser(GetJourneyByUserRequest Request);
        GetJourneyByUserResponse GetJourneyDetail(GetJourneyByUserRequest Request);
    }


    public class GetJourneyInfoOperation : IGetJourneyInfoOperation
    {
        #region Declarations

        private GetJourneyInfoRequest _Request;
        private GetJourneyInfoResponse _Response;
        private CreateJourneyResponse createJourneyResponse;
        private GetJourneyByUserResponse getJourneyByUserResponse;
        public IJourneyDataAccessClient JourneyDataAccessClient;
        private static Random random = new Random();

        #endregion Declarations

        public async Task<GetJourneyInfoResponse> GetJourneyInfo(GetJourneyInfoRequest Request)
        {
            _Request = Request;
            _Response = new GetJourneyInfoResponse();

            await GetRoutesAsync();

            return _Response;
        }

        private async System.Threading.Tasks.Task GetRoutesAsync()
        {
            //if (!_Response.ValidationResults.IsValid) return;
            List<TransportRoutes> lstTransportRoutes = new List<TransportRoutes>();
            List<TransportRoutes> lstRoutesDetails = new List<TransportRoutes>();
            string apiUrl = "https://maps.googleapis.com/maps/api/directions/json?key=AIzaSyAf7d3Tmkdg8_vDEPRYKQwdkxO9XSU3lRY&origin=" + _Request.FromLocation + " &destination=" + _Request.ToLocation + " &sensor=false&mode=transit&alternatives=true&avoid=highways";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var JourneyObj = JObject.Parse(data);
                    //var routes = Convert.ToString(JourneyObj["routes"]);
                    int TotalRoutes = ((Newtonsoft.Json.Linq.JArray)JourneyObj["routes"]).Count;
                    string[] VehicleType = new string[0];

                    if (TotalRoutes != 0)
                   {
                        TransportRoutes transportRoutes;
                        TransportRoutes RouteDetails;

                        for (int i = 0; i <= TotalRoutes - 1; i++)
                        {
                            decimal TotalPrice = 0m;
                            decimal TotalCarbonEmission = 0m;
                            int Steps = ((Newtonsoft.Json.Linq.JArray)JourneyObj["routes"][i]["legs"][0]["steps"]).Count;

                            transportRoutes = new TransportRoutes();
                            transportRoutes.RoutNo = i + 1;
                            transportRoutes.Hours = Convert.ToString(JourneyObj["routes"][i]["legs"][0]["duration"]["value"]);
                            //transportRoutes.HourValue = Convert.ToInt32(JourneyObj["routes"][i]["legs"][0]["duration"]["value"]);
                            transportRoutes.Distance = Convert.ToString(JourneyObj["routes"][i]["legs"][0]["distance"]["text"]);
                            transportRoutes.StartLocation = Convert.ToString(JourneyObj["routes"][i]["legs"][0]["start_address"]);
                            transportRoutes.EndLocation = Convert.ToString(JourneyObj["routes"][i]["legs"][0]["end_address"]);
                            transportRoutes.ArrivalTime = Convert.ToString(JourneyObj["routes"][i]["legs"][0]["arrival_time"]["value"]);
                            transportRoutes.DepartureTime = Convert.ToString(JourneyObj["routes"][i]["legs"][0]["departure_time"]["value"]);

                            VehicleType = new string[Steps];
                            int PreviousArrivalTime = 0;
                            int PreviousDepartureTime = 0;

                            for (int j = 0; j < Steps; j++)
                            {
                                try
                                {
                                    if (Convert.ToString(JourneyObj["routes"][i]["legs"][0]["steps"][j]["travel_mode"]) != "WALKING")
                                    {
                                        VehicleType[j] = JourneyObj["routes"][i]["legs"][0]["steps"][j]["transit_details"]["line"]["vehicle"]["name"].ToString();
                                    }
                                    else VehicleType[j] = "E-Bike";
                                }
                                catch (Exception ex)
                                {
                                    VehicleType[j] = "E-Bike";
                                }
                                transportRoutes.TransportMode = VehicleType[0];
                                transportRoutes.Polyline = Convert.ToString(JourneyObj["routes"][i]["overview_polyline"]["points"]);
                                transportRoutes.CurrencySymbol = "€";

                                JourneyDataAccessClient = IocManager.Resolve<IJourneyDataAccessClient>();

                                var TransportModePrice = JourneyDataAccessClient.GetTransportModePrice(new JourneyRequest
                                {
                                    TransportMode = VehicleType[j]
                                });


                                RouteDetails = new TransportRoutes();

                                RouteDetails.RoutNo = i + 1;
                                RouteDetails.TransportMode = TransportModePrice.TransportModePrice.transportPrice.TransportType;
                                RouteDetails.Hours = Convert.ToString(JourneyObj["routes"][i]["legs"][0]["steps"][j]["duration"]["value"]);
                                RouteDetails.Distance = Convert.ToString(JourneyObj["routes"][i]["legs"][0]["steps"][j]["distance"]["text"]);

                                decimal Distance = Convert.ToDecimal(RouteDetails.Distance.ToLower().Replace("km", "").Replace("mi", "").Replace("m", "").Trim());

                                RouteDetails.CarbonEmission = Distance * Convert.ToDecimal(TransportModePrice.TransportModePrice.transportPrice.Co2PerKm);

                                RouteDetails.StartLocation = Convert.ToString(JourneyObj["routes"][i]["legs"][0]["steps"][j]["html_instructions"]);
                                RouteDetails.Polyline = Convert.ToString(JourneyObj["routes"][i]["legs"][0]["steps"][j]["polyline"]["points"]);
                                RouteDetails.StartLocationLatLong = Convert.ToString(JourneyObj["routes"][i]["legs"][0]["steps"][j]["start_location"]["lat"]) + "," + Convert.ToString(JourneyObj["routes"][i]["legs"][0]["steps"][j]["start_location"]["lng"]);
                                RouteDetails.EndLocationLatLong = Convert.ToString(JourneyObj["routes"][i]["legs"][0]["steps"][j]["end_location"]["lat"]) + "," + Convert.ToString(JourneyObj["routes"][i]["legs"][0]["steps"][j]["end_location"]["lng"]);
                                if (VehicleType[j] != "E-Bike")
                                {
                                    RouteDetails.ArrivalTime = Convert.ToString(JourneyObj["routes"][i]["legs"][0]["steps"][j]["transit_details"]["arrival_time"]["value"]);
                                    RouteDetails.DepartureTime = Convert.ToString(JourneyObj["routes"][i]["legs"][0]["steps"][j]["transit_details"]["departure_time"]["value"]);
                                    RouteDetails.NoOfStops = Convert.ToString(JourneyObj["routes"][i]["legs"][0]["steps"][j]["transit_details"]["num_stops"]);
                                    RouteDetails.StartLocation = Convert.ToString(JourneyObj["routes"][i]["legs"][0]["steps"][j]["transit_details"]["arrival_stop"]["name"]);
                                    RouteDetails.EndLocation = Convert.ToString(JourneyObj["routes"][i]["legs"][0]["steps"][j]["transit_details"]["departure_stop"]["name"]);
                                }
                                else
                                {
                                    int DepartureTime = 0;
                                    int ArrivalTime = 0;

                                    if ((j + 1) < Steps)
                                    {
                                        DepartureTime = Convert.ToInt32(Convert.ToString(JourneyObj["routes"][i]["legs"][0]["steps"][j + 1]["transit_details"]["departure_time"]["value"])) - 120;
                                        ArrivalTime = DepartureTime - Convert.ToInt32(Convert.ToString(JourneyObj["routes"][i]["legs"][0]["steps"][j]["duration"]["value"]));
                                        RouteDetails.DepartureTime = Convert.ToString(DepartureTime);
                                        RouteDetails.ArrivalTime = Convert.ToString(ArrivalTime);

                                        PreviousArrivalTime = ArrivalTime;
                                        PreviousDepartureTime = DepartureTime;
                                    }
                                    else
                                    {
                                        RouteDetails.ArrivalTime = Convert.ToString(PreviousArrivalTime + 120);
                                        RouteDetails.DepartureTime = Convert.ToString(PreviousArrivalTime + 120 + Convert.ToInt32(Convert.ToString(JourneyObj["routes"][i]["legs"][0]["steps"][j]["duration"]["value"])));
                                    }

                                }
                                RouteDetails.CurrencySymbol = "€";
                                //RouteDetails.TransportCharge = Convert.ToDecimal((TransportModePrice.TransportModePrice.PricePerKm) * Convert.ToDecimal(RouteDetails.Distance.ToLower().Replace("km", "").Replace("mi", "").Replace("m", "").Trim()));
                                RouteDetails.TransportCharge = Convert.ToDecimal((TransportModePrice.TransportModePrice.transportPrice.PricePerKm) * Distance);

                                RouteDetails.TransportModeID = TransportModePrice.TransportModePrice.transportPrice.Id;
                                TotalPrice += RouteDetails.TransportCharge;
                                TotalCarbonEmission += RouteDetails.CarbonEmission;
                                if (TransportModePrice.TransportModePrice.MspName != null)
                                {
                                    RouteDetails.MSPName = TransportModePrice.TransportModePrice.MspName;
                                    RouteDetails.MSPImage = TransportModePrice.TransportModePrice.MspLogo;
                                    string url = _Request.RequestUrl.Replace("JourneyManager/SearchJourneyPlans", "");
                                    url = url.ToLower().Replace("ugjourney.productiondemo.org", "admin.productiondemo.org");
                                    RouteDetails.MSPImage = RouteDetails.MSPImage.Replace("../", "");
                                    RouteDetails.MSPImage = url + RouteDetails.MSPImage;
                                }
                                RouteDetails.TransportVehicleNo = RandomString(3) + "-" + RandomString(5);
                                if (RouteDetails.StartLocation != null)
                                {
                                    RouteDetails.StartLocation = RouteDetails.StartLocation.Replace("Walking", "E-Bike").Replace("Walk", "E-Bike");
                                }
                                if (RouteDetails.EndLocation != null)
                                {
                                    RouteDetails.EndLocation = RouteDetails.EndLocation.Replace("Walking", "E-Bike").Replace("Walk", "E-Bike");
                                }
                                lstRoutesDetails.Add(RouteDetails);
                                _Response.DetailRoutes = lstRoutesDetails;
                            }

                            transportRoutes.TransportCharge = TotalPrice;
                            transportRoutes.CarbonEmission = TotalCarbonEmission;
                            lstTransportRoutes.Add(transportRoutes);

                            //var DistinctVehicleType = VehicleType.Distinct().ToArray();
                        }
                        TransportRouteType journeyInfoResponse = new TransportRouteType();
                        if (_Request.Transportmode != null && _Request.Transportmode.Count != 0)
                        {
                            lstTransportRoutes = (from item in lstTransportRoutes
                                                  where _Request.Transportmode.ToArray().Contains(item.TransportMode)
                                                  select item).ToList();

                            _Response.DetailRoutes = (from item in _Response.DetailRoutes
                                                      where _Request.Transportmode.ToArray().Contains(item.TransportMode)
                                                      select item).ToList();
                        }
                        lstTransportRoutes = lstTransportRoutes.OrderByDescending(x => x.CarbonEmission).ToList();
                        decimal PrevCharge = 0, PrevPoint = 0;
                        for (int i = 0; i < lstTransportRoutes.Count; i++)
                        {
                            if (lstTransportRoutes[i].CarbonEmission != PrevCharge)
                            {
                                if(lstTransportRoutes.Count == 1)
                                    lstTransportRoutes[i].GreenPoint = PrevPoint + 10;
                                else if (i == 0)
                                    lstTransportRoutes[i].GreenPoint = PrevPoint;
                                else
                                    lstTransportRoutes[i].GreenPoint = PrevPoint + 10;
                            }
                            else
                                lstTransportRoutes[i].GreenPoint = PrevPoint;
                            PrevCharge = lstTransportRoutes[i].CarbonEmission;
                            PrevPoint = lstTransportRoutes[i].GreenPoint;
                        }
                        journeyInfoResponse.FastestRoutes = lstTransportRoutes.OrderBy(x => x.TransportCharge).OrderBy(x => x.Hours).ToList();
                        journeyInfoResponse.CheapestRoutes = lstTransportRoutes.OrderBy(x => x.Hours).OrderBy(x => x.TransportCharge).ToList();
                        journeyInfoResponse.GreenestRoutes = lstTransportRoutes.OrderBy(x => x.Hours).OrderByDescending(x => x.GreenPoint).ToList();

                        _Response.TransportRoutes = journeyInfoResponse;
                    }
                }
            }
        }
        public CreateJourneyResponse CreateJoureny(CreateJourneyRequest Request)
        {
            CreateJourneyResponse createJourney = new CreateJourneyResponse();
            createJourney.validationResult = new ValidationResults();

            try
            {
                JourneyDataAccessClient = IocManager.Resolve<IJourneyDataAccessClient>();
                //CreateKrypcTokenRequest createKrypcToken = new CreateKrypcTokenRequest();

                //var userresponse = JourneyDataAccessClient.GetUserByKrypcUserId(Request.Journey.UserId).user;
                //if (userresponse == null)
                //{
                //    createJourney.validationResult.AddResult(new ValidationResult
                //    {
                //        ValidationCode = 400,
                //        ValidationMessage = "user not found."
                //    });
                //    return createJourney;
                //}

                //var tokenResponse = CreateKrypcToken(createKrypcToken);
                //string token = tokenResponse.Result.FirstMessage;

                //var client = new RestClient("https://3.7.251.20/api/submitMessage");
                //client.Timeout = -1;
                //client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                //var request1 = new RestRequest(Method.POST);
                //request1.AddHeader("Authorization", token);
                //request1.AddHeader("Content-Type", "application/json");

                //var krypcMessageRequest = new
                //{
                //    createdBy = "shyam@urbango.mobi",//userresponse.UserName, //
                //    message = JsonConvert.SerializeObject(Request.Journey),
                //    apiMode = true
                //};

                //string str = JsonConvert.SerializeObject(krypcMessageRequest);

                //request1.AddParameter("application/json", str, ParameterType.RequestBody);
                //IRestResponse response = client.Execute(request1);

                //if (response.StatusCode == System.Net.HttpStatusCode.OK)
                //{
                //    string responseString = response.Content.ToString();
                //    if (responseString.Contains("Extra"))
                //    {
                //        var responseBody = JsonConvert.DeserializeObject<GetKrypcMessageResponse>(responseString);
                //        Request.Journey.topicId = responseBody.Extra.topicId;
                //        Request.Journey.transactionId = responseBody.Extra.transactionId;
                //        Request.Journey.sequenceNumber = responseBody.Extra.sequenceNumber;
                //        Request.Journey.networkType = responseBody.Extra.networkType;
                //    }
                //}
                //else
                //{
                //    string responseString = response.Content.ToString();
                //    createJourney.validationResult.AddResult(new ValidationResult
                //    {
                //        ValidationCode = 500,
                //        ValidationMessage = responseString
                //    });
                //    return createJourney;
                //}

                UG.Journey.JourneyAccess.DataAccess.Contracts.CreateJourneyRequest journeyRequest = new UG.Journey.JourneyAccess.DataAccess.Contracts.CreateJourneyRequest();

                journeyRequest.Journey = new JourneyAccess.DataAccess.Models.BookingTbl();
                journeyRequest.JourneyDetail = new List<JourneyAccess.DataAccess.Models.BookingDetailTbl>();

                journeyRequest.Journey.UserId = Request.Journey.UserId;
                journeyRequest.Journey.LocationFrom = Request.Journey.LocationFrom;
                journeyRequest.Journey.LocationTo = Request.Journey.LocationTo;
                journeyRequest.Journey.TimeFrom = Convert.ToDateTime(Request.Journey.TimeFrom);
                journeyRequest.Journey.TimeTo = Convert.ToDateTime(Request.Journey.TimeTo);
                TimeSpan TotalHours = TimeSpan.Parse(Request.Journey.TotalHours);
                journeyRequest.Journey.TotalHours = TotalHours;
                journeyRequest.Journey.TotalPrice = Request.Journey.TotalPrice;
                journeyRequest.Journey.TransportStatus = Request.Journey.TransportStatus;
                journeyRequest.Journey.CarbonEmission = Request.Journey.CarbonEmission;
                journeyRequest.Journey.GreenPoints = Request.Journey.GreenPoint;
                journeyRequest.Journey.TopicId = Request.Journey.topicId;
                journeyRequest.Journey.TransactionId = Request.Journey.transactionId;
                journeyRequest.Journey.SequenceNumber = Request.Journey.sequenceNumber;
                journeyRequest.Journey.NetworkType = Request.Journey.networkType;

                journeyRequest.JourneyDetail = new List<JourneyAccess.DataAccess.Models.BookingDetailTbl>();
                List<JourneyAccess.DataAccess.Models.BookingDetailTbl> lstbookingDetailTbls = new List<JourneyAccess.DataAccess.Models.BookingDetailTbl>();

                decimal TotalCharge = 0;

                JourneyAccess.DataAccess.Models.BookingDetailTbl bookingDetailTbl;
                for (int i = 0; i <= Request.JourneyDetail.Count - 1; i++)
                {
                    bookingDetailTbl = new JourneyAccess.DataAccess.Models.BookingDetailTbl();
                    bookingDetailTbl.TransportMode = Request.JourneyDetail[i].TransportMode;
                    bookingDetailTbl.TransportPassNumber = Request.JourneyDetail[i].TransportPassNumber;
                    bookingDetailTbl.TransportVehicleNo = Request.JourneyDetail[i].TransportVehicleNo;
                    bookingDetailTbl.LocationFrom = Request.JourneyDetail[i].LocationFrom;
                    bookingDetailTbl.LocationTo = Request.JourneyDetail[i].LocationTo;
                    bookingDetailTbl.TransportCity = Request.JourneyDetail[i].TransportCity;
                    bookingDetailTbl.TimeFrom = Convert.ToDateTime(Request.JourneyDetail[i].TimeFrom);
                    bookingDetailTbl.TimeTo = Convert.ToDateTime(Request.JourneyDetail[i].TimeTo);
                    TimeSpan LegTotalHours = TimeSpan.Parse(Request.JourneyDetail[i].TotalHours);
                    bookingDetailTbl.TotalHours = LegTotalHours;
                    bookingDetailTbl.TotalPrice = Request.JourneyDetail[i].TotalPrice;
                    bookingDetailTbl.TransportStatus = Request.JourneyDetail[i].TransportStatus;
                    bookingDetailTbl.CarbonEmission = Request.JourneyDetail[i].CarbonEmission;

                    if (bookingDetailTbl.TransportPassNumber == string.Empty)
                        bookingDetailTbl.TransportPassNumber = RandomString(4) + "-" + RandomString(4);

                    if (bookingDetailTbl.LocationTo == null)
                    {
                        if((i+1) < Request.JourneyDetail.Count)
                        {
                            bookingDetailTbl.LocationTo = Request.JourneyDetail[i+1].LocationFrom;
                        }
                        else
                        {
                            bookingDetailTbl.LocationTo = journeyRequest.Journey.LocationTo;
                        }
                    }

                    TotalCharge = TotalCharge + (Convert.ToDecimal(bookingDetailTbl.TotalPrice));

                    lstbookingDetailTbls.Add(bookingDetailTbl);
                }

                journeyRequest.JourneyDetail = lstbookingDetailTbls;

                var adminsenderResponse = JourneyDataAccessClient.GetUserByKrypcUsername("shyam@urbango.mobi");
                if (adminsenderResponse == null || adminsenderResponse.user == null)
                {
                    createJourney.validationResult.AddResult(new ValidationResult
                    {
                        ValidationCode = 400,
                        ValidationMessage = "admin account not found"
                    });
                    return createJourney;
                }
                var reciverResponse = JourneyDataAccessClient.GetUserByKrypcUserId(Request.Journey.UserId);
                if (reciverResponse == null || reciverResponse.user == null)
                {
                    createJourney.validationResult.AddResult(new ValidationResult
                    {
                        ValidationCode = 400,
                        ValidationMessage = "user account not found"
                    });
                    return createJourney;
                }
                else
                {
                    if(reciverResponse.user.WalletAmount < TotalCharge)
                    {
                        createJourney.validationResult.AddResult(new ValidationResult
                        {
                            ValidationCode = 400,
                            ValidationMessage = "The required amount is not available is user account."
                        });
                        return createJourney;
                    }
                }

                ////add blue point To Admin on Krypc
                var tokenFunblueResponse = JourneyDataAccessClient.GetFungibleToken("UGBT");
                if (tokenFunblueResponse == null || tokenFunblueResponse.fungibleToken == null)
                {
                    createJourney.validationResult.AddResult(new ValidationResult
                    {
                        ValidationCode = 400,
                        ValidationMessage = "blue token not found"
                    });
                    return createJourney;
                }

                //decimal tokenQty = (TotalCharge * 100);
                //TransferTokenKrypcRequest transferTokenRequest = new TransferTokenKrypcRequest();
                //transferTokenRequest.apiMode = true;
                //transferTokenRequest.createdBy = "ADMIN";// senderResponse.user.EmailId;
                //transferTokenRequest.dateAndTime = journeyRequest.Journey.TimeFrom;
                //transferTokenRequest.offerPrice = 1;
                //transferTokenRequest.quantity = Convert.ToInt64(tokenQty);
                //transferTokenRequest.receiverAccount = adminsenderResponse.user.AccountId;
                //transferTokenRequest.receiverPrivateKey = adminsenderResponse.user.PrivateKey;
                //transferTokenRequest.senderAccount = reciverResponse.user.AccountId;
                //transferTokenRequest.senderPrivateKey = reciverResponse.user.PrivateKey;
                //transferTokenRequest.tokenid = tokenFunblueResponse.fungibleToken.TokenId;

                //var responsedet = TransferKrypcToken(transferTokenRequest, token, false);
                //string adminTokenTransactionId = string.Empty;
                //if (responsedet.IsValid == false)
                //{
                //    createJourney.validationResult.AddResult(new ValidationResult
                //    {
                //        ValidationCode = responsedet.FirstCode,
                //        ValidationMessage = responsedet.FirstMessage
                //    });
                //    return createJourney;
                //}
                //else
                //{
                //    adminTokenTransactionId = responsedet.FirstMessage;
                //}

                //add green point to user on Krypc
                var tokenFunGreenResponse = JourneyDataAccessClient.GetFungibleToken("UGGT");
                if (tokenFunGreenResponse == null || tokenFunGreenResponse.fungibleToken == null)
                {
                    createJourney.validationResult.AddResult(new ValidationResult
                    {
                        ValidationCode = 400,
                        ValidationMessage = "green token not found"
                    });
                    return createJourney;
                }

                //decimal tokenQty1 = Convert.ToDecimal(Request.Journey.GreenPoint) * 100;
                //TransferTokenKrypcRequest transferTokenRequestGreen = new TransferTokenKrypcRequest();
                //transferTokenRequestGreen.apiMode = true;
                //transferTokenRequestGreen.createdBy = "ADMIN";// senderResponse.user.EmailId;
                //transferTokenRequestGreen.dateAndTime = journeyRequest.Journey.TimeFrom;
                //transferTokenRequestGreen.offerPrice = 1;
                //transferTokenRequestGreen.quantity = Convert.ToInt64(tokenQty1);
                //transferTokenRequestGreen.receiverAccount = reciverResponse.user.AccountId;
                //transferTokenRequestGreen.receiverPrivateKey = reciverResponse.user.PrivateKey;
                //transferTokenRequestGreen.senderAccount = adminsenderResponse.user.AccountId;
                //transferTokenRequestGreen.senderPrivateKey = adminsenderResponse.user.PrivateKey;
                //transferTokenRequestGreen.tokenid = tokenFunGreenResponse.fungibleToken.TokenId;

                //var greenresponse = TransferKrypcToken(transferTokenRequestGreen, token, false);
                //string greenTokenTransactionId = string.Empty;
                //if (greenresponse.IsValid == false)
                //{
                //    createJourney.validationResult.AddResult(new ValidationResult
                //    {
                //        ValidationCode = greenresponse.FirstCode,
                //        ValidationMessage = greenresponse.FirstMessage
                //    });
                //    return createJourney;
                //}
                //else
                //{
                //    greenTokenTransactionId = greenresponse.FirstMessage;
                //}

                createJourneyResponse = JsonConvert.DeserializeObject<CreateJourneyResponse>(JsonConvert.SerializeObject(JourneyDataAccessClient.CreateJourney(journeyRequest)));

                if (createJourneyResponse.JourneyDetail.FirstOrDefault().BookingId != 0)
                {
                    //add blue point To admin in DB
                    PaymentTransactionRequest paymentTransaction = new PaymentTransactionRequest();
                    paymentTransaction.BookingId = createJourneyResponse.JourneyDetail.FirstOrDefault().BookingId;
                    paymentTransaction.TokenId = tokenFunblueResponse.fungibleToken.Id;
                    paymentTransaction.TransactionAmount = TotalCharge;
                    paymentTransaction.TransactionDate = createJourneyResponse.JourneyDetail.FirstOrDefault().TimeFrom.Value;
                    paymentTransaction.TransactionType = "Trip";
                    paymentTransaction.UserId = Request.Journey.UserId;
                    paymentTransaction.SenderAcountId = reciverResponse.user.AccountId;
                    paymentTransaction.SenderPublicKey = reciverResponse.user.PublicKey;
                   //paymentTransaction.TransactionId = adminTokenTransactionId;
                    paymentTransaction.BookingDetailId = 0;

                    var result = JourneyDataAccessClient.AddPaymentTransactionHistory(paymentTransaction);
                    if (result.IsValid == false)
                    {
                        createJourney.validationResult.AddResult(new ValidationResult
                        {
                            ValidationCode = result.FirstCode,
                            ValidationMessage = result.FirstMessage
                        });
                        return createJourney;
                    }

                    //add green point to user in DB
                    if (Convert.ToDecimal(Request.Journey.GreenPoint) != 0)
                    {
                        paymentTransaction = new PaymentTransactionRequest();
                        paymentTransaction.BookingId = createJourneyResponse.JourneyDetail.FirstOrDefault().BookingId;
                        paymentTransaction.TokenId = tokenFunGreenResponse.fungibleToken.Id;
                        paymentTransaction.TransactionAmount = Convert.ToDecimal(Request.Journey.GreenPoint);
                        paymentTransaction.TransactionDate = createJourneyResponse.JourneyDetail.FirstOrDefault().TimeFrom.Value;
                        paymentTransaction.TransactionType = "Reward";
                        paymentTransaction.UserId = Request.Journey.UserId;
                        paymentTransaction.ReceiverAccountId = reciverResponse.user.AccountId;
                        paymentTransaction.ReceiverPublicKey = reciverResponse.user.PublicKey;
                        paymentTransaction.SenderAcountId = adminsenderResponse.user.AccountId;
                        paymentTransaction.SenderPublicKey = adminsenderResponse.user.PublicKey;
                        //paymentTransaction.TransactionId = greenTokenTransactionId;
                        paymentTransaction.BookingDetailId = 0;

                        result = JourneyDataAccessClient.AddPaymentTransactionHistory(paymentTransaction);
                        if (result.IsValid == false)
                        {
                            createJourney.validationResult.AddResult(new ValidationResult
                            {
                                ValidationCode = result.FirstCode,
                                ValidationMessage = result.FirstMessage
                            });
                            return createJourney;
                        }
                    }

                    foreach (var item in createJourneyResponse.JourneyDetail)
                    {
                        var mspreceiverResponse = JourneyDataAccessClient.GetMSPByKrypcTransport(Convert.ToInt32(item.TransportMode));
                        if (mspreceiverResponse == null || mspreceiverResponse.msp == null)
                        {
                            createJourney.validationResult.AddResult(new ValidationResult
                            {
                                ValidationCode = mspreceiverResponse.validationResults.FirstCode,
                                ValidationMessage = mspreceiverResponse.validationResults.FirstMessage
                            });
                            return createJourney;
                        }
                        //add blue point to msp in DB
                        TotalCharge = (Convert.ToDecimal(item.TotalPrice));
                        paymentTransaction = new PaymentTransactionRequest();
                        paymentTransaction.BookingId = createJourneyResponse.JourneyDetail.FirstOrDefault().BookingId;
                        paymentTransaction.TokenId = tokenFunblueResponse.fungibleToken.Id;
                        paymentTransaction.TransactionAmount = TotalCharge;
                        paymentTransaction.TransactionDate = createJourneyResponse.JourneyDetail.FirstOrDefault().TimeFrom.Value;
                        paymentTransaction.TransactionType = "Leg Trip";
                        paymentTransaction.UserId = Request.Journey.UserId;
                        paymentTransaction.ReceiverAccountId = mspreceiverResponse.msp.AccountId;
                        paymentTransaction.ReceiverPublicKey = mspreceiverResponse.msp.PublicKey;
                        paymentTransaction.SenderAcountId = reciverResponse.user.AccountId;
                        paymentTransaction.SenderPublicKey = reciverResponse.user.PublicKey;
                        paymentTransaction.MSPId = mspreceiverResponse.msp.Id;
                        paymentTransaction.TransportMode = Convert.ToInt32(item.TransportMode);
                        paymentTransaction.BookingDetailId = item.Id;

                        result = JourneyDataAccessClient.AddPaymentTransactionHistory(paymentTransaction);
                        if (result.IsValid == false)
                        {
                            createJourney.validationResult.AddResult(new ValidationResult
                            {
                                ValidationCode = result.FirstCode,
                                ValidationMessage = result.FirstMessage
                            });
                            return createJourney;
                        }
                    }
                }

                return createJourneyResponse;
            }
            catch (Exception e)
            {
                createJourney.validationResult.AddResult(new ValidationResult
                {
                    ValidationCode = 500,
                    ValidationMessage = e.StackTrace.ToString()
                });
                return createJourney;
            }
        }
        private ValidationResults TransferKrypcToken(TransferTokenKrypcRequest krypcRequest, string token, bool Recall)
        {
            ValidationResults validationResult = new ValidationResults();
            try
            {
                var client = new RestClient("https://test.vaas.krypc.com:8000/api/transferFT");
                client.Timeout = -1;
                client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                var request1 = new RestRequest(Method.POST);
                request1.AddHeader("Authorization", token);
                request1.AddHeader("Content-Type", "application/json");

                string str = JsonConvert.SerializeObject(krypcRequest);

                request1.AddParameter("application/json", str, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request1);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseString = response.Content.ToString();
                    var res = JsonConvert.DeserializeObject<TransferTokenKrypcResponse>(responseString);
                    validationResult.AddResult(new ValidationResult
                    {
                        ValidationCode = 200,
                        ValidationMessage = res.Extra.transactionId
                    });
                }
                else
                {
                    string responseString = response.Content.ToString();
                    //var responseBody = JsonConvert.DeserializeObject<CreateKrypcTokenResponse>(responseString);
                   
                     validationResult.AddResult(new ValidationResult
                    {
                        ValidationCode = (int?)response.StatusCode,
                        ValidationMessage = responseString
                    });
                    if (Recall == false)
                    {
                        Recall = true;
                        TransferKrypcToken(krypcRequest, token, true);
                    }
                }
            }
            catch (Exception e)
            {
                validationResult.AddResult(new ValidationResult
                {
                    ValidationMessage = e.StackTrace.ToString()
                });
            }
            return validationResult;
        }
        public async Task<ValidationResults> CreateKrypcTokenUser(CreateKrypcTokenRequest request)
        {
            ValidationResults validationResult = new ValidationResults();
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                using (HttpClient client = new HttpClient(clientHandler))
                {
                    client.BaseAddress = new Uri("https://3.7.251.20/");

                    HttpRequestMessage requestMessage = new HttpRequestMessage();
                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("api/createToken", requestMessage.Content);
                    string responseString = await response.Content.ReadAsStringAsync();
                    var responseBody = JsonConvert.DeserializeObject<CreateKrypcTokenResponse>(responseString);

                    if (response.IsSuccessStatusCode)
                    {
                        if (responseBody.Extra != null)
                        {
                            validationResult.AddResult(new ValidationResult
                            {
                                ValidationMessage = responseBody.Extra.token
                            });
                            var paymentData = IocManager.Resolve<IJourneyDataAccessClient>();
                            var tokencreate = paymentData.CreateKrypcToken(responseBody.Extra.token);
                        }
                    }
                    else
                    {
                        validationResult.AddResult(new ValidationResult
                        {
                            ValidationCode = (int?)response.StatusCode,
                            ValidationMessage = responseBody.Msg
                        });
                    }
                }
            }
            catch (Exception e)
            {
                validationResult.AddResult(new ValidationResult
                {
                    ValidationMessage = e.Message.ToString()
                });
            }
            return validationResult;
        }
        public async Task<ValidationResults> CreateKrypcToken(CreateKrypcTokenRequest request)
        {
            request.emailId = "shyam@urbango.mobi";
            request.password = "London201$";

            ValidationResults validationResult = new ValidationResults();
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                using (HttpClient client = new HttpClient(clientHandler))
                {
                    client.BaseAddress = new Uri("https://3.7.251.20/");

                    HttpRequestMessage requestMessage = new HttpRequestMessage();
                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("api/createToken", requestMessage.Content);
                    string responseString = await response.Content.ReadAsStringAsync();
                    var responseBody = JsonConvert.DeserializeObject<CreateKrypcTokenResponse>(responseString);

                    if (response.IsSuccessStatusCode)
                    {
                        if (responseBody.Extra != null)
                        {
                            validationResult.AddResult(new ValidationResult
                            {
                                ValidationMessage = responseBody.Extra.token
                            });
                            var paymentData = IocManager.Resolve<IJourneyDataAccessClient>();
                            var tokencreate = paymentData.CreateKrypcToken(responseBody.Extra.token);
                        }
                    }
                    else
                    {
                        validationResult.AddResult(new ValidationResult
                        {
                            ValidationCode = (int?)response.StatusCode,
                            ValidationMessage = responseBody.Msg
                        });
                    }
                }
            }
            catch (Exception e)
            {
                validationResult.AddResult(new ValidationResult
                {
                    ValidationMessage = e.Message.ToString()
                });
            }
            return validationResult;
        }
        public string UpdateJourneyLeg(int BookingDetailID, string LegStatus)
        {
            JourneyDataAccessClient = IocManager.Resolve<IJourneyDataAccessClient>();

            return JourneyDataAccessClient.UpdateJourneyLeg(BookingDetailID, LegStatus);
        }
        public string UpdateJourneyStatus(int BookingID, string JourneyStatus)
        {
            JourneyDataAccessClient = IocManager.Resolve<IJourneyDataAccessClient>();

            return JourneyDataAccessClient.UpdateJourneyStatus(BookingID, JourneyStatus);
        }
        public GetJourneyByUserResponse GetJourneyByUser(GetJourneyByUserRequest Request)
        {
            UG.Journey.JourneyAccess.DataAccess.Contracts.GetJourneyByUserRequest getJourneyByUser = new UG.Journey.JourneyAccess.DataAccess.Contracts.GetJourneyByUserRequest();

            JourneyDataAccessClient = IocManager.Resolve<IJourneyDataAccessClient>();

            getJourneyByUser.UserID = Request.UserID;
            getJourneyByUser.FromDate = Request.FromDate;
            getJourneyByUser.ToDate = Request.ToDate;
            getJourneyByUserResponse = new GetJourneyByUserResponse();
            getJourneyByUserResponse = JsonConvert.DeserializeObject<GetJourneyByUserResponse>(JsonConvert.SerializeObject(JourneyDataAccessClient.GetJourneyByUser(getJourneyByUser)));
            return getJourneyByUserResponse;
        }
        public GetJourneyByUserResponse GetJourneyDetail(GetJourneyByUserRequest Request)
        {
            UG.Journey.JourneyAccess.DataAccess.Contracts.GetJourneyByUserRequest getJourneyByUser = new UG.Journey.JourneyAccess.DataAccess.Contracts.GetJourneyByUserRequest();

            JourneyDataAccessClient = IocManager.Resolve<IJourneyDataAccessClient>();

            getJourneyByUser.BookingID = Request.BookingID;
            getJourneyByUserResponse = new GetJourneyByUserResponse();
            getJourneyByUserResponse = JsonConvert.DeserializeObject<GetJourneyByUserResponse>(JsonConvert.SerializeObject(JourneyDataAccessClient.GetJourneyDetailByJourneyID(getJourneyByUser)));
            return getJourneyByUserResponse;
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }


}
