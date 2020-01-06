using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using LicenseManagement.Models;
using LicenseManagement.Models.Entity;
using LicenseManagement.Services.Account.CustomUser;
using LicenseManagement.Services.Logger;

namespace LicenseManagement.Providers
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

        //public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        //{
        //    var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

        //    ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

        //    if (user == null)
        //    {
        //        context.SetError("invalid_grant", "The user name or password is incorrect.");
        //        return;
        //    }

        //    ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
        //       OAuthDefaults.AuthenticationType);
        //    ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
        //        CookieAuthenticationDefaults.AuthenticationType);

        //    AuthenticationProperties properties = CreateProperties(user.UserName);
        //    AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
        //    context.Validated(ticket);
        //    context.Request.Context.Authentication.SignIn(cookiesIdentity);
        //}

        #region Custom GrantResourceOwnerCredentials
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                var customUserService = new CustomUserService();
                var userLogin = customUserService.GetUserByUserNameAndPassword(context.UserName, context.Password);

                if (userLogin == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, Convert.ToString(userLogin.Id)),
                new Claim(ClaimTypes.Name, userLogin.UserName)
            };

                foreach (var role in userLogin.ListRoles)
                    claims.Add(new Claim(ClaimTypes.Role, role));

                var data = new Dictionary<string, string>
            {
                { "userName", userLogin.UserName },
                { "roles", string.Join(",",userLogin.ListRoles) }
            };

                ClaimsIdentity oAuthIdentity = new ClaimsIdentity(claims,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookiesIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = CreatePropertiesCustom(data);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                try
                {
                    //string requestUri = context.Request.Host.Value;
                    //var logger = new Logger();
                    //if (logger != null)
                    //{
                    //    logger.Info("Success -- GenerateToken", $"Request from: {requestUri}");
                    //}
                }
                catch (Exception ex)
                { }
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            });
        }
        #endregion

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
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

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }

        #region Custom AuthenticationProperties
        public static AuthenticationProperties CreatePropertiesCustom(Dictionary<string, string> data)
        {
            return new AuthenticationProperties(data);
        }
        #endregion
    }
}