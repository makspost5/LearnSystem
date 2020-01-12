using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebAPI.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials
        (OAuthGrantResourceOwnerCredentialsContext context)
        {
            int userId = 0;
            string role = "pupil";
            string userName = string.Empty;

            using (var obj = new GraduateWorkEntities())
            {
                var md5 = MD5.Create();
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(context.Password));
                var password = Convert.ToBase64String(hash);

                Person entry = obj.Person.Where(record =>
                record.Email == context.UserName &&
                record.Password == password).FirstOrDefault();

                if (entry == null)
                {
                    context.SetError("invalid_grant",
                    "The user name or password is incorrect.");
                    return;
                }

                if (entry.UserName == "admin" && entry.Email == "admin@admin.admin")
                {
                    role = "admin";
                } else
                {
                    if (entry.Teacher != null)
                    {
                        role = "teacher";
                    }
                }

                userName = entry.UserName;
                userId = entry.PersonID;
            }

            ClaimsIdentity oAuthIdentity =
            new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, userId.ToString()));

            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Role, role));

            ClaimsIdentity cookiesIdentity =
            new ClaimsIdentity(context.Options.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(userName, role);
            AuthenticationTicket ticket =
            new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string,
            string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication
        (OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri
        (OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName, string role = "pupil")
        {
            IDictionary<string, string>
            data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "role", role }
            };
            return new AuthenticationProperties(data);
        }
    }
}