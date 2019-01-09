using BusinessLayer.Repos;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AjoorLiteWebAPI.Core
{
    public class BasicAuthentication : AuthorizationFilterAttribute
    {
        static SubAdminRepo _SubAdminRepo = new SubAdminRepo();
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.Request.Headers == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
                else
                {
                    string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                    string decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                    string[] usernameAndPassword = decodedAuthenticationToken.Split(':');
                    string Username = usernameAndPassword[0];
                    string Password = usernameAndPassword[1];

                    if (_SubAdminRepo.ValidateUserCredentials(Username, Password))
                    {
                        Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
                    }
                    else
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                }
            }
            catch (Exception ex)
            {
                //return Request.CreateResponse(HttpStatusCode.InternalServerError, $"An error has occured. Error details: {ex.Message}");
            }
        }
    }
}