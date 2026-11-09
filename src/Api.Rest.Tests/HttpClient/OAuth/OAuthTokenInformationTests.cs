#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2026                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Tests.HttpClient.OAuth
{
	#region usings

	using System.Text.Json;
	using FluentAssertions;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.HttpClient.OAuth;

	#endregion

	[TestFixture]
	public class OAuthTokenInformationTests
	{
		#region methods

		[Test]
		public void Serialize_NewClientScopes_UsesExpectedJsonProperties()
		{
			var tokenInformation = new OAuthTokenInformation
			{
				BrowserClientRequestedScopes = "browser-scope",
				BrowserCompatibilityClientRequestedScopes = "compatibility-scope"
			};

			using var jsonDocument = JsonDocument.Parse( JsonSerializer.Serialize( tokenInformation ) );

			jsonDocument.RootElement.GetProperty( "browserClientRequestedScopes" ).GetString().Should().Be( "browser-scope" );
			jsonDocument.RootElement.GetProperty( "browserCompatibilityClientRequestedScopes" ).GetString().Should().Be( "compatibility-scope" );
		}

		#endregion
	}
}
