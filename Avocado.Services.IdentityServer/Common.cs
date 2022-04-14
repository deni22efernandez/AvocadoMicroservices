using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Services.IdentityServer
{
	public class Common
	{

		public const string admin = "Admin";
		public const string customer = "Customer";

		public static IEnumerable<ApiScope> apiScopes =>
			new List<ApiScope>()
			{
				new ApiScope("mango", "mango client"),
				new ApiScope("read","read"),
				new ApiScope("write","write"),
				new ApiScope("delete","delete")
			};

		public static IEnumerable<IdentityResource> resources =>
			new List<IdentityResource>() {
				new IdentityResources.OpenId(),
				new IdentityResources.Email(),
				new IdentityResources.Profile()
			};
		public static IEnumerable<Client> clients =>
			new List<Client>()
			{
				new Client()
				{
					 ClientId="guest",
					  AllowedGrantTypes= GrantTypes.Code,
					  ClientSecrets={ new Secret("secret".Sha256()) },
					   AllowedScopes= new List<string>(){"read","write"}

				},
				new Client()
				{
					ClientId="mango",
					ClientSecrets={ new Secret("secret".Sha256()) },
					AllowedGrantTypes = GrantTypes.Code,
					RedirectUris={"https://localhost:44327/signin-oidc"},
					PostLogoutRedirectUris={"https://localhost:44327/signout-callback-oidc" },
					AllowedScopes=new List<string>
					{
						 IdentityServerConstants.StandardScopes.OpenId,
						 IdentityServerConstants.StandardScopes.Email,
						 IdentityServerConstants.StandardScopes.Profile,
						 "mango"
					}
				}
			};
	}
}
