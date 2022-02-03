using Core;
using JourneyWebHost.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UG.Journey.JourneyManager.Client;
using UG.Journey.JourneyWebApp.Controllers;
using UG.Journey.JourneyManager.Contracts;
using Microsoft.AspNetCore.Http.Extensions;

namespace JourneyWebHost.Controllers
{
    [Route("JourneyManager")]
    [ApiController]
    public class JourneyManagerController : Controller
    {
        UserResponse _UserResponse;
        JourneyResponse _JourneyResponse;
        GetJourneyByUser _GetJourneyByUser;

        [HttpGet]
        [Route("Journeyfunction")]
        public async Task<IActionResult> Index()
        {
            JourneyManagerLamda.JourneyLamda journeyManager = new JourneyManagerLamda.JourneyLamda();
            string response = await journeyManager.Journeyfunction();
            return View();
        }



        [HttpPost]
        //[ServiceFilter(typeof(CustomAuthorization))]
        [Route("SearchJourneyPlans")]
        public async Task<IActionResult> SearchJourneyPlansAsync(GetJourneyInfoRequest getJourneyInfoRequest)
        {
            _UserResponse = new UserResponse();
            getJourneyInfoRequest.RequestUrl = HttpContext.Request.GetDisplayUrl();

            var JourneyServiceManagerClient = IocManager.Resolve<IJourneyServiceManagerClient>();
            var response = await JourneyServiceManagerClient.GetJourneyInfo(getJourneyInfoRequest);

            if (response.DetailRoutes == null || response.DetailRoutes.Count == 0)
            {
                _UserResponse.status = false;
                _UserResponse.message = "No route found for the selected location or the selected transport mode.";
            }
            else
            {
                _UserResponse.status = true;
                _UserResponse.message = "Success";
            }
            _UserResponse.getJourneyInfoResponse = response;

            return Ok(_UserResponse);
        }

        [HttpPost]
        [ServiceFilter(typeof(CustomAuthorization))]
        [Route("CreateJourney")]
        public IActionResult CreateJourney(CreateJourneyRequest getJourneyInfoRequest)
        {
            _JourneyResponse = new JourneyResponse();
            var JourneyServiceManagerClient = IocManager.Resolve<IJourneyServiceManagerClient>();
            var response = JourneyServiceManagerClient.CreateJourney(getJourneyInfoRequest);

            if (response.JourneyDetail == null || response.JourneyDetail.Count == 0)
            {
                _JourneyResponse.status = false;
                _JourneyResponse.message = response.validationResult.FirstMessage;
            }
            else
            {
                _JourneyResponse.status = true;
                _JourneyResponse.message = "Success";
            }
            _JourneyResponse.createJourneyResponse = response;
            return Ok(_JourneyResponse);
        }

        [HttpPost]
        [ServiceFilter(typeof(CustomAuthorization))]
        [Route("UpdateJourneyLegStaus")]
        public IActionResult UpdateJourneyLegStaus(UpdateJourney updateJourney)
        {
            _JourneyResponse = new JourneyResponse();
            var JourneyServiceManagerClient = IocManager.Resolve<IJourneyServiceManagerClient>();
            var response = JourneyServiceManagerClient.UpdateJourneyLeg(updateJourney.ID, updateJourney.Status);
            if (response.ToLower() == "success")
            {
                _JourneyResponse.status = true;
                _JourneyResponse.message = response;
            }
            else
            {
                _JourneyResponse.status = false;
                _JourneyResponse.message = "Journey Leg has not been updated";
            }
            return Ok(_JourneyResponse);
        }

        [HttpPost]
        [ServiceFilter(typeof(CustomAuthorization))]
        [Route("UpdateJourneyStatus")]
        public IActionResult UpdateJourneyStatus(UpdateJourney updateJourney)
        {
            _JourneyResponse = new JourneyResponse();
            var JourneyServiceManagerClient = IocManager.Resolve<IJourneyServiceManagerClient>();
            var response = JourneyServiceManagerClient.UpdateJourneyStatus(updateJourney.ID, updateJourney.Status);

            if (response.ToLower() == "success")
            {
                _JourneyResponse.status = true;
                _JourneyResponse.message = response;
            }
            else
            {
                _JourneyResponse.status = false;
                _JourneyResponse.message = "Journey has been not updated";
            }
            return Ok(_JourneyResponse);
        }

        [HttpPost]
        [ServiceFilter(typeof(CustomAuthorization))]
        [Route("GetUserJourney")]
        public IActionResult GetJourneyByUser(GetJourneyByUserRequest getJourneyByUserRequest) 
        {
            _GetJourneyByUser = new GetJourneyByUser();
            var JourneyServiceManagerClient = IocManager.Resolve<IJourneyServiceManagerClient>();
            var response = JourneyServiceManagerClient.GetJourneyByUser(getJourneyByUserRequest);
            if (response.Journey == null || response.Journey.Count == 0)
                _GetJourneyByUser.status = false;
            else
                _GetJourneyByUser.status = true;
            _GetJourneyByUser.message = "Success";
            _GetJourneyByUser.getJourneyByUserResponse = response;
            return Ok(_GetJourneyByUser);
        }

        [HttpPost]
        [ServiceFilter(typeof(CustomAuthorization))]
        [Route("GetUserJourneyDetail")]
        public IActionResult GetJourneyDetail(GetJourneyByUserRequest getJourneyByUserRequest)
        {
            _GetJourneyByUser = new GetJourneyByUser();
            var JourneyServiceManagerClient = IocManager.Resolve<IJourneyServiceManagerClient>();
            var response = JourneyServiceManagerClient.GetJourneyDetail(getJourneyByUserRequest);
            _GetJourneyByUser.message = "Success";
            _GetJourneyByUser.getJourneyByUserResponse = response;
            if (response.Journey == null || response.Journey.Count == 0)
                _GetJourneyByUser.status = false;
            else
            {
                _GetJourneyByUser.status = true;
                string Url = HttpContext.Request.GetDisplayUrl();
                for (int i = 0; i < _GetJourneyByUser.getJourneyByUserResponse.JourneyDetail.Count; i++)
                {
                    if (_GetJourneyByUser.getJourneyByUserResponse.JourneyDetail[i].MspName != null)
                    {
                        _GetJourneyByUser.getJourneyByUserResponse.JourneyDetail[i].MspName = _GetJourneyByUser.getJourneyByUserResponse.JourneyDetail[i].MspName;
                        _GetJourneyByUser.getJourneyByUserResponse.JourneyDetail[i].MspLogo = _GetJourneyByUser.getJourneyByUserResponse.JourneyDetail[i].MspLogo;
                        string url = Url.Replace("JourneyManager/GetUserJourneyDetail", "");
                        _GetJourneyByUser.getJourneyByUserResponse.JourneyDetail[i].MspLogo = _GetJourneyByUser.getJourneyByUserResponse.JourneyDetail[i].MspLogo.Replace("../", "");
                        _GetJourneyByUser.getJourneyByUserResponse.JourneyDetail[i].MspLogo = url + _GetJourneyByUser.getJourneyByUserResponse.JourneyDetail[i].MspLogo;
                    }
                }
            }
            return Ok(_GetJourneyByUser);
        }

    }
}
