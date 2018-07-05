using System;
using Microsoft.AspNetCore.Http;

namespace Taxology.Site.Controllers
{
    public class AnoymousIdHandler
    {
        string anonCart = "anoncart";
        private readonly IHttpContextAccessor contextAccessor;

        public AnoymousIdHandler(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public void RemoveAnonymousId()
        {
            contextAccessor.HttpContext.Response.Cookies.Delete(anonCart);
        }

        public Guid GetAnonId()
        {
            if (contextAccessor.HttpContext.Request.Cookies.ContainsKey(anonCart))
            {
                var cookie = contextAccessor.HttpContext.Request.Cookies[anonCart];

                return Guid.Parse(cookie);
            }

            else
            {
                Guid anonId = Guid.NewGuid();
                contextAccessor.HttpContext.Response.Cookies.Append(anonCart, anonId.ToString(), new CookieOptions
                {
                    //   Domain = "taxology.com",
                    Expires = DateTimeOffset.Now.AddDays(30),
                    Path = "/",
                    HttpOnly = false,
                    Secure = false
                });

                return anonId;
            }
        }
    }
}