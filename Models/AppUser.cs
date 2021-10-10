using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;

namespace SimpleDotnetMvc.Models
{
    public class AppUser
    {
        private readonly ClaimsPrincipal user;
        private readonly List<Claim> claims;
        public AppUser(ClaimsPrincipal user)
        {
            this.user = user;
            this.claims = user.Claims.ToList();
        }

        public string GetId()
        {
            return GetClaim(ClaimConstants.ObjectId);
        }

        public string GetEmail()
        {
            return GetClaim("preferred_username");
        }

        public string GetName()
        {
            return GetClaim("name");
        }

        private string GetClaim(string key)
        {
            Claim claim = this.claims.Find(claim => claim.Type.Equals(key));
            return claim.Value;
        }
    }
}
