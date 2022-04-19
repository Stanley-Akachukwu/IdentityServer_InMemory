using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;
using System.Security.Claims;
using System.Text.Json;

namespace IdentityServer;

public static class Config
{
    
    
    public static IEnumerable<IdentityResource> IdentityResources =>
       new List<IdentityResource>
       {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource()
            {
                Name = "verification",
                UserClaims = new List<string>
                {
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified
                }
            },
            new IdentityResource
            {
              Name = "role",
              UserClaims = new List<string> {"role"}
            }
       };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("phaseCreditAPI", "PhaseCreditAPI")
        };
   

    public static IEnumerable<ApiResource> ApiResources => new[]
    {
      new ApiResource("phaseCreditAPI")
      {
        Scopes = new List<string> { "phaseCreditAPI"},
        ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
        UserClaims = new List<string> {"role"}
      }
    };
    public static IEnumerable<Client> Clients =>
     new[]
     {
        // m2m client credentials flow client
        new Client
        {
          ClientId = "PhaseCreditAPIOAuthclient",//m2m means machine to machine
          ClientName = "PhaseCreditAPI",

          AllowedGrantTypes = GrantTypes.ClientCredentials,
          ClientSecrets = {new Secret("PhaseCreditAPIO_01_Authclient".Sha256())},

          AllowedScopes = { "phaseCreditAPI" }
        },

        // interactive client using code flow + pkce
        new Client
        {
          ClientId = "PhaseCreditWebOidcClient",
          ClientSecrets = {new Secret("PhaseCreditWeb_02_OidcClient".Sha256())},

          AllowedGrantTypes = GrantTypes.Code,


          RedirectUris = {"https://localhost:5444/signin-oidc"},
          FrontChannelLogoutUri = "https://localhost:5444/signout-oidc",
          PostLogoutRedirectUris = {"https://localhost:5444/signout-callback-oidc"},

          AllowOfflineAccess = true,
           AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "verification","role"
                }
        },
     };
    
    
}