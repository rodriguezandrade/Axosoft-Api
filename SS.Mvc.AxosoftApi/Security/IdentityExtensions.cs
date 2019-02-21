using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using SS.Mvc.AxosoftApi.Model;

namespace SS.Mvc.AxosoftApi.Security
{
    public static class IdentityExtensions
    {
        public static string GetRole(this IIdentity identity)
        {
            if (identity == null) throw new ArgumentNullException(nameof(identity));

            return GetRoles(identity)
                .FirstOrDefault();
        }

        public static IEnumerable<string> GetRoles(this IIdentity identity)
        {
            if (identity == null) throw new ArgumentNullException(nameof(identity));
            var claimsIdentity = identity as ClaimsIdentity;

            return claimsIdentity?.Claims.Where(x => x.Type == claimsIdentity.RoleClaimType)
                .Select(x => x.Value);
        }

        public static bool IsInRole(this IIdentity identity, string role)
        {
            if (identity == null) throw new ArgumentNullException(nameof(identity));
            if (role == null) throw new ArgumentNullException(nameof(role));
            if (role == string.Empty) throw new ArgumentException();

            return identity.GetRoles().Any(x => string.Equals(x, role, StringComparison.OrdinalIgnoreCase));
        }

        public static string GetName(this IIdentity identity)
        {
            if (identity == null) throw new ArgumentNullException(nameof(identity));

            return GetValue(identity, ClaimTypes.GivenName);
        }

        public static void SetName(this IIdentity identity, string name)
        {
            if (identity == null) throw new ArgumentNullException(nameof(identity));

            SetValue(identity, ClaimTypes.GivenName, name);
        }

        private static void SetValue(IIdentity identity, string key, string value)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                if (value != null)
                {
                    claimsIdentity.AddClaim(new Claim(key, value));
                }
                else
                {
                    var existingClaim = claimsIdentity.FindFirst(x => x.Type == key);
                    if (existingClaim != null)
                    {
                        claimsIdentity.RemoveClaim(existingClaim);
                    }
                }
            }
        }

        private static string GetValue(IIdentity identity, string key)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var value = claimsIdentity?.FindFirstValue(key);
            return value;
        }
    }
}