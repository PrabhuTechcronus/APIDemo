using JourneyWebHost.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using UG.Identity.UserManager.Contracts;
using UG.Identity.UserManager.Client;

namespace UG.Journey.JourneyWebApp.Controllers
{
    //[AttributeUsage(AttributeTargets.Class)]
    public class CustomAuthorization : Attribute, IAuthorizationFilter
    {
        public IConfiguration Configuration { get; set; }
        public CustomAuthorization (IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext != null)
            {
                Microsoft.Extensions.Primitives.StringValues authTokens;
                filterContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authTokens);

                var _token = authTokens.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(_token))
                {
                    if (!ValidateToken(_token.Replace("Bearer","").Trim()))
                    {
                        filterContext.Result = new JsonResult("NotAuthorized")
                        {
                            Value = new UserResponse
                            {
                                status = false,
                                message = "401 The token is expired or invalid"
                            },
                            StatusCode = 401
                        };
                    }
                }
                else
                {
                    filterContext.Result = new JsonResult("NotAuthorized")
                    {
                        Value = new UserResponse
                        {
                            status = false,
                            message = "401 The token is expired or invalid"
                        },
                        StatusCode = 401
                    };
                }
            }
        }

        private bool ValidateToken(string authToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters();
                SecurityToken validatedToken;
                IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);

                var claimPrincipal = getPrincipalFromExpiredToken(authToken);
                var username = claimPrincipal.Claims.FirstOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                GetUserSigninRequest userRequest = new GetUserSigninRequest();
                userRequest.EmailId = username;

                var IdentityServiceManagerClient = Core.IocManager.Resolve<Identity.UserManager.Client.IIdentityServiceManagerClient>();
                var user = IdentityServiceManagerClient.GetIdentityInfoByUsername(userRequest);


                if (user.Token != authToken) return false;
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        private ClaimsPrincipal getPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "urbango.co",
                ValidAudience = "urbango.co",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("jPZZb8leFGe5RhsDK6lDB19DRUorADQNfnMa77btdJk=")),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["JwtToken:issuer"], 
                ValidAudience = Configuration["JwtToken:audience"],
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtToken:secretKey"]))
            };
        }
    }
}
