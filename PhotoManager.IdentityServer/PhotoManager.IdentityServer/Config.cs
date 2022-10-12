using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace PhotoManager.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
                Name = "roles",
                DisplayName = "roles",
                UserClaims = new[] { JwtClaimTypes.Role, JwtClaimTypes.Role },
                ShowInDiscoveryDocument = true,
                Required = true,
            }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                 new ApiScope("api-scope", "api-scope"),
            };
        public static IEnumerable<ApiResource> ApiResources =>
          new List<ApiResource>
          {
               new ApiResource("api-scope", "api-scope")
                {
                    Scopes = { "api-scope" }
                }
          };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
            new Client
            {
                    //ClientName = "web api",
                    //ClientId = "web-api-id",
                    //RedirectUris           = { "https://localhost:5001/swagger/oauth2-redirect.html" },

                    //ClientSecrets = new [] { new Secret("this is my custom Secret key for authnetication".Sha512()) },
                    //AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    //AllowedCorsOrigins = { "https://localhost:5001"},
                    //AllowAccessTokensViaBrowser = true,
                    //AllowedScopes = {
                    //    "api-scope",
                    //    "roles",
                    //     IdentityServerConstants.StandardScopes.OpenId,
                    //     IdentityServerConstants.StandardScopes.Profile,
                    //}
                    ClientSecrets = new [] { new Secret("secret".Sha512()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = 
                    { "api-scope", "roles",
                        IdentityServerConstants.StandardScopes.OpenId, 
                        IdentityServerConstants.StandardScopes.Profile,
                    },
            },

            new Client
            {
                ClientId = "mvc",
                ClientName = "MVC Client",
                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets =    
                {       
                    new Secret("secret".Sha256())    
                },   
                RedirectUris           = { "https://localhost:6001/signin-oidc" },   
                PostLogoutRedirectUris = { "https://localhost:6001/signout-callback-oidc" },   
                AllowedScopes =    
                {      
                    IdentityServerConstants.StandardScopes.OpenId,       
                    IdentityServerConstants.StandardScopes.Profile,
                    "roles"
                },
                AllowAccessTokensViaBrowser = true,
                AlwaysIncludeUserClaimsInIdToken= true
            },            
            new Client
            {
                ClientName = "Client",
                ClientId = "angular-client",
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = new List<string>{ "http://localhost:4200/signin-callback", "http://localhost:4200/assets/silent-callback.html" },
                RequirePkce = true,
                AccessTokenType = AccessTokenType.Jwt,
                AllowAccessTokensViaBrowser = true,
                AlwaysIncludeUserClaimsInIdToken= true,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api-scope",
                    "roles"
                },        
                AllowedCorsOrigins = { "http://localhost:4200"},
                RequireClientSecret = false,
                PostLogoutRedirectUris = new List<string> { "http://localhost:4200/signout-callback" },
                RequireConsent = false,
                AccessTokenLifetime = 86400,
                IdentityTokenLifetime = 86400
            },
        };
    }   
}