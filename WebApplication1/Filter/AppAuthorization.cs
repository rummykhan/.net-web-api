using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using WebApplication1.Filter.Response;

namespace WebApplication1.Filter
{
    public class AppAuthorization : IAuthorizationFilter
    {
        public bool AllowMultiple => throw new NotImplementedException();

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            if (!ShouldVerify(actionContext.Request))
            {
                return continuation();
            }

            string token = GetToken(actionContext.Request.Headers);

            if(VerifyToken(token))
            {
                return continuation();
            }

            var response = actionContext.Request.CreateResponse<ResponseModel>(
                HttpStatusCode.Unauthorized,
                new ResponseModel() { Code=403, Message="Invalid authorization token." }
            );

            Task<HttpResponseMessage> task = new Task<HttpResponseMessage>(delegate () {
                return response;
            });

            task.RunSynchronously();

            return task;
        }

        protected bool ShouldVerify(HttpRequestMessage httpRequest)
        {
            return true;
        }

        protected string GetToken(HttpRequestHeaders headers)
        {
            return null;
        }

        protected bool VerifyToken(string token)
        {
            return false;
        }
    }
}