#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2020                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Tests
{
	#region usings

	using System;
	using System.Threading.Tasks;
	using AutoFixture;
	using Newtonsoft.Json;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;
	using Zeiss.PiWeb.Api.Rest.HttpClient.Data;

	#endregion

	[TestFixture]
	public sealed class SimpleRequestTest
	{
		#region constants

		private const int Port = 8081;

		#endregion

		#region members

		private static readonly Uri Uri = new Uri( $"http://localhost:{Port}/" );
		private static readonly Fixture Fixture = new Fixture();

		#endregion

		#region constructors

		static SimpleRequestTest()
		{
			Fixture.Register<AbstractAttributeDefinitionDto>( () => Fixture.Create<AttributeDefinitionDto>() );
		}

		#endregion

		#region methods

		[Test]
		public async Task RequestConfiguration()
		{
			// given
			using var server = WebServer.StartNew( Port );
			using var client = new DataServiceRestClient( Uri );

			var config = Fixture.Create<ConfigurationDto>();

			server.RegisterResponse( "/DataServiceRest/configuration", JsonConvert.SerializeObject( config ) );

			// when
			var result = await client.GetConfiguration();

			// then
			Assert.That( JsonConvert.SerializeObject( result ), Is.EqualTo( JsonConvert.SerializeObject( config ) ) );
		}

		#endregion
	}
}