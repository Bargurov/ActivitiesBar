using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Appliction.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security
{
    public class UserAccess : IUsernameAccess
    {
        public IHttpContextAccessor _HttpContextAccessor ;
        public UserAccess(IHttpContextAccessor httpContextAccessor)
        {
            _HttpContextAccessor = httpContextAccessor;

        }

        public string getUsername()
        {
            return _HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }
    }
}