using AjoorLiteWebAPI.Core;
using BusinessLayer.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Security;

namespace AjoorLiteWebAPI.Controllers
{
    public class SecurityController : ApiController
    {
        static SubAdminRepo _SubAdminRepo = new SubAdminRepo();

        [HttpPost]
        [Route("api/security/login")]
        //[BasicAuthentication]
        public HttpResponseMessage Login(LOGIN loginDetails)
        {
            try
            {
                if (_SubAdminRepo.ValidateUserCredentials(loginDetails.Username, loginDetails.Password))
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Username/Password mismatch.");
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"An error has occured. Error details: {ex.Message}");
            }
        }
    }

    public class LOGIN
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
